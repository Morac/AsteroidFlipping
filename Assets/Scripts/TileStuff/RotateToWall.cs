using UnityEngine;
using System.Collections;

public class RotateToWall : MonoBehaviour 
{

	void Start()
	{
		var tile = GetComponentInParent<Tile>();

		foreach(var adj in tile.AdjacentTiles(false))
		{
			if(adj.CanBeAttachedTo)
			{
				transform.LookAt(adj.transform);
				break;
			}
		}
	}
}
