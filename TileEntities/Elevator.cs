using BaseLibrary;
using BaseLibrary.Tiles.TileEntites;
using Microsoft.Xna.Framework;
using System;
using TerraFirma.Tiles;
using Terraria;

namespace TerraFirma.TileEntities
{
	public class Elevator : BaseTE
	{
		public override Type TileType => typeof(Elevator);

		public Vector2 position;
		public Vector2 oldPosition;

		public int direction = 0;

		private float velocity;

		public override void Update()
		{
			// also needs to check for solid tiles in elevator area

			int index = 1;
			while (Main.tile[Position.X, Position.Y - index].type == mod.TileType<ElevatorRail>() && Main.tile[Position.X + 4, Position.Y - index].type == mod.TileType<ElevatorRail>()) index++;
			index--;

			if (position == Vector2.Zero || oldPosition == Vector2.Zero) position = oldPosition = Position.ToVector2() * 16 - new Vector2(16f, index * 16f);

			oldPosition = position;
			if (direction == 0) return;

			position.Y = Utility.SmoothDamp(position.Y, direction == -1 ? Position.Y * 16 - index * 16 + 2f : Position.Y * 16, ref velocity, 0.5f, 1500f, 0.01666667f);
			Main.NewText(position.Y);
		}
	}
}