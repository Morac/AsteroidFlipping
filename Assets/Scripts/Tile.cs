using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{

	public enum TileType
	{
		Interior,
		Exterior,
		Empty
	}

	public int X;
	public int Y;

	public TileType type;
	public TileGrid tilegrid;

	public void SetTile(Tile[] tilelist, TileType type = TileType.Interior)
	{
		tilegrid.SetTile(tilelist, X, Y, type);
	}
}
