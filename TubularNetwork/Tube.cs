using BaseLibrary;
using LayerLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TerraFirma.TileEntities;
using Terraria;

namespace TerraFirma.TubularNetwork
{
	public class Tube : ModLayerElement<Tube>
	{
		public override string Texture => "TerraFirma/Textures/TubeNetwork2";

		public TubularNetwork Network;

		public Tube()
		{
			Network = new TubularNetwork
			{
				tiles = new List<Tube> { this }
			};
		}

		public override void OnPlace()
		{
			Merge();
		}

		public override void OnRemove()
		{
			Network.RemoveTile(this);

			TEEntryPoint entryPoint = TerraFirma.Instance.GetTileEntity<TEEntryPoint>(Position);
			if (entryPoint == null) return;

			WorldGen.KillTile(Position.X, Position.Y);
		}

		public void Merge()
		{
			foreach (Tube tube in GetNeighbors()) tube.Network.Merge(Network);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);

			foreach (TransportingPlayer player in Network.TransportingPlayers)
			{
				if (player.CurrentPosition == Position)
				{
					spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(Position.X * 16 - Main.screenPosition.X + 20), (int)(Position.Y * 16 - Main.screenPosition.Y + 20), 8, 8), Color.Red);
				}
			}
		}
	}
}