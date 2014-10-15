using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TileGrid))]
public class RoomManager : MonoBehaviour
{
	public enum RoomType
	{
		Bedroom,
		Garden,
		Workshop,
		LifeSupport
	}

	public class Room
	{
		public RoomType Type;
		public List<Tile> Tiles = new List<Tile>();
	}

	public List<Room> Rooms = new List<Room>();

	public void SetRoom(RoomType type, Tile startTile)
	{
		
		Stack<Tile> stack = new Stack<Tile>();
		stack.Push(startTile);

		var room = startTile.Room;
		if(room == null)
		{
			room = new Room();
		}
		room.Type = type;
		room.Tiles.Clear();

		while(stack.Count > 0)
		{
			var current = stack.Pop();
			room.Tiles.Add(current);
			current.Room = room;

			foreach(var adj in current.AdjacentTiles(false))
			{
				if (adj != null && adj.IsRoomBorder == false)
					stack.Push(adj);
			}
		}
	}

	public bool VerifyReplace(Tile currentTile, Tile newTile)
	{
		if (!currentTile.IsRoomBorder)
			return true;

		if(newTile.IsRoomBorder)
			return true;

		HashSet<Room> borderedRooms = new HashSet<Room>();
		foreach(var adj in currentTile.AdjacentTiles(false))
		{
			if(adj != null)
			{
				borderedRooms.Add(adj.Room);
			}
		}

		return borderedRooms.Count <= 1;
	}
}
