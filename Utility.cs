//namespace WorldInteraction
//{
//	public static class Utility
//	{
//		//public static void WriteTileData(BinaryWriter writer, Tile tile)
//		//{
//		//	writer.Write(tile.type);
//		//	writer.Write(tile.wall);
//		//	writer.Write(tile.frameX);
//		//	writer.Write(tile.frameY);
//		//	writer.Write(tile.active());
//		//	writer.Write(tile.slope());
//		//}

//		//public static Tile ReadTileData(BinaryReader reader)
//		//{
//		//	Tile tile = new Tile();
//		//	tile.type = reader.ReadUInt16();
//		//	tile.wall = reader.ReadUInt16();
//		//	tile.frameX = reader.ReadInt16();
//		//	tile.frameY = reader.ReadInt16();
//		//	tile.active(reader.ReadBoolean());
//		//	tile.slope(reader.ReadByte());
//		//	return tile;
//		//}

//		//public static void SaveTileData(TagCompound tag, Tile tile, int i, int j)
//		//{
//		//	tag.Set($"Type ({i},{j})", tile.type);
//		//	tag.Set($"Wall ({i},{j})", tile.wall);
//		//	tag.Set($"FrameX ({i},{j})", tile.frameX);
//		//	tag.Set($"FrameY ({i},{j})", tile.frameY);
//		//	tag.Set($"Active ({i},{j})", tile.active());
//		//	tag.Set($"Slope ({i},{j})", tile.slope());
//		//}

//		//public static Tile LoadTileData(TagCompound tag, int i, int j)
//		//{
//		//	Tile tile = new Tile();
//		//	tile.type = tag.Get<ushort>($"Type ({i},{j})");
//		//	tile.wall = tag.Get<ushort>($"Wall ({i},{j})");
//		//	tile.frameX = tag.GetShort($"FrameX ({i},{j})");
//		//	tile.frameY = tag.GetShort($"FrameY ({i},{j})");
//		//	tile.active(tag.GetBool($"Active ({i},{j})"));
//		//	tile.slope(tag.GetByte($"Slope ({i},{j})"));
//		//	return tile;
//		//}

//		public static bool NextTile(ref int i, ref int j, int maxX, int maxY, int minX = 0, int minY = 0)
//		{
//			j++;
//			if (j > maxY)
//			{
//				j = minY;
//				i++;
//				if (i >= maxX) return false;
//			}
//			return true;
//		}
//	}
//}

