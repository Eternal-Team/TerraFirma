using BaseLibrary;
using BaseLibrary.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraFirma.Network;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerraFirma.Tiles
{
	public class EntryPoint : BaseTile
	{
		public override string Texture => "TerraFirma/Textures/Tiles/EntryPoint";

		// todo: possibly in TerraFirma.cs
		private static Texture2D _connectionTexture;
		public static Texture2D ConnectionTexture => _connectionTexture ?? (_connectionTexture = ModContent.GetTexture("TerraFirma/Textures/Tiles/EntryPoint_Connection"));

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.Origin = new Point16(0, 3);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TileEntities.EntryPoint>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Entry Point");
			AddMapEntry(Color.Blue, name);
		}

		public override void RightClick(int i, int j)
		{
			TileEntities.EntryPoint entryPoint = Utility.GetTileEntity<TileEntities.EntryPoint>(i, j);
			if (entryPoint == null) return;

			BaseLibrary.BaseLibrary.PanelGUI.UI.HandleUI(entryPoint);
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			Main.specX[nextSpecialDrawIndex] = i;
			Main.specY[nextSpecialDrawIndex] = j;
			nextSpecialDrawIndex++;
		}

		public override bool CanPlace(int i, int j) => TerraFirma.Instance.TubeNetworkLayer.ContainsKey(i, j - 3);

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TileEntities.EntryPoint entryPoint = Utility.GetTileEntity<TileEntities.EntryPoint>(i, j);
			if (entryPoint == null || !Main.tile[i, j].IsTopLeft()) return;

			Vector2 position = new Point16(i, j).ToScreenCoordinates();

			Tube tube = TerraFirma.Instance.TubeNetworkLayer[i, j];

			if (tube.GetNeighbor(Side.Top) != null)
			{
				spriteBatch.Draw(ConnectionTexture, position + new Vector2(6, 0));
			}

			if (tube.GetNeighbor(Side.Left) != null)
			{
				spriteBatch.Draw(ConnectionTexture, position + new Vector2(-2, 24), null, Color.White, -MathHelper.PiOver2, ConnectionTexture.Size() * 0.5f, Vector2.One, SpriteEffects.None, 0f);
			}

			if (tube.GetNeighbor(Side.Right) != null)
			{
				spriteBatch.Draw(ConnectionTexture, position + new Vector2(50, 24), null, Color.White, MathHelper.PiOver2, ConnectionTexture.Size() * 0.5f, Vector2.One, SpriteEffects.None, 0f);
			}
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.EntryPoint entryPoint = Utility.GetTileEntity<TileEntities.EntryPoint>(i, j);
			if (Main.netMode != NetmodeID.Server) BaseLibrary.BaseLibrary.PanelGUI.UI.CloseUI(entryPoint);

			Item.NewItem(i * 16, j * 16, 48, 64, mod.ItemType<Items.EntryPoint>());
			entryPoint.Kill(i, j);
		}
	}
}