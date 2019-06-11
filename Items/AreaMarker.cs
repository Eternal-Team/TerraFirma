//using Terraria.ID;
//using Terraria.ModLoader;

//namespace WorldInteraction.Items
//{
//	public class AreaMarker : ModItem
//	{
//		public override string Texture => WIMod.ItemTexturePath + "AreaMarker";

//		public override void SetStaticDefaults()
//		{
//			DisplayName.SetDefault("Area Marker");
//			Tooltip.SetDefault("Use it to set the corners of an area");
//		}

//		public override void SetDefaults()
//		{
//			item.width = 40;
//			item.height = 40;
//			item.useTime = 20;
//			item.useAnimation = 20;
//			item.maxStack = 1;
//			item.useStyle = 1;
//			item.UseSound = SoundID.Item1;
//		}

//		public override bool CanRightClick() => true;

//		//public override void RightClick(Player player)
//		//{
//		//	int ID = item.stringColor;
//		//	if (WIMod.AMUIs.ContainsKey(ID) && WIMod.AMUIs[ID].visible)
//		//	{
//		//		WIMod.AMUIs[ID].visible = false;
//		//		WIMod.AMUIs[ID].CleanUp();
//		//		WIMod.AMUIs.Remove(ID);
//		//		WIMod.AMInterfaces.Remove(ID);
//		//	}
//		//	else
//		//	{
//		//		AreaMarkerUI ui = new AreaMarkerUI();
//		//		ui.Activate();
//		//		WIMod.AMInterfaces.Add(ID, new UserInterface());
//		//		WIMod.AMInterfaces[ID].SetState(ui);
//		//		WIMod.AMUIs.Add(ID, ui);
//		//		WIMod.AMUIs[ID].visible = true;
//		//		WIMod.AMUIs[ID].Load(ID);
//		//	}
//		//	item.stack++;
//		//}

//		//Point16 oldStart;
//		//Point16 oldEnd;
//		//public override void UpdateInventory(Player player)
//		//{
//		//	if (!markerData.start.Equals(oldStart))
//		//	{
//		//		oldStart = markerData.start;
//		//		SetTooltip();
//		//	}
//		//	if (!markerData.end.Equals(oldEnd))
//		//	{
//		//		oldStart = markerData.end;
//		//		SetTooltip();
//		//	}
//		//}

//		//private void SetTooltip()
//		//{
//		//	markerData.tiles = CalculateArea(markerData.start, markerData.end);
//		//	//= $"Area from ({markerData.start.X}, {markerData.start.Y}) to ({markerData.end.X}, {markerData.end.Y}){Environment.NewLine}Total of {markerData.tiles} tiles";
//		//}

//		//// draw the area when selecting second coord
//		//public override bool UseItem(Player player)
//		//{
//		//	try
//		//	{
//		//		int ID = BaseContainer.GetID(Utility.MouseToWorldPoint(), typeof(TEQuarry));
//		//		if (ID == -1)
//		//		{
//		//			if (markerData.starting)
//		//			{
//		//				markerData.start = Utility.MouseToWorldPoint();
//		//				markerData.starting = false;
//		//				Main.NewText($"First corner set at X: {markerData.start.X}, Y: {markerData.start.Y}");
//		//			}
//		//			else
//		//			{
//		//				markerData.end = Utility.MouseToWorldPoint();
//		//				markerData.starting = true;
//		//				Main.NewText($"Second corner set at X: {markerData.end.X}, Y: {markerData.end.Y}");

//		//				SetTooltip();
//		//				Main.NewText($"Total amount of tiles: {markerData.tiles}");
//		//			}
//		//		}
//		//		else
//		//		{
//		//			((TEQuarry)TileEntity.ByID[ID]).SetArea(markerData.start, markerData.end);
//		//			NetUtility.ClientSendTEUpdate(ID);
//		//			Main.NewText($"Quarry's area has been changed");
//		//		}

//		//		return true;
//		//	}
//		//	catch (Exception e)
//		//	{
//		//		ErrorLogger.Log(e.ToString());
//		//		return false;
//		//	}
//		//}

//		//public override void NetSend(BinaryWriter writer)
//		//{
//		//	writer.Write(markerData.starting);
//		//	writer.Write(markerData.tiles);
//		//	writer.Write(markerData.start.X);
//		//	writer.Write(markerData.start.Y);
//		//	writer.Write(markerData.end.X);
//		//	writer.Write(markerData.end.Y);

//		//	base.NetSend(writer);
//		//}

//		//public override void NetRecieve(BinaryReader reader)
//		//{
//		//	markerData.starting = reader.ReadBoolean();
//		//	markerData.tiles = reader.ReadInt32();
//		//	markerData.start = new Point16(reader.ReadInt16(), reader.ReadInt16());
//		//	markerData.end = new Point16(reader.ReadInt16(), reader.ReadInt16());

//		//	base.NetRecieve(reader);
//		//}

//		//public int CalculateArea(Point16 start, Point16 end)
//		//{
//		//	return Math.Abs((end.X - start.X) * (end.Y - start.Y));
//		//}

//		//public override TagCompound Save()
//		//{
//		//	return new TagCompound
//		//	{
//		//		["Start"] = markerData.start,
//		//		["End"] = markerData.end
//		//	};
//		//}

//		//public override void Load(TagCompound tag)
//		//{
//		//	markerData.start = tag.Get<Point16>("Start");
//		//	markerData.end = tag.Get<Point16>("End");
//		//	markerData.tiles = CalculateArea(markerData.start, markerData.end);
//		////	item.toolTip2 = $"Area from ({markerData.start.X}, {markerData.start.Y}) to ({markerData.end.X}, {markerData.end.Y}){Environment.NewLine}Total of {markerData.tiles} tiles";
//		//}

//		public override void AddRecipes()
//		{
//			ModRecipe recipe = new ModRecipe(mod);
//			recipe.AddIngredient(ItemID.IronBar, 10);
//			recipe.AddTile(TileID.Anvils);
//			recipe.anyIronBar = true;
//			recipe.SetResult(this);
//			recipe.AddRecipe();
//		}
//	}
//}

