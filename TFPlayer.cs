using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraFirma
{
	public class TFPlayer : ModPlayer
	{
		public bool UsingTubeSystem => Miniaturizing || Maximizing || Transporting;

		public bool Miniaturizing;
		public bool Maximizing;
		public bool Transporting;

		public float scale = 1f;
		public const float transferScale = 0.5f;

		public override void PreUpdate()
		{
			if (UsingTubeSystem)
			{
				player.width = (int)(Player.defaultWidth * scale);

				player.position.Y += player.height;
				player.height = (int)(Player.defaultHeight * scale);
				player.position.Y -= player.height;

				player.immune = true;
				player.immuneTime = 2;
				player.immuneNoBlink = true;

				player.velocity = new Vector2(0f, 0.0001f);

				player.fullRotation = 0f;

				player.gravity = 0f;
				player.fallStart = 0;
				player.jump = 0;
				player.fallStart2 = 0;
				player.wings = 0;
				player.wingsLogic = 0;
				player.wingTime = 0;
				player.wingFrame = 0;

				player.mount?.Dismount(player);
			}

			if (Maximizing)
			{
				Transporting = false;

				if (scale < 1f) scale += 0.001f;
				else Maximizing = false;
			}

			if (Miniaturizing)
			{
				if (scale > transferScale) scale -= 0.001f;
				else
				{
					Miniaturizing = false;
					Transporting = true;
				}
			}
		}

		public override void SetControls()
		{
			if (UsingTubeSystem)
			{
				player.controlJump = false;
				player.controlDown = false;
				player.controlLeft = false;
				player.controlRight = false;
				player.controlUp = false;
				player.controlUseItem = false;
				player.controlUseTile = false;
				player.controlThrow = false;
				player.gravDir = 1f;
			}
		}
	}
}