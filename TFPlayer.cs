using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraFirma.Mounts;
using TerraFirma.Network;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerraFirma
{
	public class TFPlayer : ModPlayer
	{
		public bool UsingTubeSystem => Entering || Exiting || Transporting;

		public bool Entering;
		public bool Exiting;
		public bool Transporting;

		public float scale = 1f;
		public const float transferScale = 0.4f;
		public const float scaleSpeed = 0.01f;
		public float alpha = 1f;

		public TransportingPlayer transportingPlayer;

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

				player.fullRotation = 0f;
				player.gfxOffY = 0f;

				// todo: use IL editing to prevent dust spawning
				player.mount?.SetMount(mod.MountType<TubeMount>(), player);
			}

			if (Exiting)
			{
				Transporting = false;

				scale = 1f;

				if (alpha < 1f) alpha += 0.025f;
				else
				{
					player.mount?.Dismount(player);

					Exiting = false;
					transportingPlayer = null;
				}
			}

			if (Entering)
			{
				if (alpha > 0f) alpha -= 0.025f;
				else
				{
					Entering = false;
					Transporting = true;

					scale = transferScale;

					position = transportingPlayer.CurrentPosition.ToWorldCoordinates(24, 24);
				}
			}
		}

		public static Vector2 position;

		public override void FrameEffects()
		{
			//if (usingElevator) player.bodyFrame.Y = 0;
		}

		public bool usingElevator;

		public override void PreUpdateMovement()
		{
			usingElevator = false;

			foreach (TileEntity tileEntity in TileEntity.ByID.Values)
			{
				if (tileEntity is Elevator elevator)
				{
					if (
						player.position.X + player.width >= elevator.position.X &&
						player.position.X <= elevator.position.X + 48f &&
						player.position.Y + player.height <= elevator.oldPosition.Y &&
						player.position.Y + player.height + player.velocity.Y >= elevator.position.Y)
					{
						player.velocity.Y = elevator.position.Y - elevator.oldPosition.Y;

						usingElevator = true;
					}
				}
			}

			if (Transporting)
			{
				Vector2 prevPos = transportingPlayer.PreviousPosition.ToWorldCoordinates(24, 24);
				Vector2 nextPos = transportingPlayer.CurrentPosition.ToWorldCoordinates(24, 24);

				Vector2 prevPoss = position;
				position = Vector2.Lerp(prevPos, nextPos, transportingPlayer.timer / (float)TransportingPlayer.speed);
				player.position = position;

				player.fullRotation = (position - prevPoss).ToRotation() + MathHelper.PiOver2;
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

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			return !UsingTubeSystem && base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if (usingElevator) layers.RemoveAt(layers.FindIndex(l => l.Name == "Wings"));
		}
	}
}