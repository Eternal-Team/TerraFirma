using BaseLibrary;
using BaseLibrary.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerraFirma.Tiles
{
	public class TestTile : BaseTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = false;
			Main.tileLavaDeath[Type] = false;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TETestTile>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("test");
			AddMapEntry(Color.Blue, name);
		}

		public override void RightClick(int i, int j)
		{
			TETestTile testTile = mod.GetTileEntity<TETestTile>(i, j);
			if (testTile == null) return;
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			Main.specX[nextSpecialDrawIndex] = i;
			Main.specY[nextSpecialDrawIndex] = j;
			nextSpecialDrawIndex++;
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TETestTile testTile = mod.GetTileEntity<TETestTile>(i, j);
			if (testTile == null || !Main.tile[i, j].IsTopLeft()) return;

			Vector2 position = testTile.Hitbox.TopLeft() - Main.screenPosition;

		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TETestTile testTile = mod.GetTileEntity<TETestTile>(i, j);

			Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType<Items.TestTile>());
			testTile.Kill(i, j);
		}
	}
}