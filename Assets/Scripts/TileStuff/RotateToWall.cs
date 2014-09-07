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
				Vector3 dir = adj.transform.position - transform.position;
				dir.y = 0;
				transform.forward = dir;

				//Vector3 lookpoint = adj.transform.position;
				//lookpoint.y = adj.transform.position.y;
				//transform.LookAt(lookpoint);
				break;
			}
		}
	}
}
