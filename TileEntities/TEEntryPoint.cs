using BaseLibrary.Tiles.TileEntites;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using System;
using TerraFirma.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraFirma.TileEntities
{
	public class TEEntryPoint : BaseTE, IHasUI
	{
		public Rectangle Hitbox => new Rectangle(Position.X * 16, Position.Y * 16, 48, 56);

		public override Type TileType => typeof(EntryPoint);

		public override void Update()
		{
			foreach (Player player in Main.player)
			{
				if (player.active && !player.dead)
				{
					TFPlayer tfPlayer = player.GetModPlayer<TFPlayer>();
					tfPlayer.Miniaturizing = false;
				}
			}
		}

		public Guid ID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound => SoundID.Item1;
		public LegacySoundStyle OpenSound => SoundID.Item1;
	}
}