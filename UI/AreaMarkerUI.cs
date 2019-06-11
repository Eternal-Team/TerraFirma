//using TheOneLibrary.Base.UI;
//using TheOneLibrary.Utility;
//using WorldInteraction.Items;

//namespace WorldInteraction.UI
//{
//	public class AreaMarkerUI : BaseUI
//	{
//		public AreaMarker marker;

//		//public UIOption xCoordStart;
//		//public UIOption yCoordStart;

//		//public UIOption xCoordEnd;
//		//public UIOption yCoordEnd;

//		public override void OnInitialize()
//		{
//			panelMain.Width.Precent = 0.2f;
//			panelMain.Height.Pixels = 120;
//			panelMain.Center();
//			panelMain.BackgroundColor = panelColor;
//			panelMain.SetPadding(0);
//			panelMain.OnMouseDown += DragStart;
//			panelMain.OnMouseUp += DragEnd;
//			Append(panelMain);

//			//xCoordStart = new UIOption("Start (X): ", 1, 0, Main.maxTilesX);
//			//xCoordStart.Width.Set(panelMain.Width.Pixels - 16f, 0f);
//			//xCoordStart.Height.Set(20f, 0f);
//			//xCoordStart.Left.Set(8f, 0f);
//			//xCoordStart.Top.Set(8f, 0f);
//			//xCoordStart.OnChange += () =>
//			//{
//			//	AreaMarker.markerDataCache[ID].start = new Point16((int)xCoordStart.Value, AreaMarker.markerDataCache[ID].start.Y);
//			//};
//			//panelMain.Append(xCoordStart);

//			//yCoordStart = new UIOption("Start (Y): ", 1, 0, Main.maxTilesY);
//			//yCoordStart.Width.Set(panelMain.Width.Pixels - 16f, 0f);
//			//yCoordStart.Height.Set(20f, 0f);
//			//yCoordStart.Left.Set(8f, 0f);
//			//yCoordStart.Top.Set(36f, 0f);
//			//yCoordStart.OnChange += () =>
//			//{
//			//	AreaMarker.markerDataCache[ID].start = new Point16(AreaMarker.markerDataCache[ID].start.X, (int)yCoordStart.Value);
//			//};
//			//panelMain.Append(yCoordStart);

//			//xCoordEnd = new UIOption("End (X): ", 1, 0, Main.maxTilesX);
//			//xCoordEnd.Width.Set(panelMain.Width.Pixels - 16f, 0f);
//			//xCoordEnd.Height.Set(20f, 0f);
//			//xCoordEnd.Left.Set(8f, 0f);
//			//xCoordEnd.Top.Set(64f, 0f);
//			//xCoordEnd.OnChange += () =>
//			//{
//			//	AreaMarker.markerDataCache[ID].end = new Point16((int)xCoordEnd.Value, AreaMarker.markerDataCache[ID].end.Y);
//			//};
//			//panelMain.Append(xCoordEnd);

//			//yCoordEnd = new UIOption("End (Y): ", 1, 0, Main.maxTilesY);
//			//yCoordEnd.Width.Set(panelMain.Width.Pixels - 16f, 0f);
//			//yCoordEnd.Height.Set(20f, 0f);
//			//yCoordEnd.Left.Set(8f, 0f);
//			//yCoordEnd.Top.Set(92f, 0f);
//			//yCoordEnd.OnChange += () =>
//			//{
//			//	AreaMarker.markerDataCache[ID].end = new Point16(AreaMarker.markerDataCache[ID].end.X, (int)yCoordEnd.Value);
//			//};
//			//panelMain.Append(yCoordEnd);
//		}

//		public void Load(AreaMarker marker)
//		{
//			this.marker = marker;

//			//	xCoordStart.Value = AreaMarker.markerDataCache[ID].start.X;
//			//	yCoordStart.Value = AreaMarker.markerDataCache[ID].start.Y;
//			//	xCoordEnd.Value = AreaMarker.markerDataCache[ID].end.X;
//			//	yCoordEnd.Value = AreaMarker.markerDataCache[ID].end.Y;
//		}
//	}
//}

