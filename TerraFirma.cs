using BaseLibrary;
using LayerLibrary;
using Terraria.ModLoader;

namespace TerraFirma
{
	// builder, spawner, grinder, transport pipes - Tubular Network? (items, fluids, NPCs, Players), farms, item collector (vortex chest)

	public class TerraFirma : Mod
	{
		// entire transport pipes layer shifted by 8 on X-axis
		public static TerraFirma Instance;

		public TubularNetworkLayer layer;

		public override void Load()
		{
			Instance = this;

			Hooking.Initialize();

			layer = new TubularNetworkLayer();
		}

		public override void Unload()
		{
			Hooking.Uninitialize();

			Utility.UnloadNullableTypes();
		}
	}

	public class TubularNetworkLayer : ModLayer<Tube>
	{
		public override string Name { get; }
	}

	public class Tube : ModLayerElement
	{
		public override string Texture => "TerraFirma/Textures/TubeNetwork";
	}

	public class TubeItem : BaseLayerItem<Tube>
	{
		public override ModLayer<Tube> Layer => TerraFirma.Instance.layer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tube");
		}
	}
}