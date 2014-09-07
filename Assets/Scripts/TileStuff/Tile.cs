using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour 
{

	public enum TileType
	{
		Interior,
		Exterior,
		Empty
	}

	public bool CanBeAttachedTo = false;

	[HideInInspector]
	public int X;
	[HideInInspector]
	public int Y;

	[HideInInspector]
	public TileType type;
	[HideInInspector]
	public TileGrid tilegrid;

	public void SetTile(params Tile[] tilelist)
	{
		tilegrid.SetTile(tilelist, X, Y, type);
	}

	public List<Tile> AdjacentTiles(bool diagonals)
	{
		List<Tile> list = new List<Tile>();
		foreach(var adj in tilegrid.Adjacents(new Vector2(X,Y), diagonals))
		{
			list.Add(tilegrid.grid[(int)adj.x, (int)adj.y]);
		}
		return list;
	}
}
