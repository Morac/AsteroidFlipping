using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileGrid : MonoBehaviour
{

	public int SizeX;
	public int SizeY;

	public float AsteroidRadius = 10;

	public Tile[] SolidTiles;
	public Tile[] BorderTiles;

	public Tile[] FloorTiles;

	public Tile[,] grid { get; private set; }

	public int StartTileX = 3;
	public int StartTileY = 3;

	public List<Vector2> perimeter;

	public GameObject player;

	public void Start()
	{
		grid = new Tile[SizeX, SizeY];

		var solidgrid = GenerateInitialAsteroid();
		solidgrid = CleanUpHoles(solidgrid);

		var tilegrid = ToTileTypeGrid(solidgrid);
		perimeter = GetPerimeter(tilegrid);

		//set up room for spawn point - hardcoding ftw
		var spawnpoint = perimeter[Random.Range(0, perimeter.Count - 1)];
		for (int x = -StartTileX; x < StartTileX; x++)
		{
			for (int y = -StartTileY; y < StartTileY; y++)
			{
				int mx = (int)spawnpoint.x + x;
				int my = (int)spawnpoint.y + y;
				if(InRange(mx,my))
					tilegrid[mx, my] = Tile.TileType.Interior;
			}
		}
		//end spawn point

		perimeter = GetPerimeter(tilegrid); //re-get perimeter because we just changed it while setting up the spawn point
		tilegrid = SetPerimeter(tilegrid, perimeter);

		for (int x = 0; x < SizeX; x++)
		{
			for (int y = 0; y < SizeY; y++)
			{
				var t = tilegrid[x, y];
				Tile[] tiles = null;
				switch (t)
				{
					default:
						break;
					case Tile.TileType.Interior:
						tiles = SolidTiles;
						break;
					case Tile.TileType.Exterior:
						tiles = BorderTiles;
						break;
				}
				SetTile(tiles, x, y, t);
			}
		}

		//do some extra spawn point setup
		for (int x = -StartTileX + 1; x < StartTileX - 1; x++)
		{
			for (int y = -StartTileY + 1; y < StartTileY - 1; y++)
			{
				int mx = (int)spawnpoint.x + x;
				int my = (int)spawnpoint.y + y;
				SetTile(FloorTiles, mx, my);
			}
		}

		player.transform.position = new Vector3(spawnpoint.x, 1, spawnpoint.y);
	}

	public void SetTile(Tile[] tilelist, int x, int y, Tile.TileType type = Tile.TileType.Interior)
	{
		if (!InRange(x, y))
			return;

		if (grid[x, y] != null)
			Destroy(grid[x, y].gameObject);

		if (tilelist == null || tilelist.Length == 0)
			return;

		var point = new Vector3(x, 0, y);
		Tile tile = Instantiate(tilelist[Random.Range(0, tilelist.Length - 1)]) as Tile;
		tile.transform.position = point;
		tile.transform.parent = transform;
		tile.X = (int)point.x;
		tile.Y = (int)point.y;
		tile.type = type;
		tile.tilegrid = this;
		grid[x, y] = tile;
	}


	
	bool[,] GenerateInitialAsteroid()
	{
		bool[,] grid = new bool[SizeX, SizeY];

		Stack<Vector2> growStack = new Stack<Vector2>();
		Vector2 startPoint = new Vector2(SizeX / 2, SizeY / 2);
		growStack.Push(startPoint);

		while (growStack.Count > 0)
		{
			var current = growStack.Pop();
			grid[(int)current.x, (int)current.y] = true;

			foreach (var adj in Adjacents(current))
			{
				if (!grid[(int)adj.x, (int)adj.y] && solid(adj, startPoint))
				{
					growStack.Push(adj);
				}
			}
		}

		return grid;
	}

	bool[,] CleanUpHoles(bool[,] startGrid)
	{
		var grid = startGrid;
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				if (x == 0 || y == 0 || x == grid.GetLength(0) - 1 || y == grid.GetLength(1) - 1)
				{
					grid[x, y] = false;
				}
			}
		}

		HashSet<Vector2> flood = new HashSet<Vector2>();
		Stack<Vector2> stack = new Stack<Vector2>();
		stack.Push(Vector2.zero);

		while (stack.Count > 0)
		{
			var current = stack.Pop();
			flood.Add(current);

			foreach (var adj in Adjacents(current))
			{
				if (InRange(adj) && !flood.Contains(adj) && !grid[(int)adj.x, (int)adj.y])
				{
					stack.Push(adj);
				}
			}
		}

		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				if (!grid[x, y] && !flood.Contains(new Vector2(x, y)))
				{
					grid[x, y] = true;
				}
			}
		}

		return grid;
	}

	Tile.TileType[,] ToTileTypeGrid(bool[,] startgrid)
	{
		var tilegrid = new Tile.TileType[startgrid.GetLength(0), startgrid.GetLength(1)];
		for (int x = 0; x < tilegrid.GetLength(0); x++)
		{
			for (int y = 0; y < tilegrid.GetLength(1); y++)
			{
				if (startgrid[x, y])
				{
					tilegrid[x, y] = Tile.TileType.Interior;
				}
				else
				{
					tilegrid[x, y] = Tile.TileType.Empty;
				}
			}
		}

		return tilegrid;
	}

	List<Vector2> GetPerimeter(Tile.TileType[,] grid)
	{
		HashSet<Vector2> perim = new HashSet<Vector2>();

		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				if (grid[x, y] == Tile.TileType.Interior)
				{
					foreach (var adj in Adjacents(new Vector2(x, y)))
					{
						if (InRange(adj) && grid[(int)adj.x, (int)adj.y] == Tile.TileType.Empty)
						{
							perim.Add(adj);
						}
					}
				}
			}
		}

		return perim.ToList();
	}

	Tile.TileType[,] SetPerimeter(Tile.TileType[,] oldgrid, List<Vector2> perimeter)
	{
		var grid = oldgrid;
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				if (perimeter.Contains(new Vector2(x, y)))
				{
					grid[x, y] = Tile.TileType.Exterior;
				}
			}
		}
		return grid;
	}


	bool InRange(Vector2 v)
	{
		return v.x >= 0 && v.x < SizeX && v.y >= 0 && v.y < SizeY;
	}

	bool InRange(int x, int y)
	{
		return x >= 0 && x < SizeX && y >= 0 && y < SizeY;
	}

	bool solid(Vector2 point, Vector2 startpoint)
	{
		var dist = (point - startpoint).magnitude;
		var percent = dist / AsteroidRadius;
		return Random.value > percent;
	}

	List<Vector2> Adjacents(Vector2 v)
	{
		List<Vector2> adj = new List<Vector2>();
		adj.Add(v + Vector2.up);
		adj.Add(v - Vector2.up);
		adj.Add(v + Vector2.right);
		adj.Add(v - Vector2.right);
		return adj;
	}
}
