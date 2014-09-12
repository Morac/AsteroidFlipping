using UnityEngine;
using System.Collections.Generic;

public class TilePrefabList : ScriptableObject {

	const string TileListName = "TilePrefabList";

	public List<Tile> BasicTiles = new List<Tile>();
	public List<Tile> PurchasableTiles = new List<Tile>();
	public List<Tile> SystemTiles = new List<Tile>();

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

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Assets/Create/Tile Prefab List")]
	static void Create()
	{
		ScriptableObjectUtility.CreateAsset<TilePrefabList>();
	}
#endif
}
