using BaseLibrary;
using LayerLibrary;
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
			Network = new TubularNetwork(this);
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
	}
}