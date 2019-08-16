//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.DataStructures;
//using Terraria.ModLoader;
//using Terraria.ObjectData;
//using TheOneLibrary.Utility;
//using WorldInteraction.Tiles.TileEntities;
//using WorldInteraction.UI;

//namespace WorldInteraction.Tiles
//{
//	public class Builder : ModTile
//	{
//		public override bool Autoload(ref string name, ref string texture)
//		{
//			texture = WIMod.TileTexturePath + "Builder";
//			return base.Autoload(ref name, ref texture);
//		}

//		public override void SetDefaults()
//		{
//			Main.tileSolidTop[Type] = false;
//			Main.tileFrameImportant[Type] = true;
//			Main.tileNoAttach[Type] = true;
//			Main.tileLavaDeath[Type] = true;
//			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
//			TileObjectData.newTile.Origin = new Point16(0, 1);
//			TileObjectData.newTile.CoordinateHeights = new[] {16, 16};
//			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TEBuilder>().Hook_AfterPlacement, -1, 0, false);
//			TileObjectData.addTile(Type);
//			disableSmartCursor = true;

//			ModTranslation name = CreateMapEntryName();
//			name.SetDefault("Builder");
//			AddMapEntry(Color.Orange, name);
//		}

//		public override void RightClick(int i, int j)
//		{
//			int ID = mod.GetID<TEBuilder>(i, j);
//			if (ID == -1) return;

//			WIMod.Instance.HandleUI<BuilderUI>(ID);
//		}

//		public override void KillMultiTile(int i, int j, int frameX, int frameY)
//		{
//			int ID = mod.GetID<TEBuilder>(i, j);
//			if (ID != -1) WIMod.Instance.CloseUI(ID);

//			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType<Items.Builder>());
//			Utility.GetTileEntity<TEBuilder>().Kill(i, j);
//		}

//		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
//		{
//			Main.specX[nextSpecialDrawIndex] = i;
//			Main.specY[nextSpecialDrawIndex] = j;
//			nextSpecialDrawIndex++;
//		}

//		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
//		{
//			int ID = mod.GetID<TEBuilder>(i, j);
//			if (ID == -1) return;

//			Tile tile = Main.tile[i, j];
//			if (tile.TopLeft())
//			{
//				TEBuilder builder = (TEBuilder) TileEntity.ByID[ID];

//				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
//				if (Main.drawToScreen) zero = Vector2.Zero;

//				//string text = $"Mining at {te.currentX}, {te.currentY}";
//				//if (te.finished) text = "Quarry has finished!";

//				//Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X + 16 - Main.fontMouseText.MeasureString(text).X / 2 * 0.7f, j * 16 - (int)Main.screenPosition.Y - 4 - Main.fontMouseText.MeasureString(text).Y * 0.7f) + zero;

//				//Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, text, pos.X, pos.Y, !te.finished ? Color.Goldenrod * 0.7f : Color.DarkRed, Color.Black, Vector2.Zero, 0.7f);
//			}
//		}
//	}
//}

