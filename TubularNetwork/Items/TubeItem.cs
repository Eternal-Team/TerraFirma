using LayerLibrary;

namespace TerraFirma.TubularNetwork.Items
{
	public class TubeItem : BaseLayerItem<Tube>
	{
		public override ModLayer<Tube> Layer => TerraFirma.Instance.TubeNetworkLayer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tube");
		}
	}
}