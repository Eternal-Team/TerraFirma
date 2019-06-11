using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraFirma
{
    public class TFPlayer : ModPlayer
    {
        public bool Miniaturizing = false;
        public bool Maximizing = false;
        public bool InTube;

        private float scale = 1f;

        public override void PreUpdate()
        {
            if (Miniaturizing)
            {
                if (scale > 0.75f) scale -= 0.005f;
                if (scale < 0.75f) scale += 0.005f;
				else InTube = true;
            }
			scale = 1f;
            if (InTube || Miniaturizing || Maximizing)
            {
                player.width = (int)(Player.defaultWidth * scale);

                player.position.Y += player.height;
                player.height = (int)(Player.defaultHeight * scale);
                player.position.Y -= player.height;

                player.immune = true;
                player.immuneTime = 2;
                player.immuneNoBlink = true;
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {

        }
    }
}