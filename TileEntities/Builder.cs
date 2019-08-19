//using System.Collections.Generic;
//using System.IO;
//using Terraria;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.ModLoader.IO;
//using TheOneLibrary.Base;
//using TheOneLibrary.Storage;
//using TheOneLibrary.Utility;

//namespace WorldInteraction.Tiles.TileEntities
//{
//	public class TEBuilder : BaseTE, IContainerTile
//	{
//		public bool active;

//		public Point16 start;
//		public Point16 current = new Point16(-1, -1);

//		public IList<Item> Items = new List<Item>();

//		public List<ushort> listTypes = new List<ushort>();
//		public List<Item> requiredItems = new List<Item>();

//		public override bool ValidTile(Tile tile) => tile.type == mod.TileType<Builder>() && tile.TopLeft();

//		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
//		{
//			if (Main.netMode != NetmodeID.MultiplayerClient) Place(i, j - 1);

//			NetMessage.SendTileSquare(Main.myPlayer, i, j - 1, 2);
//			NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j - 1, Type);
//			return -1;
//		}

//		public override void OnPlace()
//		{
//			for (int i = 0; i < 28; i++) Items.Add(new Item());
//		}

//		public override void OnNetPlace() => OnPlace();

//		//blueprint part of inventory
//		// Quarry+Builder deactivate when it tries to break itself
//		private int timer;

//		public override void Update()
//		{
//			if (++timer >= 5 && active)
//			{
//				//MoveToNextTile();
//				//Blueprint blueprint = (Blueprint)Items[27].modItem;

//				//int indexDIM0 = current.X - start.X;
//				//int indexDIM1 = current.Y - start.Y;

//				//Tile tile = Main.tile[current.X, current.Y];

//				//if (!tile.active() && bpData.tiles[indexDIM0, indexDIM1].active())
//				//{
//				//	Main.tile[current.X, current.Y].type = bpData.tiles[indexDIM0, indexDIM1].type;
//				//	Main.tile[current.X, current.Y].frameX = bpData.tiles[indexDIM0, indexDIM1].frameX;
//				//	Main.tile[current.X, current.Y].frameY = bpData.tiles[indexDIM0, indexDIM1].frameY;
//				//	Main.tile[current.X, current.Y].slope(bpData.tiles[indexDIM0, indexDIM1].slope());
//				//	Main.tile[current.X, current.Y].active(true);
//				//	NetMessage.SendTileSquare(-1, current.X, current.Y, 1);
//				//}

//				//if (tile.wall == WallID.None && bpData.tiles[indexDIM0, indexDIM1].wall != WallID.None)
//				//{
//				//	Main.tile[current.X, current.Y].wall = bpData.tiles[indexDIM0, indexDIM1].wall;
//				//	NetMessage.SendTileSquare(-1, current.X, current.Y, 1);
//				//}

//				//if (current.X == start.X + bpData.tiles.GetLength(0) - 1 && current.Y == start.Y)
//				//{
//				//	active = false;
//				//	current = new Point16(-1, -1);
//				//}

//				timer = 0;
//			}

//			this.HandleUIFar();
//		}

//		// check if it has all ingredients -> build

//		// wall requirements
//		//public void OnActive()
//		//{
//		//	try
//		//	{
//		//		active = true;

//		//		BlueprintData bpData = ((Blueprint)blueprint.modItem).blueprintData;
//		//		List<Tile> lst = UsingForLoop(bpData.tiles);

//		//		foreach (Tile tile in lst)
//		//		{
//		//			if (tile.type >= 0 && tile.active())
//		//			{
//		//				int style = 0;
//		//				int alt = 0;
//		//				TileObjectData.GetTileInfo(tile, ref style, ref alt);
//		//				TileObjectData data = TileObjectData.GetTileData(tile.type, style, 0);

//		//				if (data != null && (data.Width > 1 || data.Height > 1))
//		//				{
//		//					Item item = new Item();
//		//					item.SetDefaults(WIMod.NewTileMap[new TileData(tile.type, style)]);

//		//					if (requiredItems.Count(x => x.type == item.type) == 0)
//		//					{
//		//						Func<Tile, bool> pred = (x) =>
//		//						{
//		//							if (x.type == tile.type)
//		//							{
//		//								int passStyle = 0;
//		//								int passAlt = 0;
//		//								TileObjectData.GetTileInfo(x, ref passStyle, ref passAlt);

//		//								int scanStyle = 0;
//		//								int scanAlt = 0;
//		//								TileObjectData.GetTileInfo(tile, ref scanStyle, ref scanAlt);

//		//								if (passStyle == scanStyle) return true;
//		//							}

//		//							return false;
//		//						};

//		//						item.stack = lst.Count(pred) / (data.Width * data.Height);
//		//						requiredItems.Add(item);
//		//					}
//		//				}
//		//				else if (data == null || data != null && data.Width == 1 && data.Height == 1)
//		//				{
//		//					Item item = new Item();
//		//					item.SetDefaults(WIMod.NewTileMap[new TileData(tile.type, style)]);
//		//					item.stack = lst.Count(x => x.type == tile.type);
//		//					if (requiredItems.Where(x => x.type == item.type).Count() == 0) requiredItems.Add(item);
//		//				}
//		//			}
//		//		}
//		//	}
//		//	catch (Exception e)
//		//	{
//		//		ErrorLogger.Log(e.ToString());
//		//	}
//		//}

//		//static List<Tile> UsingForLoop(Tile[,] array)
//		//{
//		//	int width = array.GetLength(0);
//		//	int height = array.GetLength(1);
//		//	List<Tile> ret = new List<Tile>();
//		//	for (int i = 0; i < width; i++)
//		//	{
//		//		for (int j = 0; j < height; j++)
//		//		{
//		//			ret.Add(array[i, j]);
//		//		}
//		//	}
//		//	return ret;
//		//}

//		//public void MoveToNextTile()
//		//{
//		//	BlueprintData data = ((Blueprint)blueprint.modItem).blueprintData;
//		//	if (current.X == -1 && current.Y == -1)
//		//	{
//		//		current = new Point16(start.X, start.Y + data.tiles.GetLength(1));
//		//	}
//		//	else
//		//	{
//		//		if (current.Y > start.Y) current = new Point16(current.X, current.Y - 1);
//		//		else
//		//		{
//		//			if (current.X < start.X + data.tiles.GetLength(0))
//		//			{
//		//				current = new Point16(current.X + 1, start.Y + data.tiles.GetLength(1));
//		//			}
//		//			else
//		//			{
//		//				current = new Point16(-1, -1);
//		//				return;
//		//			}
//		//		}
//		//	}
//		//}

//		public override TagCompound Save() => new TagCompound
//		{
//			["Items"] = Items.Save(),
//			["Active"] = active,
//			["Start"] = start,
//			["Current"] = current
//		};

//		public override void Load(TagCompound tag)
//		{
//			Items = TheOneLibrary.Utility.Utility.Load(tag);
//			active = tag.GetBool("Active");
//			start = tag.Get<Point16>("Start");
//			current = tag.Get<Point16>("Current");
//		}

//		public override void NetSend(BinaryWriter writer, bool lightSend) => TagIO.Write(Save(), writer);

//		public override void NetReceive(BinaryReader reader, bool lightReceive) => Load(TagIO.Read(reader));

//		public IList<Item> GetItems() => Items;

//		public Item GetItem(int slot) => Items[slot];

//		public void SetItem(Item item, int slot) => Items[slot] = item;

//		public ModTileEntity GetTileEntity() => this;
//	}
//}

