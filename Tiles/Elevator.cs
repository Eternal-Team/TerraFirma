using BaseLibrary;
using BaseLibrary.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerraFirma.Tiles
{
	public class Elevator : BaseTile
	{
		public override string Texture => "TerraFirma/Textures/Tiles/Elevator";

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;

			TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 5, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TileEntities.Elevator>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Elevator");
			AddMapEntry(Color.Blue, name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.Elevator elevator = Utility.GetTileEntity<TileEntities.Elevator>(i, j);

			Item.NewItem(i * 16, j * 16, 48, 16, mod.ItemType<Items.Elevator>());
			elevator.Kill(i, j);
		}
	}
}