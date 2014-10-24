using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TileGrid))]
public class RoomManager : Singleton<RoomManager>
{
	public enum RoomType
	{
		None,
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

		var room = new Room();
		room.Type = type;
		room.Tiles.Clear();

		int bail = 0;
		while (stack.Count > 0)
		{
			var current = stack.Pop();
			room.Tiles.Add(current);
			if(current.Room != null)
			{
				current.Room.Tiles.Remove(current);
			}
			current.Room = room;

			//set floor tile
			if (current.DefaultFloor != null)
			{
				var floors = TilePrefabList.Instance.GetAllFloorsPrefabsForType(type);
				if (floors.Count > 0)
				{
					if (current.CurrentFloor != null)
						Destroy(current.CurrentFloor.gameObject);
					var newfloor = Instantiate(floors[Random.Range(0, floors.Count)]) as Floor;
					newfloor.transform.position = current.DefaultFloor.transform.position;
					newfloor.transform.parent = current.transform;
					current.CurrentFloor = newfloor;
					current.DefaultFloor.SetActive(false);
				}
				else
				{
					current.DefaultFloor.SetActive(true);
				}
			}

			//push adjacent
			foreach (var adj in current.AdjacentTiles(false))
			{
				if (adj != null && adj.IsRoomBorder == false && adj.Room != room)
					stack.Push(adj);
			}

			bail++;
			if (bail > 10000)
			{
				Debug.LogError("!");
				break;
			}
		}

		var copy = Rooms;
		foreach(var r in copy)
		{
			if(r.Tiles.Count == 0)
			{
				Rooms.Remove(r);
			}
		}
	}

	public void AddToRoom(Room room, Tile tile)
	{
		if (tile.Room != null)
			tile.Room.Tiles.Remove(tile);
		room.Tiles.Add(tile);
		tile.Room = room;

		if (tile.DefaultFloor != null)
		{
			var floors = TilePrefabList.Instance.GetAllFloorsPrefabsForType(room.Type);	
			if (floors.Count > 0)
			{
				if (tile.CurrentFloor != null)
					Destroy(tile.CurrentFloor.gameObject);
				var newfloor = Instantiate(floors[Random.Range(0, floors.Count)]) as Floor;
				newfloor.transform.position = tile.DefaultFloor.transform.position;
				newfloor.transform.parent = tile.transform;
				tile.CurrentFloor = newfloor;
				tile.DefaultFloor.SetActive(false);
			}
			else
			{
				tile.DefaultFloor.SetActive(true);
			}
		}
	}

	public bool VerifyReplace(Tile currentTile, Tile newTile)
	{
		if (!currentTile.IsRoomBorder)
			return true;

		if (newTile.IsRoomBorder)
			return true;

		HashSet<Room> borderedRooms = new HashSet<Room>();
		foreach (var adj in currentTile.AdjacentTiles(false))
		{
			if (adj != null)
			{
				borderedRooms.Add(adj.Room);
			}
		}

		return borderedRooms.Count <= 1;
	}
}
