using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace TerraFirma
{
	public static class Hooking
	{
		internal static RenderTarget2D[] playerTargets;

		internal static void Initialize()
		{
			playerTargets = new RenderTarget2D[Main.player.Length];

			On.Terraria.Main.DrawPlayer += Main_DrawPlayer;
			On.Terraria.Main.DrawPlayer_DrawAllLayers += Main_DrawPlayer_DrawAllLayers;

			Main.graphics.PreparingDeviceSettings += SetPreserveContents;

			Main.OnRenderTargetsInitialized += InitializePlayerTargets;
			Main.OnRenderTargetsReleased += ReleasePlayerTargets;

			IL.Terraria.Main.DoDraw += Main_DoDraw;

			Scheduler.EnqueueMessage(() =>
			{
				Main.ToggleFullScreen();
				Main.ToggleFullScreen();
			});
		}

		private static void Main_DoDraw(ILContext il)
		{
			ILCursor cursor = new ILCursor(il);

			if (cursor.TryGotoNext(
				i => i.MatchCall<Main>("DrawWoF"))
			)
			{
				cursor.EmitDelegate<Action>(() =>
				{
					SpriteBatchState state = Utility.End(Main.spriteBatch);
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

					TerraFirma.Instance.TubeNetworkLayer.Draw(Main.spriteBatch);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(state);
				});
			}
		}

		internal static void Uninitialize()
		{
			Main.graphics.PreparingDeviceSettings -= SetPreserveContents;

			Main.OnRenderTargetsInitialized -= InitializePlayerTargets;
			Main.OnRenderTargetsReleased -= ReleasePlayerTargets;
		}

		private static void InitializePlayerTargets(int width, int height)
		{
			for (int i = 0; i < playerTargets.Length; i++)
			{
				ref RenderTarget2D target = ref playerTargets[i];
				if (target != null) target = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height);
			}
		}

		private static void ReleasePlayerTargets()
		{
			foreach (RenderTarget2D target in playerTargets)
			{
				target?.Dispose();
			}
		}

		private static void SetPreserveContents(object sender, PreparingDeviceSettingsEventArgs args) => args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

		private static void Main_DrawPlayer_DrawAllLayers(On.Terraria.Main.orig_DrawPlayer_DrawAllLayers orig, Main self, Player drawPlayer, int projectileDrawPosition, int cHead)
		{
			RenderTarget2D target = playerTargets[drawPlayer.whoAmI] ?? new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);

			Main.spriteBatch.End();
			Main.graphics.GraphicsDevice.SetRenderTarget(target);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

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
					if (value.texture != null) value.Draw(Main.spriteBatch);
				}
			}

			Main.spriteBatch.End();
			Main.graphics.GraphicsDevice.SetRenderTarget(null);

			float scale = drawPlayer.GetModPlayer<TFPlayer>().scale;

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			Main.spriteBatch.Draw(target, new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f + new Vector2(0, 8 - 8 * scale), null, Color.White, 0f, target.Size() * 0.5f, scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
		}

		private static void Main_DrawPlayer(On.Terraria.Main.orig_DrawPlayer orig, Main self, Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
		{
			int playerWidth = drawPlayer.width;
			int playerHeight = drawPlayer.height;

			drawPlayer.width = Player.defaultWidth;
			drawPlayer.height = Player.defaultHeight;

			if (drawPlayer.GetModPlayer<TFPlayer>().Miniaturizing) drawPlayer.wingFrame = 0;

			orig(self, drawPlayer, Position, rotation, new Vector2(drawPlayer.width, drawPlayer.height) * 0.5f, shadow);

			drawPlayer.width = playerWidth;
			drawPlayer.height = playerHeight;
		}
	}
}