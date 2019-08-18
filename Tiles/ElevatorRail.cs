using BaseLibrary.Tiles;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerraFirma.Tiles
{
	public class ElevatorRail : BaseTile
	{
		public override string Texture => "TerraFirma/Textures/Tiles/ElevatorRail";

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileMergeDirt[Type] = false;
			Main.tileLighted[Type] = true;

			AddMapEntry(Color.Orange);
			disableSmartCursor = false;
			drop = mod.ItemType<Items.ElevatorRail>();
		}

		public override void PlaceInWorld(int i, int j, Item item)
		{
			int index = 1;
			while (!Main.tile[i, j + index].active())
			{
				WorldGen.PlaceTile(i, j + index, Type);

				index++;
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (j % 7 == 0)
			{
				r = g = b = 1.4f;
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			short frameX = 0;
			short frameY = 0;
			if (WorldGen.InWorld(i - 1, j) && Main.tile[i - 1, j].active() && Main.tile[i - 1, j].type == Type) frameX += 18;
			if (WorldGen.InWorld(i + 1, j) && Main.tile[i + 1, j].active() && Main.tile[i + 1, j].type == Type) frameX += 36;
			if (WorldGen.InWorld(i, j - 1) && Main.tile[i, j - 1].active() && Main.tile[i, j - 1].type == Type) frameY += 18;
			if (WorldGen.InWorld(i, j + 1) && Main.tile[i, j + 1].active() && Main.tile[i, j + 1].type == Type) frameY += 36;
			Main.tile[i, j].frameX = frameX;
			Main.tile[i, j].frameY = frameY;
			return false;
		}
	}
}