using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace TerraFirma
{
	public static class Hooking
	{
		internal static void Initialize()
		{
			On.Terraria.Main.DrawPlayer += Main_DrawPlayer;
			On.Terraria.Main.DrawPlayer_DrawAllLayers += Main_DrawPlayer_DrawAllLayers;

			Main.graphics.PreparingDeviceSettings += SetPreserveContents;
		}

		internal static void Uninitialize()
		{
			Main.graphics.PreparingDeviceSettings -= SetPreserveContents;
		}

		internal static void SetPreserveContents(object sender, PreparingDeviceSettingsEventArgs args) => args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

		// need render target for each player
		static RenderTarget2D target;

		internal static void Main_DrawPlayer_DrawAllLayers(On.Terraria.Main.orig_DrawPlayer_DrawAllLayers orig, Main self, Player drawPlayer, int projectileDrawPosition, int cHead)
		{
			try
			{
				if (target == null)
					target = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);

				Main.spriteBatch.End();
				Main.graphics.GraphicsDevice.SetRenderTarget(target);
				Main.graphics.GraphicsDevice.Clear(Color.Transparent);

				Main.spriteBatch.Begin();

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
						if (!value.sourceRect.HasValue)
						{
							value.sourceRect = new Microsoft.Xna.Framework.Rectangle?(value.texture.Frame(1, 1, 0, 0));
						}
						if (value.shader >= 0)
						{
							GameShaders.Hair.Apply(0, drawPlayer, new DrawData?(value));
							GameShaders.Armor.Apply(value.shader, drawPlayer, new DrawData?(value));
						}
						else if (drawPlayer.head == 0)
						{
							GameShaders.Hair.Apply(0, drawPlayer, new DrawData?(value));
							GameShaders.Armor.Apply(cHead, drawPlayer, new DrawData?(value));
						}
						else
						{
							GameShaders.Armor.Apply(0, drawPlayer, new DrawData?(value));
							GameShaders.Hair.Apply((short)(-(short)value.shader), drawPlayer, new DrawData?(value));
						}
						num = value.shader;
						if (value.texture != null)
						{
							value.Draw(Main.spriteBatch);
						}
					}
				}

				Main.spriteBatch.End();
				Main.graphics.GraphicsDevice.SetRenderTarget(null);

				Main.spriteBatch.Begin();
				Main.spriteBatch.Draw(target, new Vector2(Main.screenWidth, Main.screenHeight) * (0.25f), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
			}
			catch
			{

			}
		}

		// reset player size for drawing
		internal static void Main_DrawPlayer(On.Terraria.Main.orig_DrawPlayer orig, Main self, Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
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
