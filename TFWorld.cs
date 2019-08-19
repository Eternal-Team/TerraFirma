using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerraFirma
{
	public class TFWorld : ModWorld
	{
		public override void PostDrawTiles()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			foreach (TileEntity tileEntity in TileEntity.ByID.Values)
			{
				if (tileEntity is Elevator elevator)
				{
					Vector2 position = elevator.position - Main.screenPosition;
					position.Y -= 74f;
					Main.spriteBatch.Draw(ModContent.GetTexture("TerraFirma/Textures/Tiles/ElevatorCage"),
						position, null, Color.DimGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
			}

			Main.spriteBatch.End();
		}

		public override void NetSend(BinaryWriter writer)
		{
			TerraFirma.Instance.TubeNetworkLayer.NetSend(writer);
		}

		public override void NetReceive(BinaryReader reader)
		{
			TerraFirma.Instance.TubeNetworkLayer.NetReceive(reader);
		}

		public override TagCompound Save() => new TagCompound
		{
			["TubularNetwork"] = TerraFirma.Instance.TubeNetworkLayer.Save()
		};

		public override void Load(TagCompound tag)
		{
			TerraFirma.Instance.TubeNetworkLayer.Load(tag.GetList<TagCompound>("TubularNetwork").ToList());
		}

		public override void PostUpdate()
		{
			TerraFirma.Instance.TubeNetworkLayer.Update();
		}
	}
}