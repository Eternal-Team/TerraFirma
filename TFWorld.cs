using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerraFirma
{
	public class TFWorld : ModWorld
	{
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