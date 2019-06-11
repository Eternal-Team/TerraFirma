//using Terraria.ModLoader;
//using TheOneLibrary.Base.UI;
//using TheOneLibrary.Utility;
//using WorldInteraction.Tiles.TileEntities;

//namespace WorldInteraction.UI
//{
//	public class BuilderUI : BaseUI, ITileEntityUI
//	{
//		public TEBuilder builder;

//		//public UIItemSlot blueprint;
//		//public UIItemSlot[] slots;

//		//public UIToggleButton listReq;

//		// start x/y
//		// active
//		// recursion (active + direction)

//		public override void OnInitialize()
//		{
//			panelMain.Width.Set(9 * 40f + 10 * 8f, 0f);
//			panelMain.Height.Set(4 * 40f + 5 * 8f, 0f);
//			panelMain.Center();
//			panelMain.SetPadding(0);
//			panelMain.OnMouseDown += DragStart;
//			panelMain.OnMouseUp += DragEnd;
//			panelMain.BackgroundColor = panelColor;
//			Append(panelMain);

//			//blueprint = new UIItemSlot(new Item());
//			//blueprint.Width.Set(40f, 0f);
//			//blueprint.Height.Set(40f, 0f);
//			//blueprint.Left.Set(8f, 0f);
//			//blueprint.Top.Set(8f, 0f);
//			//blueprint.OnItemChange += () =>
//			//{
//			//	((TEBuilder)TileEntity.ByID[ID]).blueprint = blueprint.item;
//			//	if (!blueprint.item.IsAir)
//			//	{
//			//		((TEBuilder)TileEntity.ByID[ID]).OnActive();
//			//	}
//			//	NetUtility.ClientSendTEUpdate(ID);
//			//};
//			////blueprint.ItemCondition += () =>
//			////{
//			////	return Main.mouseItem.type == WIMod.Instance.ItemType<Blueprint>() || Main.mouseItem.IsAir;
//			////};
//			//panelMain.Append(blueprint);

//			//listReq = new UIToggleButton(TextureManager.Load("Item_" + ItemID.Torch));
//			//listReq.Width.Set(16f, 0f);
//			//listReq.Height.Set(16f, 0f);
//			//listReq.Left.Set(-24f, 1f);
//			//listReq.Top.Set(8f, 0f);
//			//listReq.OnClick += (UIMouseEvent e, UIElement s) =>
//			//{
//			//	foreach (Item item in ((TEBuilder)TileEntity.ByID[ID]).requiredItems)
//			//	{
//			//		Main.NewText($"Item: {item.Name} (x{item.stack})");
//			//	}
//			//};
//			//panelMain.Append(listReq);
//		}

//		public override void Load()
//		{
//			//blueprint.item = builder.blueprint;

//			//slots = new UIItemSlot[builder.inventory.Count];
//			//for (int i = 0; i < slots.Length; i++)
//			//{
//			//	slots[i] = new UIItemSlot(builder.inventory[i]);
//			//	slots[i].Width.Set(40f, 0f);
//			//	slots[i].Height.Set(40f, 0f);
//			//	slots[i].Left.Set(8f + 48f * (i % 9), 0f);
//			//	slots[i].Top.Set(56f + 48f * (i / 9), 0f);
//			//	panelMain.Append(slots[i]);
//			//}
//		}

//		public void SetTileEntity(ModTileEntity tileEntity) => builder = (TEBuilder) tileEntity;
//	}
//}

