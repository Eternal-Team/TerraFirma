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
		public bool UsingElevator;

		public float scale = 1f;
		public const float transferScale = 0.4f;
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

				// todo: lerp player position to this
				player.position = transportingPlayer.CurrentPosition.ToWorldCoordinates(24 - Player.defaultWidth * 0.5f, 64 - Player.defaultHeight);

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
					// todo: lerp player position to this
					player.position = transportingPlayer.CurrentPosition.ToWorldCoordinates(24, 24);
				}
			}
		}

		public override void PreUpdateMovement()
		{
			UsingElevator = false;

			foreach (TileEntity tileEntity in TileEntity.ByID.Values)
			{
				if (tileEntity is Elevator elevator)
				{
					if (
						player.position.X + player.width >= elevator.position.X &&
						player.position.X <= elevator.position.X + 80f &&
						player.position.Y + player.height <= elevator.oldPosition.Y &&
						player.position.Y + player.height + player.velocity.Y >= elevator.position.Y)
					{
						player.velocity.Y = elevator.position.Y - elevator.oldPosition.Y;

						UsingElevator = true;
					}
				}
			}

			if (Transporting)
			{
				Vector2 oldPosition = player.position;

				// note: maybe not interpolate from prev to next, but current to next?
				player.position = Vector2.Lerp(transportingPlayer.PreviousPosition.ToWorldCoordinates(24, 24), transportingPlayer.CurrentPosition.ToWorldCoordinates(24, 24), transportingPlayer.timer / (float)TransportingPlayer.speed);

				player.fullRotation = (player.position - oldPosition).ToRotation() + MathHelper.PiOver2;
			}
		}

		public static void Update(Player player)
		{
			if (player == null || !player.active) return;

			TFPlayer tfPlayer = player.GetModPlayer<TFPlayer>();

			if (tfPlayer.UsingElevator)
			{
				player.maxFallSpeed = 5000f;
				player.gravity = 50f;
			}
		}

		public override void SetControls()
		{
			if (UsingElevator)
			{
				foreach (TileEntity tileEntity in TileEntity.ByID.Values)
				{
					if (tileEntity is Elevator elevator)
					{
						if (player.controlDown) elevator.direction = 1;
						else if (player.controlUp) elevator.direction = -1;
					}
				}

				player.controlJump = false;
			}

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
			if (UsingElevator) layers.RemoveAt(layers.FindIndex(l => l.Name == "Wings"));
		}
	}
}