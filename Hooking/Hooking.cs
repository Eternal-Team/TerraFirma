using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using Terraria;

namespace TerraFirma
{
	internal static partial class Hooking
	{
		private static RenderTarget2D[] playerTargets;
		internal static GameTime gameTime;

		internal static void Initialize()
		{
			playerTargets = new RenderTarget2D[Main.player.Length];

			On.Terraria.Main.EnsureRenderTargetContent += Main_EnsureRenderTargetContent;
			Main.OnRenderTargetsInitialized += InitializePlayerTargets;
			Main.OnRenderTargetsReleased += ReleasePlayerTargets;

			Main.OnPreDraw += DrawPlayersToTargets;
			On.Terraria.Main.DrawPlayers += DrawPlayerTargets;

			On.Terraria.Player.PlayerFrame += FixPlayerFrameForElevator;
			On.Terraria.Main.DrawPlayer += FixPlayerSize;

			IL.Terraria.Main.DoDraw += DrawTubes;

			On.Terraria.Main.DoUpdate += (orig, self, time) =>
			{
				gameTime = time;

				orig(self, gameTime);
			};

			IL.Terraria.Player.Update += Player_Update;

			Dispatcher.Dispatch(() => Main.instance.InvokeMethod<object>("InitTargets"));
		}

		internal static void Uninitialize()
		{
			Main.OnRenderTargetsInitialized -= InitializePlayerTargets;
			Main.OnRenderTargetsReleased -= ReleasePlayerTargets;
		}

		private static void DrawTubes(ILContext il)
		{
			ILCursor cursor = new ILCursor(il);

			if (cursor.TryGotoNext(i => i.MatchCall<Main>("DrawWoF")))
			{
				cursor.EmitDelegate<Action>(() =>
				{
					SpriteBatchState state = Utility.End(Main.spriteBatch);
					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

					TerraFirma.Instance.TubeNetworkLayer.Draw(Main.spriteBatch);

					foreach (Player player in Main.player)
					{
						if (player == null || !player.active || player.dead || playerTargets[player.whoAmI].IsDisposed) continue;

						if (player.GetModPlayer<TFPlayer>().Transporting)
						{
							var target = playerTargets[player.whoAmI];
							float alpha = player.GetModPlayer<TFPlayer>().alpha;
							Main.spriteBatch.Draw(target,
								new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f
								- new Vector2(6, 8)
								, null, Color.White * alpha, 0f, target.Size() * 0.5f, 0.5f, SpriteEffects.None, 0f);

							// note: rotate the entire spritebatch instead of using player.fullRotation?
						}
					}

					TerraFirma.Instance.TubeNetworkLayer.PostDraw(Main.spriteBatch);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(state);
				});
			}
		}
	}
}