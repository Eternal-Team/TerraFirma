using BaseLibrary.Tiles.TileEntites;
using BaseLibrary.UI;
using System;
using Terraria.Audio;
using Terraria.ID;

namespace TerraFirma.TileEntities
{
	public class EntryPoint : BaseTE, IHasUI
	{
		public override Type TileType => typeof(Tiles.EntryPoint);

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound => SoundID.Item1;
		public LegacySoundStyle OpenSound => SoundID.Item1;
	}
}