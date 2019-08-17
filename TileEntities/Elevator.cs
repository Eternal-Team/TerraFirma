using BaseLibrary.Tiles.TileEntites;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace TerraFirma.TileEntities
{
	public class Elevator : BaseTE
	{
		public override Type TileType => typeof(Elevator);

		public Vector2 position;
		public Vector2 oldPosition;

		private int progress;
		private int speed = 1;

		private static float gauss(float x, float a, float b, float c)
		{
			var v1 = (x - b) / (2d * c * c);
			var v2 = -v1 * v1 / 2d;
			var v3 = a * (float)Math.Exp(v2);

			return v3;
		}

		public override void Update()
		{
			if (position == Vector2.Zero || oldPosition == Vector2.Zero) position = oldPosition = Position.ToVector2() * 16;

			if (progress > 160 || progress < 0) speed *= -1;
			progress += speed;

			oldPosition = position;

			float avg = 80f;
			float amp = 10f;
			float sd = 3.5f;

			float s = gauss(progress, amp, avg, sd) * -speed;
			position.Y += s;
		}
	}
}