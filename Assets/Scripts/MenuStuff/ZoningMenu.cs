using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ZoningMenu : GenericUIList
{

	public GameObject MenuTopLevel;

	protected override List<object> ListData()
	{
		var types = System.Enum.GetValues(typeof(RoomManager.RoomType));
		var retlist = new List<object>();
		foreach(var t in types)
		{
			if(t != Player.Instance.CurrentTile)
			{
				retlist.Add(t);
			}
		}
		return retlist;
	}

	public override List<string> LabelNames()
	{
		return new List<string>() { "Zone Name" };
	}

	protected override void OnListItemCreate(ListItem item)
	{
		item.Button0Clicked += Zone;
		var typename = ((RoomManager.RoomType)item.Data).ToString();
		item.Labels[0].text = typename;
	}

	void Zone(ListItem source)
	{
		var type = (RoomManager.RoomType)source.Data;
		RoomManager.Instance.SetRoom(type, Player.Instance.CurrentTile);
		MenuTopLevel.SetActive(false);
	}

}
