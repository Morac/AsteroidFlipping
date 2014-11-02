using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{
	Behaviour _controller;
	public Behaviour Controller
	{
		get
		{
			if (_controller == null)
				_controller = GetComponent("PlayerMoveController") as Behaviour;
			return _controller;
		}
	}

	public PlayerTool MainTool;

	public Tile CurrentTile;

	void Update()
	{
		var hits = Physics.RaycastAll(transform.position, -transform.up);
		CurrentTile = null;
		foreach(var hit in hits)
		{
			var tile = hit.transform.GetComponentInParents<Tile>();
			if(tile != null)
			{
				CurrentTile = tile;
				break;
			}
		}

		if (CurrentTile != null)
		{
			if (CurrentTile.Room == null && CurrentTile.IsRoomBorder == false)
			{
				RoomManager.Instance.SetRoom(RoomManager.RoomType.Unzoned, CurrentTile);
			}
		}
	}
}
