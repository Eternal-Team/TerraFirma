using BaseLibrary.Items;
using Terraria;
using Terraria.ModLoader;

namespace TerraFirma.Items
{
	public class Quarry : BaseItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quarry");
			Tooltip.SetDefault("Mines out an area");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType<Tiles.Quarry>();
			item.value = Item.sellPrice(0, 0, 50);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}