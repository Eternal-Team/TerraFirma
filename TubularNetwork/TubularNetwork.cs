using BaseLibrary;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraFirma.TileEntities;
using Terraria;

namespace TerraFirma.TubularNetwork
{
	public class TubularNetwork
	{
		public List<Tube> tiles = new List<Tube>();
		public Color debugColor;

		public TubularNetwork()
		{
			debugColor = new Color(Main.rand.NextFloat(), Main.rand.NextFloat(), Main.rand.NextFloat());
		}

		public void AddTile(Tube tile)
		{
			if (!tiles.Contains(tile))
			{
				tile.Network = this;
				tiles.Add(tile);
			}
		}

		public void RemoveTile(Tube tile)
		{
			if (tiles.Contains(tile))
			{
				tiles.Remove(tile);
				Reform();
			}
		}

		public void Merge(TubularNetwork other)
		{
			for (int i = 0; i < other.tiles.Count; i++) AddTile(other.tiles[i]);
		}

		public void Reform()
		{
			for (int i = 0; i < tiles.Count; i++)
			{
				tiles[i].Network = new TubularNetwork
				{
					tiles = new List<Tube> { tiles[i] }
				};
			}

			for (int i = 0; i < tiles.Count; i++) tiles[i].Merge();
		}

		public IEnumerable<TEEntryPoint> GetEntryPoints()
		{
			foreach (Tube tube in tiles)
			{
				TEEntryPoint entryPoint = TerraFirma.Instance.GetTileEntity<TEEntryPoint>(tube.Position);
				if (entryPoint != null) yield return entryPoint;
			}
		}
	}
}