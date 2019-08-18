using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Linq;
using Terraria;

namespace TerraFirma
{
	internal static partial class Hooking
	{
		private static RenderTarget2D[] playerTargets;
		private static bool DrawToTarget;

		internal static GameTime gameTime;

		internal static void Initialize()
		{
			playerTargets = new RenderTarget2D[Main.player.Length];

			Main.OnRenderTargetsInitialized += InitializePlayerTargets;
			Main.OnRenderTargetsReleased += ReleasePlayerTargets;
			Main.OnPreDraw += Main_OnPreDraw;

			On.Terraria.Player.PlayerFrame += Player_PlayerFrame;

			On.Terraria.Main.DrawPlayers += Main_DrawPlayers;
			On.Terraria.Main.DrawPlayer_DrawAllLayers += DrawPlayerToTarget;
			On.Terraria.Main.DrawPlayer += FixPlayerSize;

			On.Terraria.Main.DrawPlayer += Main_DrawPlayer;

			On.Terraria.Main.EnsureRenderTargetContent += Main_EnsureRenderTargetContent;

			IL.Terraria.Main.DoDraw += DrawTubes;

			On.Terraria.Main.DoUpdate += Main_DoUpdate;

			IL.Terraria.Player.Update += Player_Update;

			Dispatcher.Dispatch(() => Main.instance.InvokeMethod<object>("InitTargets"));
		}

		private static void Main_DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, GameTime gameTime)
		{
			Hooking.gameTime = gameTime;

			orig(self, gameTime);
		}

		private static void Player_Update(ILContext il)
		{
			ILCursor cursor = new ILCursor(il);

			if (cursor.TryGotoNext(i => i.MatchLdarg(0), i => i.MatchLdfld<Player>("mount"), i => i.MatchCallvirt<Mount>("get_Active"), i => i.MatchBrfalse(out _)))
			{
				cursor.Emit(OpCodes.Ldarg, 0);

				cursor.EmitDelegate<Action<Player>>(TFPlayer.Update);
			}

			/*
			IL_879D: ldarg.0
			IL_879E: ldfld     class Terraria.Mount Terraria.Player::mount
			IL_87A3: callvirt  instance bool Terraria.Mount::get_Active()
			IL_87A8: brfalse.s IL_87BB
			*/
		}

		private static void Main_DrawPlayer(On.Terraria.Main.orig_DrawPlayer orig, Main self, Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
		{
			orig(self, drawPlayer, Position, rotation, rotationOrigin, shadow);
		}

		private static void Player_PlayerFrame(On.Terraria.Player.orig_PlayerFrame orig, Player self)
		{
			if (self.GetModPlayer<TFPlayer>().UsingElevator)
			{
				float velocityY = self.velocity.Y;
				self.velocity.Y = 0f;

				orig(self);

				self.velocity.Y = velocityY;
			}
			else orig(self);
		}

		private static void Main_OnPreDraw(GameTime obj)
		{
			// todo: maybe neatify this
			DrawToTarget = true;
			Main.instance.InvokeMethod<object>("DrawPlayers");
			DrawToTarget = false;
		}

		internal static void Uninitialize()
		{
			Main.OnRenderTargetsInitialized -= InitializePlayerTargets;
			Main.OnRenderTargetsReleased -= ReleasePlayerTargets;
		}

		private static void Main_DrawPlayers(On.Terraria.Main.orig_DrawPlayers orig, Main self)
		{
			if (DrawToTarget) orig(self);
			else
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
				for (int i = 0; i < playerTargets.Length; i++)
				{
					RenderTarget2D target = playerTargets[i];
					Player player = Main.player[i];
					if (target == null || player == null || !player.active || player.GetModPlayer<TFPlayer>().Transporting) continue;

					float scale = player.GetModPlayer<TFPlayer>().scale;
					float alpha = player.GetModPlayer<TFPlayer>().alpha;

					Main.spriteBatch.Draw(target, new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f, null, Color.White * alpha, 0f, target.Size() * 0.5f, scale, SpriteEffects.None, 0f);
				}

				Main.spriteBatch.End();
			}
		}

		private static void Main_EnsureRenderTargetContent(On.Terraria.Main.orig_EnsureRenderTargetContent orig, Main self)
		{
			if (
				Main.waterTarget == null || Main.waterTarget.IsContentLost ||
				self.backWaterTarget == null || self.backWaterTarget.IsContentLost ||
				self.blackTarget == null || self.blackTarget.IsContentLost ||
				self.tileTarget == null || self.tileTarget.IsContentLost ||
				self.tile2Target == null || self.tile2Target.IsContentLost ||
				self.wallTarget == null || self.wallTarget.IsContentLost ||
				self.backgroundTarget == null || self.backgroundTarget.IsContentLost ||
				Main.screenTarget == null || Main.screenTarget.IsContentLost ||
				Main.screenTargetSwap == null || Main.screenTargetSwap.IsContentLost ||
				playerTargets.Any(target => target == null || target.IsContentLost))
			{
				self.InvokeMethod<object>("InitTargets");
			}
		}

		private static void InitializePlayerTargets(int width, int height)
		{
			for (int i = 0; i < playerTargets.Length; i++)
			{
				playerTargets[i] = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
			}
		}

		private static void ReleasePlayerTargets()
		{
			foreach (RenderTarget2D target in playerTargets) target?.Dispose();
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