using BaseLibrary;
using LayerLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerraFirma
{
	// builder, spawner, grinder, transport pipes - Tubular Network? (items, fluids, NPCs, Players), farms, item collector (vortex chest)

	public class TerraFirma : Mod
	{
		// entire transport pipes layer shifted by 8 on X-axis
		public static TerraFirma Instance;

		public TubularNetworkLayer layer;

		public override void Load()
		{
			Instance = this;

			Hooking.Initialize();

			layer = new TubularNetworkLayer();
		}

		public override void Unload()
		{
			Hooking.Uninitialize();

			Utility.UnloadNullableTypes();
		}
	}

	public class TubularNetworkLayer : ModLayer<Tube>
	{
		public override string Name => "Tubular Network";

		public override bool Place(BaseLayerItem<Tube> item)
		{
			int posX = (int)(Main.MouseWorld.X / 16f);
			int posY = (int)(Main.MouseWorld.Y / 16f);
			Main.NewText("posX: " + posX); // even X
			Main.NewText("posY: " + posY); // odd Y

			if (posX % 2 != 0) posX -= 1;
			if (posY % 2 == 0) posY -= 1;
			
			if (!ContainsKey(posY, posY))
			{
				Tube element = new Tube
				{
					Position = new Point16(posX, posY),
					Frame = Point16.Zero,
					Layer = this
				};
				data.Add(new Point16(posX, posY), element);

				element.UpdateFrame();

				if (ContainsKey(posX + 2, posY)) this[posX + 2, posY].UpdateFrame();
				if (ContainsKey(posX - 2, posY)) this[posX - 2, posY].UpdateFrame();
				if (ContainsKey(posX, posY + 2)) this[posX, posY + 2].UpdateFrame();
				if (ContainsKey(posX, posY - 2)) this[posX, posY - 2].UpdateFrame();

				return true;
			}

			return false;
		}

		public override void Remove(BaseLayerItem<Tube> item)
		{
			base.Remove(item);
		}
	}

	public class Tube : ModLayerElement
	{
		public override string Texture => "TerraFirma/Textures/TubeNetwork";

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 position = Position.ToVector2() * 16 - Main.screenPosition;
			Color color = Lighting.GetColor(Position.X, Position.Y);
			spriteBatch.Draw(ModContent.GetTexture(Texture), position + new Vector2(8, 8), new Rectangle(Frame.X, Frame.Y, 32, 32), color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
		}

		public override void UpdateFrame()
		{
			short frameX = 0, frameY = 0;
			if (Layer.ContainsKey(Position.X - 2, Position.Y)) frameX += 34;
			if (Layer.ContainsKey(Position.X + 2, Position.Y)) frameX += 68;
			if (Layer.ContainsKey(Position.X, Position.Y - 2)) frameY += 34;
			if (Layer.ContainsKey(Position.X, Position.Y + 2)) frameY += 68;
			Frame = new Point16(frameX, frameY);
		}
	}

	public class TubeItem : BaseLayerItem<Tube>
	{
		public override ModLayer<Tube> Layer => TerraFirma.Instance.layer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tube");
		}
	}
}