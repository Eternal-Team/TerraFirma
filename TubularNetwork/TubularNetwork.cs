using BaseLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.DataStructures;

namespace TerraFirma.Network
{
	public class TubularNetwork
	{
		public static List<TubularNetwork> Networks = new List<TubularNetwork>();

		public List<Tube> Tiles { get; }

		public Color debugColor;

		public List<TransportingPlayer> TransportingPlayers;

		public TubularNetwork(Tube tube)
		{
			Networks.Add(this);

			Tiles = new List<Tube> { tube };
			TransportingPlayers = new List<TransportingPlayer>();

			debugColor = new Color(Main.rand.NextFloat(), Main.rand.NextFloat(), Main.rand.NextFloat());
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

				TFPlayer player = transportingPlayer.player;
				if (!player.UsingTubeSystem) TransportingPlayers.Remove(transportingPlayer);
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

		// todo: put this into baselibrary or layerlibrary
		internal static class Pathfinding
		{
			private class Node
			{
				public int X;
				public int Y;

				public int F;
				public int G;
				public int H;

				public Node Parent;
			}

			private static int ComputeHScore(Node current, Node target) => Math.Abs(target.X - current.X) + Math.Abs(target.Y - current.Y);

			private static IEnumerable<Node> GetNeighborNodes(IEnumerable<Tube> Tiles, Node node)
			{
				return Tiles.First(tube => tube.Position.X == node.X && tube.Position.Y == node.Y).GetNeighbors().Select(neighbor => new Node { X = neighbor.Position.X, Y = neighbor.Position.Y });
			}

			public static Stack<Point16> FindPath(List<Tube> network, Point16 startPos, Point16 endPos)
			{
				Node current = null;
				var start = new Node { X = startPos.X, Y = startPos.Y };
				var target = new Node { X = endPos.X, Y = endPos.Y };
				var openList = new List<Node>();
				var closedList = new List<Node>();
				int g = 0;

				openList.Add(start);

				while (openList.Count > 0)
				{
					var lowest = openList.Min(l => l.F);
					current = openList.First(l => l.F == lowest);

					closedList.Add(current);

					openList.Remove(current);

					if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null) break;

					var adjacentSquares = GetNeighborNodes(network, current);
					g++;

					foreach (var adjacentSquare in adjacentSquares)
					{
						if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y) != null) continue;

						if (openList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y) == null)
						{
							adjacentSquare.G = g;
							adjacentSquare.H = ComputeHScore(adjacentSquare, target);
							adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
							adjacentSquare.Parent = current;

							openList.Insert(0, adjacentSquare);
						}
						else
						{
							if (g + adjacentSquare.H < adjacentSquare.F)
							{
								adjacentSquare.G = g;
								adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
								adjacentSquare.Parent = current;
							}
						}
					}
				}

				Stack<Point16> points = new Stack<Point16>();
				while (current != null)
				{
					points.Push(new Point16(current.X, current.Y));

					current = current.Parent;
				}

				return points;
			}
		}
	}
}