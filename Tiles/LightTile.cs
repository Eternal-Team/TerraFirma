//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.ID;
//using TheOneLibrary.Base;

//namespace WorldInteraction.Tiles
//{
//	public class LightTile : BaseTile
//	{
//		public override bool Autoload(ref string name, ref string texture)
//		{
//			texture = WIMod.TileTexturePath + "LightTile";
//			return base.Autoload(ref name, ref texture);
//		}

//		public override void SetDefaults()
//		{
//			Main.tileSolid[Type] = false;
//			Main.tileMergeDirt[Type] = false;
//			Main.tileBlockLight[Type] = true;
//			Main.tileLighted[Type] = true;
//			AddMapEntry(Color.Orange);
//			disableSmartCursor = false;
//		}

//		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
//		{
//			r = 0.9f;
//			g = 0.9f;
//			b = 0.9f;
//		}

//		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
//		{
//			if (Main.rand.Next(15) == 0)
//			{
//				for (int x = 0; x < 4; x++)
//				{
//					int index = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.Iron, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
//					Main.dust[index].noGravity = true;
//					Main.dust[index].color = Color.White;
//				}
//			}
//		}
//	}
//}

