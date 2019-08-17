using BaseLibrary;
using BaseLibrary.Tiles.TileEntites;
using BaseLibrary.UI;
using ContainerLibrary;
using EnergyLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraFirma.TileEntities
{
	public class Quarry : BaseTE, IItemHandler, IHasUI, IEnergyHandler
	{
		//public int pickPower = 30;
		//public int speed = 30;
		//public bool removeWalls;
		//public bool removeLiquids;
		//public bool autoplaceLight;

		public Point CurrentTile;

		public override Type TileType => typeof(Tiles.Quarry);

		public ItemHandler Handler { get; }
		public EnergyHandler EnergyHandler { get; }

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound => SoundID.Item1;
		public LegacySoundStyle OpenSound => SoundID.Item1;

		public bool Active = true;
		private int timer;
		public float Angle;
		public bool Targetted;
		public float Lenght;

		private int radius;

		private List<Point> NextTiles;

		public Quarry()
		{
			Handler = new ItemHandler(27);
			EnergyHandler = new EnergyHandler(long.MaxValue, 10000);
		}

		public override void Update()
		{
			EnergyHandler.SetMaxTransfer(long.MaxValue);
			EnergyHandler.InsertEnergy(Main.rand.Next(1000, 1000000));

			if (!Active) return;

			if (CurrentTile == default) CurrentTile = new Point(Position.X - 16, Position.Y + 4);

			if (++timer > 5 && Targetted)
			{
				timer = 0;

				if (NextTiles == null || NextTiles.Count == 0)
				{
					NextTiles = GetHemicircleTiles(Position.X + 1, Position.Y + 4, radius).OrderBy(point => point.X).ThenBy(point => point.X > Position.X + 1 ? -point.Y : point.Y).ToList();

					radius++;
					// deactivate
					if (radius > 16)
					{
						radius = 0;
						for (int x = Position.X - 20; x < Position.X + 20; x++)
						{
							for (int y = Position.Y + 4; y < Position.Y + 24; y++)
							{
								WorldGen.PlaceTile(x, y, TileID.Dirt);
							}
						}
					}
				}

				CurrentTile = NextTiles[0];
				NextTiles.RemoveAt(0);
				WorldGen.KillTile(CurrentTile.X, CurrentTile.Y);

				for (int i = 0; i < Main.item.Length; i++)
				{
					ref Item item = ref Main.item[i];
					if (item != null && item.active && new Rectangle(CurrentTile.X * 16, CurrentTile.Y * 16, 16, 16).Intersects(item.getRect()))
					{
						Handler.InsertItem(ref item);
					}
				}
			}

			Vector2 position = Position.ToScreenCoordinates();
			Vector2 targetPosition = CurrentTile.ToScreenCoordinates();

			float targetAngle = (targetPosition + new Vector2(8) - (position + new Vector2(24))).ToRotation();
			Angle = Angle.AngleLerp(targetAngle, 0.25f);

			float targetLenght = Vector2.Distance(position + new Vector2(24) + 30 * Angle.ToRotationVector2(), targetPosition + new Vector2(16));
			Lenght = MathHelper.Lerp(Lenght, targetLenght, 0.25f);

			Targetted = Math.Abs(Math.Abs(Angle) - Math.Abs(targetAngle)) < 7.5f.ToRadians();

			//if (active && ++timer >= speed)
			//{
			//	if (currentX != -1 && currentY != -1 && currentX <= end.X && Utility.NextTile(ref currentX, ref currentY, end.X, end.Y, minY: start.Y))
			//	{
			//		Tile tile = Main.tile[currentX, currentY];
			//		int damage = 1;
			//		TileLoader.PickPowerCheck(tile, pickPower, ref damage);
			//		if (damage > 0)
			//		{
			//			if (TheOneLibrary.Utility.Utility.IsChest(tile.type)) TransferItemFromChest();
			//			WorldGen.KillTile(currentX, currentY);
			//		}

			//		if (removeWalls) WorldGen.KillWall(currentX, currentY);
			//		if (removeLiquids) tile.liquid = 0;
			//		if (autoplaceLight && Math.Abs(currentX - start.X) % 10 == 0 && Math.Abs(currentY - start.Y) % 10 == 0) WorldGen.PlaceTile(currentX, currentY, mod.TileType<LightTile>());
			//	}
			//}
		}

		public IEnumerable<Point> GetHemicircleTiles(int centerX, int centerY, int radius)
		{
			var x = 0;
			var y = radius;
			var d = -(int)((uint)radius >> 1);

			while (x <= y)
			{
				yield return new Point(x + centerX, y + centerY);
				yield return new Point(y + centerX, x + centerY);
				yield return new Point(-x + centerX, y + centerY);
				yield return new Point(-y + centerX, x + centerY);

				if (d <= 0)
				{
					x++;
					d += x;
				}
				else
				{
					y--;
					d -= y;
				}
			}
		}
	}
}