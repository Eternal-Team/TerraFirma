using BaseLibrary;
using LayerLibrary;
using Microsoft.Xna.Framework;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace TerraFirma
{
	// builder, spawner, grinder, transport pipes - Tubular Network? (items, fluids, NPCs, Players), farms, item collector (vortex chest)

	public class TerraFirma : Mod
	{
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

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			Transform.Zoom = new Vector2(1f);
		}
	}

	public class TubularNetworkLayer : ModLayer<Tube>
	{
		public override string Name => "Tubular Network";

		public override int TileSize => 3;
	}

	public class Tube : ModLayerElement
	{
		public override string Texture => "TerraFirma/Textures/TubeNetwork2";

		public override void OnRemove()
		{
			TEEntryPoint entryPoint = TerraFirma.Instance.GetTileEntity<TEEntryPoint>(Position);
			if (entryPoint == null) return;

			WorldGen.KillTile(Position.X, Position.Y);
		}
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