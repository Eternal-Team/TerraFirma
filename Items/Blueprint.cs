////using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using TheOneLibrary.Base.Items;

//namespace WorldInteraction.Items
//{
//	public class Blueprint : BaseItem
//	{
//		public override string Texture => WIMod.ItemTexturePath + "Blueprint";

//		public override void SetStaticDefaults()
//		{
//			DisplayName.SetDefault("Blueprint");
//			Tooltip.SetDefault("Creates a virtual copy of the tiles you select");
//		}

//		public override void SetDefaults()
//		{
//			item.width = 24;
//			item.height = 24;
//			item.useTime = 20;
//			item.useAnimation = 20;
//			item.maxStack = 1;
//			item.useStyle = 1;
//			item.UseSound = SoundID.Item1;
//		}

//		public override bool CanRightClick() => true;

//		// preview option?
//		public override void RightClick(Player player)
//		{
//			//blueprintData.tiles = new Tile[0, 0];
//			item.stack++;
//		}

//		//public override bool UseItem(Player player)
//		//{
//		//	try
//		//	{
//		//		if (blueprintData.starting)
//		//		{
//		//			blueprintData.start = Utility.MouseToWorldPoint();
//		//			Main.NewText($"Start: {blueprintData.start.X}, {blueprintData.start.Y}");
//		//			blueprintData.starting = false;
//		//		}
//		//		else
//		//		{
//		//			blueprintData.end = Utility.MouseToWorldPoint();
//		//			blueprintData.starting = true;
//		//			Main.NewText($"End: {blueprintData.end.X}, {blueprintData.end.Y}");
//		//			if (Math.Abs(blueprintData.start.X - blueprintData.end.X) + 1 <= 20 && Math.Abs(blueprintData.start.Y - blueprintData.end.Y) + 1 <= 20) StoreTileData();
//		//			else Main.NewText("Maximum blueprint size is 20x20");
//		//		}
//		//	}
//		//	catch (Exception e)
//		//	{
//		//		ErrorLogger.Log(e.ToString());
//		//		return false;
//		//	}

//		//	return true;
//		//}

//		//private void StoreTileData()
//		//{
//		//	int x = Math.Abs(blueprintData.start.X - blueprintData.end.X);
//		//	int y = Math.Abs(blueprintData.start.Y - blueprintData.end.Y);

//		//	Point16 start = Utility.TopLeftPoint(blueprintData.start, blueprintData.end
//		//		, x, y);

//		//	blueprintData.tiles = new Tile[x + 1, y + 1];

//		//	for (int i = 0; i <= x; i++)
//		//	{
//		//		for (int j = 0; j <= y; j++)
//		//		{
//		//			Tile tile = Main.tile[start.X + i, start.Y + j];

//		//			blueprintData.tiles[i, j] = new Tile();
//		//			blueprintData.tiles[i, j].type = tile.type;
//		//			blueprintData.tiles[i, j].wall = tile.wall;
//		//			blueprintData.tiles[i, j].frameX = tile.frameX;
//		//			blueprintData.tiles[i, j].frameY = tile.frameY;
//		//			blueprintData.tiles[i, j].active(tile.active());
//		//			blueprintData.tiles[i, j].slope(tile.slope());
//		//		}
//		//	}
//		//}

//		//public override void NetSend(BinaryWriter writer)
//		//{
//		//	writer.Write(blueprintData.starting);
//		//	writer.Write(blueprintData.tiles.GetLength(0));
//		//	writer.Write(blueprintData.tiles.GetLength(1));

//		//	for (int i = 0; i < blueprintData.tiles.GetLength(0); i++)
//		//	{
//		//		for (int j = 0; j < blueprintData.tiles.GetLength(1); j++)
//		//		{
//		//			Utility.WriteTileData(writer, blueprintData.tiles[i, j]);
//		//		}
//		//	}

//		//	base.NetSend(writer);
//		//}

//		//public override void NetRecieve(BinaryReader reader)
//		//{
//		//	blueprintData.starting = reader.ReadBoolean();
//		//	int dim0 = reader.ReadInt32();
//		//	int dim1 = reader.ReadInt32();

//		//	blueprintData.tiles = new Tile[dim0, dim1];
//		//	for (int i = 0; i < dim0; i++)
//		//	{
//		//		for (int j = 0; i < dim1; j++)
//		//		{
//		//			blueprintData.tiles[i, j] = Utility.ReadTileData(reader);
//		//		}
//		//	}

//		//	base.NetRecieve(reader);
//		//}

//		//public override TagCompound Save()
//		//{
//		//	TagCompound tag = new TagCompound();
//		//	tag.Set("DIM0", blueprintData.tiles.GetLength(0));
//		//	tag.Set("DIM1", blueprintData.tiles.GetLength(1));

//		//	for (int i = 0; i < blueprintData.tiles.GetLength(0); i++)
//		//	{
//		//		for (int j = 0; j < blueprintData.tiles.GetLength(1); j++)
//		//		{
//		//			Utility.SaveTileData(tag, blueprintData.tiles[i, j], i, j);
//		//		}
//		//	}

//		//	return tag;
//		//}

//		//public override void Load(TagCompound tag)
//		//{
//		//	int dim0 = tag.GetInt("DIM0");
//		//	int dim1 = tag.GetInt("DIM1");

//		//	blueprintData.tiles = new Tile[dim0, dim1];
//		//	for (int i = 0; i < dim0; i++)
//		//	{
//		//		for (int j = 0; j < dim1; j++)
//		//		{
//		//			blueprintData.tiles[i, j] = Utility.LoadTileData(tag, i, j);
//		//		}
//		//	}
//		//}

//		public override void AddRecipes()
//		{
//			ModRecipe recipe = new ModRecipe(mod);
//			recipe.AddIngredient(ItemID.Silk, 17);
//			recipe.AddTile(TileID.WorkBenches);
//			recipe.SetResult(this);
//			recipe.AddRecipe();
//		}
//	}
//}

