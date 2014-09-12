using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class PlayerInventory
{
	#region StringConsts
	const string FundsKey = "Player.Funds";
	static string ItemKey(Item m) { return "Player." + m.ToString(); }
	static string TileKey(Tile t) { return "Tile." + t.name; }
	#endregion

	public enum Item
	{
		Metals,
		Organics,
		Seeds,
		Electronics
	}

	#region Events
	public delegate void FundsChanged(int newval);
	public static FundsChanged FundsChangedCallback;

	public delegate void ItemChanged(Item m, int newval);
	public static ItemChanged ItemChangedCallback;
	#endregion

	static int _funds = -1;
	public static int Funds
	{
		get
		{
			if (_funds == -1)
			{
				_funds = PlayerPrefs.GetInt(FundsKey, 0);
			}
			return _funds;
		}
		set
		{
			_funds = value;
			PlayerPrefs.SetInt(FundsKey, value);
			if (FundsChangedCallback != null)
			{
				FundsChangedCallback(value);
			}
		}
	}

	public static bool CanAfford(int val)
	{
		return Funds >= val;
	}

	public static bool CanAfford(Item m, int val)
	{
		return inventory[m] >= val;
	}


	public static Inventory inventory = new Inventory();

	public class Inventory
	{
		Dictionary<Item, int> counts = new Dictionary<Item, int>();

		public Inventory()
		{
			foreach (Item m in System.Enum.GetValues(typeof(Item)))
			{
				counts[m] = PlayerPrefs.GetInt(ItemKey(m), 0);
			}
		}

		public int this[Item m]
		{
			get
			{
				return counts[m];
			}
			set
			{
				counts[m] = value;
				PlayerPrefs.SetInt(ItemKey(m), value);
				if (ItemChangedCallback != null)
					ItemChangedCallback(m, value);

			}
		}

		public override string ToString()
		{
			string s = " ";
			foreach (var m in counts)
			{
				s += m.Key + ": " + m.Value + " ";
			}
			return s;
		}
	}


	static Tile[] cachedTileList = null;

	static void CacheTileList()
	{
		var prefablist = TilePrefabList.Instance;
		var returnlist = new List<Tile>();
		returnlist.AddRange(prefablist.BasicTiles);

		foreach (var item in prefablist.PurchasableTiles)
		{
			if (IsUnlocked(item))
				returnlist.Add(item);
		}
		cachedTileList = returnlist.ToArray();
	}


	public static Tile[] GetUsableTiles()
	{
		if (cachedTileList == null)
			CacheTileList();
		return cachedTileList;
	}

	public static void Unlock(Tile tile)
	{
		PlayerPrefs.SetInt(TileKey(tile), 1);
		CacheTileList();
	}

	public static bool IsUnlocked(Tile tile)
	{
		return PlayerPrefs.GetInt(TileKey(tile), 0) == 1;
	}


}
