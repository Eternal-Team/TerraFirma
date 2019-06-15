using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using Microsoft.Xna.Framework;
using TerraFirma.TileEntities;
using Terraria;

namespace TerraFirma.UI
{
	public class EntryPointPanel : BaseUIPanel<TEEntryPoint>
	{
		private UIGrid<UIEntryPointItem> gridLocations;

		private BaseElement elementMain;

		public override void OnInitialize()
		{
			Width = (0, 0.25f);
			Height = (0, 0.4f);
			this.Center();
			SetPadding(8);

			// Top Panel
			{
				UIText textName = new UIText("Entry Point")
				{
					Width = (-56, 1),
					Height = (20, 0),
					HAlign = 0.5f
				};
				textName.GetHoverText += () => "Click to change name";
				Append(textName);

				UITextButton buttonClose = new UITextButton("X")
				{
					Size = new Vector2(20),
					Left = (-20, 1),
					RenderPanel = false,
					Padding = (0, 0, 0, 0)
				};
				buttonClose.GetHoverText += () => "Close";
				buttonClose.OnClick += (evt, element) => BaseLibrary.BaseLibrary.PanelGUI.UI.CloseUI(Container);
				Append(buttonClose);
			}

			// Middle Panel
			{
				// Main
				{
					elementMain = new BaseElement
					{
						Width = (0, 1),
						Height = (-28, 1),
						Top = (28, 0)
					};
					Append(elementMain);

					UIPanel panelLocations = new UIPanel
					{
						Width = (0, 1),
						Height = (-48, 1)
					};
					elementMain.Append(panelLocations);

					gridLocations = new UIGrid<UIEntryPointItem>
					{
						Width = (0, 1),
						Height = (0, 1)
					};
					panelLocations.Append(gridLocations);
					PopulateGrid();
				}
			}
		}

		public void PopulateGrid()
		{
			gridLocations.Clear();

			foreach (TEEntryPoint entryPoint in TerraFirma.Instance.TubeNetworkLayer[Container.Position].Network.GetEntryPoints())
			{
				if (entryPoint == Container) continue;

				UIEntryPointItem entryPointItem = new UIEntryPointItem(entryPoint)
				{
					Width = (0, 1),
					Height = (60, 0)
				};
				entryPointItem.OnClick += (evt, element) =>
				{
					if (Main.LocalPlayer.getRect().Intersects(Container.Hitbox)) Main.LocalPlayer.Bottom = entryPoint.Hitbox.Bottom();
				};
				gridLocations.Add(entryPointItem);
			}
		}
	}
}