using BaseLibrary;
using BaseLibrary.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerraFirma.Tiles
{
	public class Quarry : BaseTile
	{
		public override string Texture => "TerraFirma/Textures/Tiles/Quarry";

		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.EmptyTile | AnchorType.SolidTile, 3, 0);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TileEntities.Quarry>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Quarry");
			AddMapEntry(Color.Black, name);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TileEntities.Quarry quarry = Utility.GetTileEntity<TileEntities.Quarry>(i, j);
			if (quarry == null || !Main.tile[i, j].IsTopLeft()) return false;
			// todo: put in TerraFirma.cs
			Texture2D texture = ModContent.GetTexture(Texture);
			Vector2 position = quarry.Position.ToScreenCoordinates();

			spriteBatch.Draw(texture, position + new Vector2(24), null, Color.White, quarry.Angle - MathHelper.PiOver2, new Vector2(24), Vector2.One, SpriteEffects.None, 0f);

			if (quarry.Targetted)
			{
				Main.instance.LoadProjectile(632);
				DelegateMethods.f_1 = 1f;
				DelegateMethods.c_1 = Color.Red * 0.9f;

				Vector2 beamStart = position + new Vector2(24) + 30 * quarry.Angle.ToRotationVector2();
				Utils.DrawLaser(spriteBatch, Main.projectileTexture[632], beamStart, beamStart + quarry.Angle.ToRotationVector2() * quarry.Lenght, Vector2.One * 0.5f, DelegateMethods.RainbowLaserDraw);
			}

			return false;
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			Main.specX[nextSpecialDrawIndex] = i;
			Main.specY[nextSpecialDrawIndex] = j;
			nextSpecialDrawIndex++;
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TileEntities.Quarry quarry = Utility.GetTileEntity<TileEntities.Quarry>(i, j);
			if (quarry == null || !Main.tile[i, j].IsTopLeft()) return;

			Vector2 position = quarry.CurrentTile.ToScreenCoordinates();
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, 16, 16), Color.Red * 0.5f);
		}

		public override void RightClick(int i, int j)
		{
			TileEntities.Quarry quarry = Utility.GetTileEntity<TileEntities.Quarry>(i, j);

			BaseLibrary.BaseLibrary.PanelGUI.UI.HandleUI(quarry);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			TileEntities.Quarry quarry = Utility.GetTileEntity<TileEntities.Quarry>(i, j);

			BaseLibrary.BaseLibrary.PanelGUI.UI.CloseUI(quarry);

			Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType<Items.Quarry>());
			quarry.Kill(i, j);
		}
	}
}