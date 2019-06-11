using BaseLibrary;
using Terraria.ModLoader;

namespace TerraFirma
{
	// builder, spawner, grinder, transport pipes (items, fluids, NPCs, Players), farms, item collector (vortex chest)

	public class TerraFirma : Mod
	{
		public override void Load()
		{
			Hooking.Initialize();
		}

		public override void Unload()
		{
			Hooking.Uninitialize();

			Utility.UnloadNullableTypes();
		}
	}
}