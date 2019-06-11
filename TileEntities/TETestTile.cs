using System;
using BaseLibrary.Tiles.TileEntites;
using Microsoft.Xna.Framework;
using TerraFirma.Tiles;
using Terraria;

namespace TerraFirma.TileEntities
{
	public class TETestTile : BaseTE
	{
		public static float scale = 1f;

		public Rectangle Hitbox => new Rectangle(Position.X * 16, Position.Y * 16 - 48, 48, 48);

		public override Type TileType => typeof(TestTile);

		public override void Update()
		{
            if (scale > 0.5f) scale -= 0.005f;

            foreach (Player player in Main.player)
			{
				if (player.active && !player.dead)
				{
					TFPlayer tfPlayer = player.GetModPlayer<TFPlayer>();
                    tfPlayer.Miniaturizing = true;
				}
			}
		}
	}
}