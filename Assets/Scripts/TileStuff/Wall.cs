using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tile))]
public class Wall : MonoBehaviour
{

	public GameObject visible;
	public GameObject hidden;

	Tile tile;

	void Start()
	{
		tile = GetComponent<Tile>();
		OnAdjacentUpdated();
	}

	void OnAdjacentUpdated()
	{
		if (tile == null)
			return;
		foreach (var adjacent in tile.AdjacentTiles(false))
		{
			if (adjacent == null)
				continue;
			if (adjacent.name != name && adjacent.type != Tile.TileType.Exterior)
			{
				visible.SetActive(true);
				hidden.SetActive(false);
				return;
			}
		}
		visible.SetActive(false);
		hidden.SetActive(true);
	}
}
