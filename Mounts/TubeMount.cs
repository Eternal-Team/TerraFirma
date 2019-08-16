using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraFirma.Mounts
{
	public class TubeMount : ModMountData
	{
		public const float speed = 10f;

		public override void SetDefaults()
		{
			mountData.heightBoost = 0;
			mountData.flightTimeMax = int.MaxValue;
			mountData.fatigueMax = int.MaxValue;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = speed;
			mountData.dashSpeed = speed;
			mountData.acceleration = speed;
			mountData.swimSpeed = speed;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 8f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 1;
			mountData.playerYOffsets = new[] { 0 };
			mountData.xOffset = 0;
			mountData.bodyFrame = 5;
			mountData.yOffset = 0;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 0;
			mountData.standingFrameDelay = 0;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 0;
			mountData.runningFrameDelay = 0;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 0;
			mountData.swimFrameStart = 0;

			if (Main.netMode != NetmodeID.Server)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
	}
}