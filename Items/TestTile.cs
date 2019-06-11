using BaseLibrary.Items;

namespace TerraFirma.Items
{
	public class TestTile : BaseItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("test tile");
			Tooltip.SetDefault("bzoom");
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
			item.createTile = mod.TileType<Tiles.TestTile>();
		}
	}
}