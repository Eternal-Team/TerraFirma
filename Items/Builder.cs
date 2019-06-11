//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using TheOneLibrary.Base.Items;

//namespace WorldInteraction.Items
//{
//	public class Builder : BaseItem
//	{
//		public override string Texture => WIMod.ItemTexturePath + "Builder";

//		public override void SetStaticDefaults()
//		{
//			DisplayName.SetDefault("Builder");
//			Tooltip.SetDefault("Automatically builds tiles");
//		}

//		public override void SetDefaults()
//		{
//			item.width = 16;
//			item.height = 16;
//			item.maxStack = 99;
//			item.useTurn = true;
//			item.autoReuse = true;
//			item.useAnimation = 15;
//			item.useTime = 10;
//			item.useStyle = 1;
//			item.consumable = true;
//			item.createTile = mod.TileType<Tiles.Builder>();
//			item.value = Item.sellPrice(0, 0, 50, 0);
//		}

//		public override void AddRecipes()
//		{
//			ModRecipe recipe = new ModRecipe(mod);
//			recipe.AddIngredient(ItemID.GrayBrick, 100);
//			recipe.AddIngredient(ItemID.IronBar, 2);
//			recipe.AddTile(TileID.Anvils);
//			recipe.anyIronBar = true;
//			recipe.SetResult(this);
//			recipe.AddRecipe();
//		}
//	}
//}

