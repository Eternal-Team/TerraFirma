using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using ContainerLibrary;
using TerraFirma.TileEntities;

namespace TerraFirma.UI
{
	public class QuarryPanel : BaseUIPanel<TEQuarry>
	{
		public override void OnInitialize()
		{
			Width = (0, 0.22f);
			Height = (0, 0.17f);
			this.Center();
			SetPadding(8);

			UIGrid<UIContainerSlot> gridItems = new UIGrid<UIContainerSlot>(9)
			{
				Width = (0, 1),
				Height = (0, 1)
			};
			Append(gridItems);

			for (int i = 0; i < Container.Handler.Items.Count; i++)
			{
				UIContainerSlot slot = new UIContainerSlot(() => Container.Handler, i);
				gridItems.Add(slot);
			}
		}
	}
}