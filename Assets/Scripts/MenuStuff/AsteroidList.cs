using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AsteroidList : MonoBehaviour {

	public AsteroidListItem listItemPrefab;

	const int asteroidCount = 4;

	void Start()
	{
		List<AsteroidListItem> list = new List<AsteroidListItem>();
		var free = Instantiate(listItemPrefab, transform.position, transform.rotation) as AsteroidListItem;
		free.SetupFree();
		list.Add(free);

		for (int i = 0; i < asteroidCount; i++)
		{
			var item = Instantiate(listItemPrefab, transform.position, transform.rotation) as AsteroidListItem;
			item.Setup();
			list.Add(item);
		}

		list = list.OrderBy(item => item.cost).ToList();

		foreach(var item in list)
		{
			item.transform.parent = transform;
			item.transform.localScale = Vector3.one;
		}

		GetComponent<UIGrid>().Reposition();
	}

}
