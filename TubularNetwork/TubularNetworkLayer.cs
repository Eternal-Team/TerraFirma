using LayerLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace TerraFirma.Network
{
	public class TubularNetworkLayer : ModLayer<Tube>
	{
		public override int TileSize => 3;

		public override void Load(List<TagCompound> list)
		{
			base.Load(list);

			foreach (Tube tube in data.Values) tube.Merge();
		}

		public override void Update()
		{
			base.Update();

			foreach (TubularNetwork network in TubularNetwork.Networks) network.Update();
		}

		public void PostDraw(SpriteBatch spriteBatch)
		{
			if (!Visible) return;

			DrawPreview(Main.spriteBatch);

			if (data.Count <= 0) return;

			Vector2 zero = new Vector2(Main.offScreenRange);
			if (Main.drawToScreen) zero = Vector2.Zero;

			int startX = (int)((Main.screenPosition.X - zero.X) / 16f) - 3;
			int endX = (int)((Main.screenPosition.X + Main.screenWidth + zero.X) / 16f) + 3;
			int startY = (int)((Main.screenPosition.Y - zero.Y) / 16f) - 3;
			int endY = (int)((Main.screenPosition.Y + Main.screenHeight + zero.Y) / 16f) + 3;

			foreach (KeyValuePair<Point16, Tube> pair in data)
			{
				if (pair.Key.X > startX && pair.Key.X < endX && pair.Key.Y > startY && pair.Key.Y < endY)
				{
					pair.Value.PostDraw(spriteBatch);
				}
			}
		}
	}
}