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
				if (item == GameManager.Instance.AsteroidName)
					return true;
				System.IO.StreamReader reader = new System.IO.StreamReader(GameManager.SavePath(item));
				return bool.Parse(reader.ReadLine());
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
