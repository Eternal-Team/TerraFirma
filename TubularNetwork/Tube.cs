using BaseLibrary;
using LayerLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraFirma.Network.Items;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.ModLoader;

namespace TerraFirma.Network
{
	public class Tube : ModLayerElement<Tube>
	{
		public override string Texture => "TerraFirma/Textures/TubeNetwork_Back";

		public override int DropItem => TerraFirma.Instance.ItemType<TubeItem>();

		public TubularNetwork Network;

		public Tube()
		{
			Network = new TubularNetwork(this);
		}

		public override void OnPlace()
		{
			Merge();
		}

		public override void OnRemove()
		{
			Network.RemoveTile(this);

			EntryPoint entryPoint = Utility.GetTileEntity<EntryPoint>(Position);
			if (entryPoint == null) return;

			WorldGen.KillTile(Position.X, Position.Y);
		}

		public void Merge()
		{
			foreach (Tube tube in GetNeighbors()) tube.Network.Merge(Network);
		}

		public void PostDraw(SpriteBatch spriteBatch)
		{
			Vector2 position = Position.ToScreenCoordinates(false);

			for (int x = 0; x < Layer.TileSize; x++)
			{
				for (int y = 0; y < Layer.TileSize; y++)
				{
					Color color = Lighting.GetColor(Position.X + x, Position.Y + y);
					color *= 0.5f;
					spriteBatch.Draw(ModContent.GetTexture("TerraFirma/Textures/TubeNetwork_Front"), position + new Vector2(x, y) * 16, new Rectangle(Frame.X + 16 * x, Frame.Y + 16 * y, 16, 16), color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
				}
			}
		}
	}
}