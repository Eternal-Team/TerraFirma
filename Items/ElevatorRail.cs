using BaseLibrary.Items;
using Terraria;

namespace TerraFirma.Items
{
	public class ElevatorRail : BaseItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elevator Rail");
			Tooltip.SetDefault("Not to be confused with trains");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType<Tiles.ElevatorRail>();
			item.value = Item.sellPrice(0, 0, 50);
		}
	}
}