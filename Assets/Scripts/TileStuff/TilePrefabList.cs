using UnityEngine;
using System.Collections.Generic;

public class TilePrefabList : ScriptableObject {

	const string TileListName = "TilePrefabList";

	public List<Tile> BasicTiles = new List<Tile>();
	public List<Tile> PurchasableTiles = new List<Tile>();
	public List<Tile> SystemTiles = new List<Tile>();

	public List<Floor> FloorPrefabs = new List<Floor>();

	//todo: set up a loading scene that can download and set this somehow
	static TilePrefabList _instance;
	public static TilePrefabList Instance
	{
		get
		{
			if (_instance == null)
				_instance = Resources.Load(TileListName) as TilePrefabList;
			return _instance;
		}
	}

	public List<Tile> GetAllTiles()
	{
		List<Tile> ret = new List<Tile>();
		ret.AddRange(BasicTiles);
		ret.AddRange(PurchasableTiles);
		ret.AddRange(SystemTiles);
		return ret;
	}

	public IList<Floor> GetAllFloorsPrefabsForType(RoomManager.RoomType type)
	{
		List<Floor> ret = new List<Floor>();
		foreach(var f in FloorPrefabs)
		{
			if (f.Type == type)
				ret.Add(f);
		}
		return ret;
	}


#if UNITY_EDITOR
	[UnityEditor.MenuItem("Assets/Create/Tile Prefab List")]
	static void Create()
	{
		ScriptableObjectUtility.CreateAsset<TilePrefabList>();
	}
#endif
}
