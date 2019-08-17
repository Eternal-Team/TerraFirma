using BaseLibrary.UI.Elements;
using TerraFirma.TileEntities;
using Terraria.UI;

namespace TerraFirma.UI
{
	public class UIEntryPointItem : UIPanel, IGridElement<UIEntryPointItem>
	{
		public EntryPoint EntryPoint;
		//public bool Selected;

		public UIGrid<UIEntryPointItem> Grid { get; set; }

		public UIEntryPointItem(EntryPoint entryPoint)
		{
			EntryPoint = entryPoint;

			UIText textDisplayName = new UIText(entryPoint.Position.ToString());
			Append(textDisplayName);
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);

			if (evt.Target != this) return;

			//Grid.Items.ForEach(item => item.Selected = false);
			//Selected = true;
		}

		//public override void PreDraw(SpriteBatch spriteBatch)
		//{
		//	BackgroundColor = IsMouseHovering ? Utility.ColorPanel_Hovered : Selected ? Utility.ColorPanel_Selected : Utility.ColorPanel;
		//}
	}
}