using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RandomVariations : MonoBehaviour
{

	public float ScaleVariation = 0.1f;
	
	void Start()
	{
		//transform.localRotation = Quaternion.Euler(new Vector3(90, Random.Range(0, 4) * 90, 0));
		transform.localScale *= Random.Range(1 - ScaleVariation, 1 + ScaleVariation);
	}


	void OnAdjacentUpdated()
	{
		if (GetComponent<SpriteRenderer>() == null)
			return;

		var tile = GetComponentInParent<Tile>();

		var sorting = new HashSet<int>();
		for (int i = 0; i < 9; i++)
		{
			sorting.Add(i);
		}
		
		foreach(var item in tile.AdjacentTiles(true))
		{
			if (item == null)
				continue;
			foreach(var sprite in item.GetComponentsInChildren<SpriteRenderer>())
			{
				sorting.Remove(sprite.sortingOrder);
			}
		}

		var list = sorting.ToList();
		GetComponent<SpriteRenderer>().sortingOrder = list[Random.Range(0, list.Count)];
	}
}
