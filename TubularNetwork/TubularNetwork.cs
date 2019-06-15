using BaseLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TerraFirma.TileEntities;
using Terraria;
using Terraria.DataStructures;

namespace TerraFirma.TubularNetwork
{
	public class TubularNetwork
	{
		public static List<TubularNetwork> Networks = new List<TubularNetwork>();

		public List<Tube> tiles;
		public Color debugColor;
		public List<Point16> debugPath;
		public List<TransportingPlayer> TransportingPlayers;

		public TubularNetwork()
		{
			Networks.Add(this);

			tiles = new List<Tube>();
			TransportingPlayers = new List<TransportingPlayer>();

			debugColor = new Color(Main.rand.NextFloat(), Main.rand.NextFloat(), Main.rand.NextFloat());
		}
		
		public void AddTile(Tube tile)
		{
			if (!tiles.Contains(tile))
			{
				Networks.Remove(tile.Network);
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
			Networks.Remove(this);

			for (int i = 0; i < tiles.Count; i++)
			{
				tiles[i].Network = new TubularNetwork
				{
					tiles = new List<Tube> { tiles[i] }
				};
			}

			for (int i = 0; i < tiles.Count; i++) tiles[i].Merge();
		}

		public void Update()
		{
			for (int i = 0; i < TransportingPlayers.Count; i++)
			{
				TransportingPlayer player = TransportingPlayers[i];
				player.Update();
				if (player.path.Count == 0) TransportingPlayers.Remove(player);
			}
		}

		public IEnumerable<TEEntryPoint> GetEntryPoints()
		{
			foreach (Tube tube in tiles)
			{
				TEEntryPoint entryPoint = TerraFirma.Instance.GetTileEntity<TEEntryPoint>(tube.Position);
				if (entryPoint != null) yield return entryPoint;
			}
		}

		class Location
		{
			public int X;
			public int Y;
			public int F;
			public int G;
			public int H;
			public Location Parent;
		}

		private int ComputeHScore(int x, int y, int targetX, int targetY) => Math.Abs(targetX - x) + Math.Abs(targetY - y);

		private IEnumerable<Location> GetWalkableAdjacentSquares(int x, int y)
		{
			return tiles.First(tube => tube.Position.X == x && tube.Position.Y == y).GetNeighbors().Select(neighbor => new Location { X = neighbor.Position.X, Y = neighbor.Position.Y });
		}

		public Stack<Point16> FindPath(Point16 startPos, Point16 endPos)
		{
			Location current = null;
			var start = new Location { X = startPos.X, Y = startPos.Y };
			var target = new Location { X = endPos.X, Y = endPos.Y };
			var openList = new List<Location>();
			var closedList = new List<Location>();
			int g = 0;

			// start by adding the original position to the open list
			openList.Add(start);

			while (openList.Count > 0)
			{
				// get the square with the lowest F score
				var lowest = openList.Min(l => l.F);
				current = openList.First(l => l.F == lowest);

				// add the current square to the closed list
				closedList.Add(current);

				// remove it from the open list
				openList.Remove(current);

				// if we added the destination to the closed list, we've found a path
				if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
					break;

				// get neighbors
				var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y);
				g++;

				foreach (var adjacentSquare in adjacentSquares)
				{
					// if this adjacent square is already in the closed list, ignore it
					if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
					                                   && l.Y == adjacentSquare.Y) != null)
						continue;

					// if it's not in the open list...
					if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
					                                 && l.Y == adjacentSquare.Y) == null)
					{
						// compute its score, set the parent
						adjacentSquare.G = g;
						adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
						adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
						adjacentSquare.Parent = current;

						// and add it to the open list
						openList.Insert(0, adjacentSquare);
					}
					else
					{
						// test if using the current G score makes the adjacent square's F score
						// lower, if yes update the parent because it means it's a better path
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

	public class TransportingPlayer
	{
		public Player player;
		public Stack<Point16> path;
		public Point16 CurrentPosition;

		private int timer = 0;

		public void Update()
		{
			if (++timer > 10)
			{
				CurrentPosition = path.Pop();

				timer = 0;
			}
		}
	}
}