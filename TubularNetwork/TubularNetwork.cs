using BaseLibrary;
using System.Collections.Generic;
using TerraFirma.TileEntities;

namespace TerraFirma.Network
{
	public class TubularNetwork
	{
		public static List<TubularNetwork> Networks = new List<TubularNetwork>();

		public List<Tube> Tiles { get; }

		public List<TransportingPlayer> TransportingPlayers;

		public TubularNetwork(Tube tube)
		{
			Networks.Add(this);

			Tiles = new List<Tube> { tube };
			TransportingPlayers = new List<TransportingPlayer>();
		}

		public void AddTile(Tube tile)
		{
			if (!Tiles.Contains(tile))
			{
				Networks.Remove(tile.Network);
				tile.Network = this;
				Tiles.Add(tile);
			}
		}

		public void RemoveTile(Tube tile)
		{
			if (Tiles.Contains(tile))
			{
				Tiles.Remove(tile);
				Reform();
			}
		}

		public void Merge(TubularNetwork other)
		{
			for (int i = 0; i < other.Tiles.Count; i++) AddTile(other.Tiles[i]);
		}

		public void Reform()
		{
			Networks.Remove(this);

			for (int i = 0; i < Tiles.Count; i++) Tiles[i].Network = new TubularNetwork(Tiles[i]);

			for (int i = 0; i < Tiles.Count; i++) Tiles[i].Merge();
		}

		public void Update()
		{
			for (int i = 0; i < TransportingPlayers.Count; i++)
			{
				TransportingPlayer transportingPlayer = TransportingPlayers[i];
				transportingPlayer.Update();

				if (!transportingPlayer.player.UsingTubeSystem) TransportingPlayers.Remove(transportingPlayer);
			}
		}

		public IEnumerable<EntryPoint> GetEntryPoints()
		{
			foreach (Tube tube in Tiles)
			{
				EntryPoint entryPoint = Utility.GetTileEntity<EntryPoint>(tube.Position);
				if (entryPoint != null) yield return entryPoint;
			}
		}
	}
}