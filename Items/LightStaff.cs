//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using TheOneLibrary.Base.Items;
//using WorldInteraction.Tiles;

//namespace WorldInteraction.Items
//{
//	public class LightStaff : BaseItem
//	{
//		public override string Texture => WIMod.ItemTexturePath + "LightStaff";

//		public override void SetStaticDefaults()
//		{
//			DisplayName.SetDefault("Light Staff");
//			Tooltip.SetDefault("Places a light where you click, for a bit of mana");
//		}

//		public override void SetDefaults()
//		{
//			item.width = 16;
//			item.height = 16;
//			item.maxStack = 1;
//			item.useTurn = true;
//			item.useAnimation = 15;
//			item.useTime = 10;
//			item.useStyle = 1;
//			item.mana = 5;
//			item.value = Item.sellPrice(0, 0, 50, 0);
//		}

//		public override bool UseItem(Player player)
//		{
//			Tile tile = Main.tile[(int) Main.MouseWorld.X / 16, (int) Main.MouseWorld.Y / 16];
//			if (!tile.active()) WorldGen.PlaceTile((int) Main.MouseWorld.X / 16, (int) Main.MouseWorld.Y / 16, mod.TileType<LightTile>());
//			return true;
//		}

//		public override void AddRecipes()
//		{
//			ModRecipe recipe = new ModRecipe(mod);
//			recipe.AddIngredient(ItemID.Torch, 100);
//			recipe.AddIngredient(ItemID.Topaz, 2);
//			recipe.AddTile(TileID.Anvils);
//			recipe.SetResult(this);
//			recipe.AddRecipe();
//		}
//	}
//}

