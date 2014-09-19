using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour 
{

	public enum TileType
	{
		Interior,
		Exterior,
		Empty
	}

	public enum TileRarity
	{
		Common = 4,
		Uncommon = 2,
		Rare = 1
	}

	public string DisplayName = "";
	public string DisplayPluralName = "";

	[TextArea]
	public string Description = "";

	public TileRarity Rarity = TileRarity.Common;
	public int BuyBlueprintCost = 0;

	public bool CanBeAttachedTo = false;
	public bool CanPlaceOnTopOfCharacter = true;

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
}
