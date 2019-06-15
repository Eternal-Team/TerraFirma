using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TerraFirma
{
	public class TFWorld : ModWorld
	{
		public override TagCompound Save() => new TagCompound
		{
			["TubularNetwork"] = TerraFirma.Instance.TubeNetworkLayer.Save()
		};

		public override void Load(TagCompound tag)
		{
			TerraFirma.Instance.TubeNetworkLayer.Load(tag.GetList<TagCompound>("TubularNetwork").ToList());
		}

		public override void PostUpdate()
		{
			TerraFirma.Instance.TubeNetworkLayer.Update();
		}
	}
}