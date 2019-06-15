using BaseLibrary;
using Microsoft.Xna.Framework;
using TerraFirma.TubularNetwork;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace TerraFirma
{
	// builder, spawner, grinder, farms, item collector (vortex chest)

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

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			Transform.Zoom = new Vector2(1f);
		}
	}
}