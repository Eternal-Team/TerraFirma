using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using On.Terraria;
using System;
using System.Linq;

namespace TerraFirma
{
	internal static partial class Hooking
	{
		private static void FixPlayerSize(Main.orig_DrawPlayer orig, Terraria.Main self, Terraria.Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
		{
			int playerWidth = drawPlayer.width;
			int playerHeight = drawPlayer.height;

			drawPlayer.width = Terraria.Player.defaultWidth;
			drawPlayer.height = Terraria.Player.defaultHeight;

			orig(self, drawPlayer, Position, rotation, new Vector2(drawPlayer.width, drawPlayer.height) * 0.5f, shadow);

			drawPlayer.width = playerWidth;
			drawPlayer.height = playerHeight;
		}

		private static void ReleasePlayerTargets()
		{
			foreach (RenderTarget2D target in playerTargets) target?.Dispose();
		}

		private static void InitializePlayerTargets(int width, int height)
		{
			for (int i = 0; i < playerTargets.Length; i++)
			{
				playerTargets[i] = new RenderTarget2D(Terraria.Main.graphics.GraphicsDevice, width, height, false, Terraria.Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
			}
		}

		private static void Main_EnsureRenderTargetContent(Main.orig_EnsureRenderTargetContent orig, Terraria.Main self)
		{
			if (
				Terraria.Main.waterTarget == null || Terraria.Main.waterTarget.IsContentLost ||
				self.backWaterTarget == null || self.backWaterTarget.IsContentLost ||
				self.blackTarget == null || self.blackTarget.IsContentLost ||
				self.tileTarget == null || self.tileTarget.IsContentLost ||
				self.tile2Target == null || self.tile2Target.IsContentLost ||
				self.wallTarget == null || self.wallTarget.IsContentLost ||
				self.backgroundTarget == null || self.backgroundTarget.IsContentLost ||
				Terraria.Main.screenTarget == null || Terraria.Main.screenTarget.IsContentLost ||
				Terraria.Main.screenTargetSwap == null || Terraria.Main.screenTargetSwap.IsContentLost ||
				playerTargets.Any(target => target == null || target.IsContentLost))
			{
				self.InvokeMethod<object>("InitTargets");
			}
		}

		private static void DrawPlayerTargets(Main.orig_DrawPlayers orig, Terraria.Main self)
		{
			Terraria.Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

			for (int i = 0; i < playerTargets.Length; i++)
			{
				RenderTarget2D target = playerTargets[i];
				Terraria.Player player = Terraria.Main.player[i];

				if (target == null || player == null || !player.active || player.GetModPlayer<TFPlayer>().Transporting) continue;

				float scale = player.GetModPlayer<TFPlayer>().scale;
				float alpha = player.GetModPlayer<TFPlayer>().alpha;

				Terraria.Main.spriteBatch.Draw(target, new Vector2(Terraria.Main.screenWidth, Terraria.Main.screenHeight) * 0.5f, null, Color.White * alpha, 0f, Terraria.Utils.Size(target) * 0.5f, scale, SpriteEffects.None, 0f);
			}

			Terraria.Main.spriteBatch.End();
		}

		private static void DrawPlayersToTargets(GameTime obj)
		{
			GraphicsDevice graphicsDevice = Terraria.Main.graphics.GraphicsDevice;

			for (int i = 0; i < 255; i++)
			{
				Terraria.Player player = Terraria.Main.player[i];
				if (player.active && !player.outOfRange)
				{
					SamplerState samplerState = Terraria.Main.DefaultSamplerState;
					if (player.mount.Active) samplerState = Terraria.Main.MountedSamplerState;

					var originalTargets = graphicsDevice.GetRenderTargets();

					graphicsDevice.SetRenderTarget(playerTargets[i]);
					graphicsDevice.Clear(Color.Transparent);

					Terraria.Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Terraria.Main.instance.Rasterizer, null, Terraria.Main.GameViewMatrix.TransformationMatrix);
					if (Terraria.Main.gamePaused) player.PlayerFrame();

					if (player.ghost)
					{
						for (int j = 0; j < 3; j++) Terraria.Main.instance.InvokeMethod<object>("DrawGhost", player, player.shadowPos[j], 0.5f + 0.2f * j);

						Terraria.Main.instance.InvokeMethod<object>("DrawGhost", player, player.position, 0f);
						Terraria.Main.spriteBatch.End();
						graphicsDevice.SetRenderTargets(originalTargets);
					}
					else
					{
						if (player.inventory[player.selectedItem].flame || player.head == 137 || player.wings == 22)
						{
							player.itemFlameCount--;
							if (player.itemFlameCount <= 0)
							{
								player.itemFlameCount = 5;
								for (int j = 0; j < 7; j++)
								{
									player.itemFlamePos[j].X = Terraria.Main.rand.Next(-10, 11) * 0.15f;
									player.itemFlamePos[j].Y = Terraria.Main.rand.Next(-10, 1) * 0.35f;
								}
							}
						}

						if (player.armorEffectDrawShadowEOCShield)
						{
							int num = player.eocDash / 4;
							if (num > 3) num = 3;

							for (int j = 0; j < num; j++) Terraria.Main.instance.DrawPlayer(player, player.shadowPos[j], player.shadowRotation[j], player.shadowOrigin[j], 0.5f + 0.2f * j);
						}

						Vector2 position;
						if (player.invis)
						{
							player.armorEffectDrawOutlines = false;
							player.armorEffectDrawShadow = false;
							player.armorEffectDrawShadowSubtle = false;
							position = player.position;
							if (player.aggro <= -750) Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin, 1f);
							else
							{
								player.invis = false;
								Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin);
								player.invis = true;
							}
						}

						if (player.armorEffectDrawOutlines)
						{
							if (!Terraria.Main.gamePaused) player.ghostFade += player.ghostDir * 0.075f;

							if (player.ghostFade < 0.1)
							{
								player.ghostDir = 1f;
								player.ghostFade = 0.1f;
							}
							else if (player.ghostFade > 0.9)
							{
								player.ghostDir = -1f;
								player.ghostFade = 0.9f;
							}

							float shadowOffset = player.ghostFade * 5f;
							for (int j = 0; j < 4; j++)
							{
								float offX;
								float offY;
								switch (j)
								{
									default:
										offX = shadowOffset;
										offY = 0f;
										break;
									case 1:
										offX = -shadowOffset;
										offY = 0f;
										break;
									case 2:
										offX = 0f;
										offY = shadowOffset;
										break;
									case 3:
										offX = 0f;
										offY = -shadowOffset;
										break;
								}

								position = new Vector2(player.position.X + offX, player.position.Y + player.gfxOffY + offY);
								Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin, player.ghostFade);
							}
						}

						if (player.armorEffectDrawOutlinesForbidden)
						{
							Vector2 arg_39F_0 = player.position;
							if (!Terraria.Main.gamePaused)
							{
								player.ghostFade += player.ghostDir * 0.025f;
							}

							if (player.ghostFade < 0.1)
							{
								player.ghostDir = 1f;
								player.ghostFade = 0.1f;
							}
							else if (player.ghostFade > 0.9)
							{
								player.ghostDir = -1f;
								player.ghostFade = 0.9f;
							}

							float num5 = player.ghostFade * 5f;
							for (int n = 0; n < 4; n++)
							{
								float num6;
								float num7;
								switch (n)
								{
									case 0:
									default:
										num6 = num5;
										num7 = 0f;
										break;
									case 1:
										num6 = -num5;
										num7 = 0f;
										break;
									case 2:
										num6 = 0f;
										num7 = num5;
										break;
									case 3:
										num6 = 0f;
										num7 = -num5;
										break;
								}

								position = new Vector2(player.position.X + num6, player.position.Y + player.gfxOffY + num7);
								Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin, player.ghostFade);
							}
						}

						if (player.armorEffectDrawShadowBasilisk)
						{
							int num8 = (int)(player.basiliskCharge * 3f);
							for (int j = 0; j < num8; j++) Terraria.Main.instance.DrawPlayer(player, player.shadowPos[j], player.shadowRotation[j], player.shadowOrigin[j], 0.5f + 0.2f * j);
						}
						else if (player.armorEffectDrawShadow)
						{
							for (int j = 0; j < 3; j++) Terraria.Main.instance.DrawPlayer(player, player.shadowPos[j], player.shadowRotation[j], player.shadowOrigin[j], 0.5f + 0.2f * j);
						}

						if (player.armorEffectDrawShadowLokis)
						{
							for (int j = 0; j < 3; j++) Terraria.Main.instance.DrawPlayer(player, Vector2.Lerp(player.shadowPos[j], player.position + new Vector2(0f, player.gfxOffY), 0.5f), player.shadowRotation[j], player.shadowOrigin[j], MathHelper.Lerp(1f, 0.5f + 0.2f * j, 0.5f));
						}

						if (player.armorEffectDrawShadowSubtle)
						{
							for (int j = 0; j < 4; j++)
							{
								position.X = player.position.X + Terraria.Main.rand.Next(-20, 21) * 0.1f;
								position.Y = player.position.Y + Terraria.Main.rand.Next(-20, 21) * 0.1f + player.gfxOffY;
								Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin, 0.9f);
							}
						}

						if (player.shadowDodge)
						{
							player.shadowDodgeCount += 1f;
							if (player.shadowDodgeCount > 30f)
							{
								player.shadowDodgeCount = 30f;
							}
						}
						else
						{
							player.shadowDodgeCount -= 1f;
							if (player.shadowDodgeCount < 0f) player.shadowDodgeCount = 0f;
						}

						if (player.shadowDodgeCount > 0f)
						{
							position.X = player.position.X + player.shadowDodgeCount;
							position.Y = player.position.Y + player.gfxOffY;
							Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin, 0.5f + Terraria.Main.rand.Next(-10, 11) * 0.005f);
							position.X = player.position.X - player.shadowDodgeCount;
							Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin, 0.5f + Terraria.Main.rand.Next(-10, 11) * 0.005f);
						}

						position = player.position;
						position.Y += player.gfxOffY;
						if (player.stoned) Terraria.Main.instance.InvokeMethod<object>("DrawPlayerStoned", player, position);
						else if (!player.invis) Terraria.Main.instance.DrawPlayer(player, position, player.fullRotation, player.fullRotationOrigin);

						Terraria.Main.spriteBatch.End();
						graphicsDevice.SetRenderTargets(originalTargets);
					}
				}
			}

			Terraria.TimeLogger.DetailedDrawTime(21);
		}

		private static void FixPlayerFrameForElevator(Player.orig_PlayerFrame orig, Terraria.Player self)
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

		private static void Player_Update(ILContext il)
		{
			ILCursor cursor = new ILCursor(il);

			if (cursor.TryGotoNext(i => i.MatchLdarg(0), i => i.MatchLdfld<Terraria.Player>("mount"), i => i.MatchCallvirt<Terraria.Mount>("get_Active"), i => i.MatchBrfalse(out _)))
			{
				cursor.Emit(OpCodes.Ldarg, 0);

				cursor.EmitDelegate<Action<Terraria.Player>>(TFPlayer.Update);
			}
		}
	}
}