using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace TerraFirma.TubularNetwork
{
	// player is not centered on tube
	// entering/exiting looks weird
	// wings get drawn open
	// mount doesn't get disabled

	public class TransportingPlayer
	{
		public Player player;
		public Stack<Point16> path;
		public Point16 CurrentPosition;
		private Point16 PreviousPosition;

		public const int speed = 5;
		private int timer = speed;

		public void Update()
		{
			if (!player.GetModPlayer<TFPlayer>().Transporting) return;

			if (PreviousPosition == Point16.Zero) PreviousPosition = CurrentPosition;

			Vector2 prevPos = PreviousPosition.ToVector2() * 16 + new Vector2(24, 24);
			Vector2 nextPos = CurrentPosition.ToVector2() * 16 + new Vector2(24, 24);
			player.Center = Vector2.Lerp(prevPos,nextPos , (float)timer / speed);
			player.fullRotation = Vector2.Normalize(nextPos - prevPos).ToRotation()+MathHelper.PiOver2;
			if (player.fullRotation > MathHelper.Pi) player.direction = -1;

			if (++timer > speed)
			{
				if (path.Count == 0) player.GetModPlayer<TFPlayer>().Maximizing = true;
				else
				{
					PreviousPosition = CurrentPosition;
					CurrentPosition = path.Pop();
				}

				timer = 0;
			}
		}
	}
}