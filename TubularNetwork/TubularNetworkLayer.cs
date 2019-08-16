using LayerLibrary;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace TerraFirma.Network
{
	public class TubularNetworkLayer : ModLayer<Tube>
	{
		public override int TileSize => 3;

		public override void Load(List<TagCompound> list)
		{
			base.Load(list);

			foreach (Tube tube in data.Values) tube.Merge();
		}

		public override void Update()
		{
			base.Update();

			foreach (TubularNetwork network in TubularNetwork.Networks) network.Update();
		}
	}
}