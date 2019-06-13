using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.ModLoader;

namespace TerraFirma
{
	public class TFPlayer : ModPlayer
	{
		public bool UsingTubeSystem => Miniaturizing || Maximizing || InTube;

		public bool Miniaturizing = false;
		public bool Maximizing = false;
		public bool InTube;

		public float scale = 1f;

		public override void PreUpdate()
		{
			Miniaturizing = true;
			if (UsingTubeSystem)
			{
				player.width = (int)(Player.defaultWidth * scale);

				player.position.Y += player.height;
				player.height = (int)(Player.defaultHeight * scale);
				player.position.Y -= player.height;

				player.immune = true;
				player.immuneTime = 2;
				player.immuneNoBlink = true;
			}

			if (Miniaturizing)
			{
				float maxScale = 0.5f;
				if (scale > maxScale) scale -= 0.05f;
				if (scale < maxScale) scale += 0.05f;
				else InTube = true;

				//player.velocity = new Vector2(-0.001f);

				//player.fullRotation = 0f;

				//player.gravity = 0f;
				//player.fallStart = 0;
				//player.jump = 0;
				//player.fallStart2 = 0;
				//player.wings = 0;
				//player.wingsLogic = 0;
				//player.wingTime = 0;
				//player.wingFrame = 0;
			}
		}

		public override void SetControls()
		{
			if (UsingTubeSystem)
			{
				//player.controlJump = false;
				//player.controlDown = false;
				//player.controlLeft = false;
				//player.controlRight = false;
				//player.controlUp = false;
				//player.controlUseItem = false;
				//player.controlUseTile = false;
				//player.controlThrow = false;
				//player.gravDir = 1f;
			}
		}
	}
}