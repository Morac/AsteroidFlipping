using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(TileGrid))]
public class RoomManager : Singleton<RoomManager>
{
	public enum RoomType
	{
		Unzoned = 1,
		Bedroom = 2,
		Garden = 4,
		Workshop = 8,
		LifeSupport = 16
	}

	public enum RoomQuality
	{
		None = 0,
		Poor = 1,
		Moderate = 5,
		Good = 10,
		Exquisite = 20
	}


	public class Room
	{
		public RoomType Type;

		RoomQuality quality;
		public RoomQuality Quality
		{
			get
			{
				return quality;
			}
		}

		List<Tile> tiles = new List<Tile>();
		public List<Tile> Tiles
		{
			get
			{
				return tiles;
			}
		}

		public void AddTile(Tile t)
		{
			if (!tiles.Contains(t))
			{
				tiles.Add(t);
				RecalculateQuality();
			}
		}

		public void RemoveTile(Tile t)
		{
			tiles.Remove(t);
			RecalculateQuality();
		}

		public void RecalculateQuality()
		{
			int value = 0;
			foreach(var tile in tiles)
			{
				var val = tile.GetComponent<Valuable>();
				if(val != null)
				{
					value += val.value;
				}
			}
			foreach(var val in System.Enum.GetValues(typeof(RoomQuality)))
			{
				if(value > (int)val)
				{
					quality = (RoomQuality)val;
				}
				else
				{
					break;
				}
			}
		}
	}

	public List<Room> Rooms = new List<Room>();

	public void SetRoom(RoomType type, Tile startTile)
	{

		Stack<Tile> stack = new Stack<Tile>();
		stack.Push(startTile);

		var room = new Room();
		room.Type = type;
		Rooms.Add(room);

		int bail = 0;
		while (stack.Count > 0)
		{
			var current = stack.Pop();
			room.AddTile(current);
			if (current.Room != null)
			{
				current.Room.RemoveTile(current);
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

		var copy = new List<Room>();
		copy.AddRange(Rooms);
		foreach (var r in copy)
		{
			if (r.Tiles.Count == 0)
			{
				Rooms.Remove(r);
			}
		}
		Rooms = copy;
	}

	public void AddToRoom(Room room, Tile tile)
	{
		if (tile.Room != null)
			tile.Room.RemoveTile(tile);
		room.AddTile(tile);
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


		HashSet<Room> adjacentRooms = new HashSet<Room>();
		foreach (var adj in currentTile.AdjacentTiles(false))
		{
			if (adj != null && adj.IsRoomBorder)
				continue;
			if (adj != null && adj.Room != null)
			{
				if (adjacentRooms.Any(item => item.Type != adj.Room.Type))
					return false;
				adjacentRooms.Add(adj.Room);
			}
		}

		return true;
	}

	public void MergeRooms(Room a, Room b)
	{
		if (a == null || b == null)
			return;
		if (a.Type != b.Type)
		{
			Debug.LogError("Cannot merge rooms");
			return;
		}

		var mergetiles = new List<Tile>();
		mergetiles.AddRange(b.Tiles);
		foreach (var tile in mergetiles)
		{
			b.RemoveTile(tile);
			tile.Room = a;
			a.AddTile(tile);
		}

		Rooms.Remove(b);
	}

	public string Save()
	{
		string s = "";
		foreach (var room in Rooms)
		{
			if (room.Tiles.Count == 0)
				continue;
			s += room.Type + ",";
			foreach (var tile in room.Tiles)
			{
				s += tile.X + "," + tile.Y + ",";
			}
			s = s.Substring(0, s.Length - 1);
			s += ";";
		}
		s = s.Substring(0, s.Length - 1);
		return s;
	}

	public void Load(string save)
	{
		var grid = GetComponent<TileGrid>();
		var rooms = save.Split(';');
		foreach (var room in rooms)
		{
			Room r = new Room();
			var data = room.Split(',');
			r.Type = (RoomType)System.Enum.Parse(typeof(RoomType), data[0]);
			for (int i = 1; i < data.Length; i += 2)
			{
				int x = int.Parse(data[i]);
				int y = int.Parse(data[i + 1]);
				var tile = grid.grid[x, y];
				if (tile != null)
				{
					AddToRoom(r, tile);
				}
			}
			Rooms.Add(r);
		}
	}
}
