using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace TerraFirma.Network
{
	// bug: entering/exiting looks weird

	public class TransportingPlayer
	{
		public TFPlayer player;

		public Stack<Point16> path;
		public Point16 CurrentPosition;
		public Point16 PreviousPosition;

		public const int speed = 10;
		public int timer = speed;

		public TransportingPlayer(Player player, Stack<Point16> path)
		{
			this.player = player.GetModPlayer<TFPlayer>();
			this.path = path;
			CurrentPosition = PreviousPosition = path.Pop();

			this.player.Entering = true;
			this.player.transportingPlayer = this;
		}

		public void Update()
		{
			if (!player.Transporting) return;

			if (++timer >= speed)
			{
				PreviousPosition = CurrentPosition;

				if (path.Count == 0)
				{
					player.alpha = 0f;
					player.Exiting = true;
				}
				else
				{
					CurrentPosition = path.Pop();
					player.alpha = 1f;
				}

				timer = 0;
			}
		}
	}
}