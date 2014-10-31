using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Tile : MonoBehaviour 
{

	public enum TileType
	{
		Interior = 'i',
		Exterior = 'e',
		Empty = 'n'
	}

	public enum TileRarity
	{
		Common = 4,
		Uncommon = 2,
		Rare = 1
	}

	public string DisplayName = "";
	public string DisplayPluralName = "";
	public string SaveCode = "t";

	[TextArea]
	public string Description = "";

	public TileRarity Rarity = TileRarity.Common;
	public int BuyBlueprintCost = 0;

	public bool CanBeAttachedTo = false;
	public bool CanPlaceOnTopOfCharacter = true;
	public bool IsRoomBorder = true;

	public float ShakeAmount = 0.05f;
	public float ShakeDuration = 0.2f;

	public List<Contract.ContractType> ContractTags = new List<Contract.ContractType>();

	[HideInInspector]
	public int X;
	[HideInInspector]
	public int Y;

	[HideInInspector]
	public TileType type;
	[HideInInspector]
	public TileGrid tilegrid;

	public RoomManager.Room Room = null;

	public GameObject DefaultFloor;
	
	[HideInInspector]
	public Floor CurrentFloor;

	[HideInInspector]
	[SerializeField]
	public int RoomTypes = 0xFFFFFF;

	public string GetDisplayName()
	{
		return DisplayName != "" ? DisplayName : name;
	}

	public string GetDisplayPluralName()
	{
		return DisplayPluralName != "" ? DisplayPluralName : GetDisplayName() + "s";
	}

	public Tile SetTile(params Tile[] tilelist)
	{
		return tilegrid.SetTile(tilelist, X, Y, type);
	}

	public List<Tile> AdjacentTiles(bool diagonals)
	{
		List<Tile> list = new List<Tile>();
		if (tilegrid == null)
			return list;
		foreach(var adj in tilegrid.Adjacents(new Vector2(X,Y), diagonals))
		{
			list.Add(tilegrid.grid[(int)adj.x, (int)adj.y]);
		}
		return list;
	}

	public bool CanAfford()
	{
		bool afford = true;
		foreach(var cost in GetComponents<CostsItem>())
		{
			afford &= PlayerInventory.CanAfford(cost.item, cost.count);
		}
		return afford;
	}

	public string CostString()
	{
		string cost_str = "";
		foreach(var cost in GetComponents<CostsItem>())
		{
			string s = cost.item + ": " + cost.count;
			if(!PlayerInventory.CanAfford(cost.item, cost.count))
			{
				s = "[FF0000]" + s + "[-]";
			}
			cost_str += s + " ";
		}
		return cost_str;

	}

	void PlacedByPlayer()
	{
		Camera.main.Shake(ShakeAmount, ShakeDuration);
	}

	public delegate void SaveCallback();
	public delegate void LoadCallback(Dictionary<string, string> args);

	public SaveCallback OnSave;
	public LoadCallback OnLoad;

	public Dictionary<string, string> SaveOutput = new Dictionary<string, string>();

	public string Save()
	{
		if(OnSave != null)
			OnSave();

		string r = "";
		foreach(var pair in SaveOutput)
		{
			r += pair.Key + ":" + pair.Value + ",";
		}
		if(r != "")
			r = r.Substring(0, r.Length - 1);
		return r;
	}

	public void Load(string[] saveargs)
	{
		Dictionary<string, string> args = new Dictionary<string, string>();
		
		foreach(var s in saveargs)
		{
			var kvp = s.Split(':');
			if(kvp.Length == 2)
				args[kvp[0]] = args[kvp[1]];
		}
		if(OnLoad != null)
			OnLoad(args);
	}
}
