using System;
using BaseLibrary.Tiles.TileEntites;
using Microsoft.Xna.Framework;
using TerraFirma.Tiles;
using Terraria;
using Terraria.DataStructures;

namespace TerraFirma.TileEntities
{
	public class TEEntryPoint : BaseTE
	{
		public Rectangle Hitbox => new Rectangle(Position.X * 16, Position.Y * 16 - 48, 48, 48);

		public override Type TileType => typeof(EntryPoint);

		public static float t;
		public static Point16 Position;

		public override void Update()
		{
			t -= 0.05f;
			Position = base.Position;

            foreach (Player player in Main.player)
			{
				if (player.active && !player.dead)
				{
					TFPlayer tfPlayer = player.GetModPlayer<TFPlayer>();
                    tfPlayer.Miniaturizing = false;
				}
			}
		}
	}
}