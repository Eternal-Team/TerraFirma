using BaseLibrary;
using TerraFirma.Network;
using Terraria.ModLoader;

namespace TerraFirma
{
	// todo: add these - builder, spawner, grinder, farms, item collector (vortex chest), pump, pipes (several channels)

	public class TerraFirma : Mod
	{
		public static TerraFirma Instance;

		public TubularNetworkLayer TubeNetworkLayer;

		public override void Load()
		{
			Instance = this;

			Hooking.Initialize();

			TubeNetworkLayer = new TubularNetworkLayer();
		}

		public override void Unload()
		{
			Hooking.Uninitialize();

			Utility.UnloadNullableTypes();
		}
	}
}