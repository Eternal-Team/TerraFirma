using LayerLibrary;

namespace TerraFirma.Network.Items
{
	public class TubeItem : BaseLayerItem
	{
		public override IModLayer Layer => TerraFirma.Instance.TubeNetworkLayer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tube");
		}
	}
}