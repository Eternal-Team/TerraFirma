using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace TerraFirma
{
	internal static partial class Hooking
	{
		private static void DrawPlayerToTarget(On.Terraria.Main.orig_DrawPlayer_DrawAllLayers orig, Main self, Player drawPlayer, int projectileDrawPosition, int cHead)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;

			spriteBatch.End();

			var originalTargets = graphicsDevice.GetRenderTargets();

			ref RenderTarget2D playerTarget = ref playerTargets[drawPlayer.whoAmI];
			//if (playerTarget == null || playerTarget.IsContentLost || playerTarget.IsDisposed) playerTarget = new RenderTarget2D(graphicsDevice, Main.screenWidth, Main.screenHeight);

			graphicsDevice.SetRenderTarget(playerTarget);
			graphicsDevice.Clear(Color.Transparent);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

			int num = -1;
			for (int i = 0; i <= Main.playerDrawData.Count; i++)
			{
				if (projectileDrawPosition == i)
				{
					if (num != 0)
					{
						Main.pixelShader.CurrentTechnique.Passes[0].Apply();
						num = 0;
					}

					Main.projectile[drawPlayer.heldProj].gfxOffY = drawPlayer.gfxOffY;
					try
					{
						self.DrawProj(drawPlayer.heldProj);
					}
					catch
					{
						Main.projectile[drawPlayer.heldProj].active = false;
					}
				}

				if (i != Main.playerDrawData.Count)
				{
					DrawData value = Main.playerDrawData[i];

					if (!value.sourceRect.HasValue) value.sourceRect = value.texture.Frame();

					if (value.shader >= 0)
					{
						GameShaders.Hair.Apply(0, drawPlayer, value);
						GameShaders.Armor.Apply(value.shader, drawPlayer, value);
					}
					else if (drawPlayer.head == 0)
					{
						GameShaders.Hair.Apply(0, drawPlayer, value);
						GameShaders.Armor.Apply(cHead, drawPlayer, value);
					}
					else
					{
						GameShaders.Armor.Apply(0, drawPlayer, value);
						GameShaders.Hair.Apply((short)-(short)value.shader, drawPlayer, value);
					}

					num = value.shader;
					if (value.texture != null) value.Draw(spriteBatch);
				}
			}

			spriteBatch.End();

			graphicsDevice.SetRenderTargets(originalTargets);
			spriteBatch.Begin();
		}

		private static void FixPlayerSize(On.Terraria.Main.orig_DrawPlayer orig, Main self, Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
		{
			int playerWidth = drawPlayer.width;
			int playerHeight = drawPlayer.height;

			drawPlayer.width = Player.defaultWidth;
			drawPlayer.height = Player.defaultHeight;

			orig(self, drawPlayer, Position, rotation, new Vector2(drawPlayer.width, drawPlayer.height) * 0.5f, shadow);

			drawPlayer.width = playerWidth;
			drawPlayer.height = playerHeight;
		}
	}
}