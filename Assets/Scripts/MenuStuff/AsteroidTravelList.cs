using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AsteroidTravelList : GenericUIList
{
	protected override List<object> ListData()
	{
		var list = GameManager.CurrentAsteroids();
		list.RemoveAll
		(
			item =>
			{
				return item == GameManager.Instance.AsteroidName;
			}
		);
		return list.Cast<object>().ToList();
	}

	protected override void OnListItemCreate(ListItem item)
	{
		item.Button0Clicked += Travel;
		item.Labels[0].text = (string)item.Data;
	}

	void Travel(ListItem item)
	{
		FindObjectOfType<GameManager>().Save();
		GameManager.LoadAsteroid((string)item.Data);
	}
}
