using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerraFirma
{
	public class TFWorld : ModWorld
	{
		public override void PostDrawTiles()
		{
			Main.spriteBatch.Begin();
			TerraFirma.Instance.layer.Draw(Main.spriteBatch);
			Main.spriteBatch.End();
		}

		public override TagCompound Save() => new TagCompound
		{
			["TubularNetwork"] = TerraFirma.Instance.layer.Save()
		};

		public override void Load(TagCompound tag)
		{
			TerraFirma.Instance.layer.Load(tag.GetList<TagCompound>("TubularNetwork").ToList());
		}
	}
}