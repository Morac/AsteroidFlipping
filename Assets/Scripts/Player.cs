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
}
