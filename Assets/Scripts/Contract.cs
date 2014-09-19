using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Contract
{
	public enum ContractType
	{
		Housing,
		Industrial,
		LuxuryHousing,
		ApartmentHousing,
		Farming,
		Storage
	}
	
	public enum ContractSize
	{
		Small = 1,
		Medium = 4,
		Large = 16
	}

	public ContractType Type { get; private set; }
	public ContractSize Size { get; private set; }
	public int Payout { get; private set; }

	public List<Requirement> Requirements = new List<Requirement>();

	Contract() { }

	public bool Evaluate(Tile[,] tiles)
	{
		foreach(var requirement in Requirements)
		{
			if(!requirement.Pass(tiles))
				return false;
		}
		return true;
	}

	public static Contract GenerateRandomContract(ContractSize size, ContractType type)
	{
		Contract contract = new Contract();
		contract.Type = type;
		contract.Size = size;
		contract.Payout = GlobalSettings.BaseContractPayout * (int)size + Random.Range(-GlobalSettings.ContractVariation, GlobalSettings.ContractVariation);

		for (int i = 0; i < NumRequirements(size); i++)
		{
			var requirement = Requirement.GetRandomRequirement(size, type, contract.Requirements);
			if (requirement != null)
				contract.Requirements.Add(requirement);
		}

		return contract;
	}

	public override string ToString()
	{
		string s = Type + " Contract (" + Size + ")\n";
		foreach(var r in Requirements)
		{
			s += r.ToString() + "\n";
		}
		s += "Payout: " + Payout;
		return s;
	}

	static int NumRequirements(ContractSize size)
	{
		switch (size)
		{
			default:
			case ContractSize.Small:
				return Random.Range(1, 4);
			case ContractSize.Medium:
				return Random.Range(3, 6);
			case ContractSize.Large:
				return Random.Range(5, 8);
		}
	}

	public abstract class Requirement
	{
		public abstract float Chance();
		public abstract bool Pass(Tile[,] grid);
		public abstract Requirement Create(ContractSize size, ContractType type, List<Requirement> existing);

		public static List<Requirement> Subclasses = new List<Requirement>()
		{
			new TileRequirement(),
			new TileExclusionRequirement(),
			new RoomRequirement()
		};

		public static Requirement GetRandomRequirement(ContractSize size, ContractType t, List<Requirement> existingRequirements)
		{
			float sum = 0;
			foreach(var type in Subclasses)
			{
				sum += type.Chance();
			}
			float r = Random.Range(0, sum);
			foreach(var type in Subclasses)
			{
				r -= type.Chance();
				if (r <= 0)
					return type.Create(size, t, existingRequirements);
			}
			return null;
		}
	}

	public class TileRequirement : Requirement
	{

		public int count;
		public Tile tile;

		public override float Chance()
		{
			return 5f / 6f;
		}

		public override Requirement Create(ContractSize size, ContractType type, List<Requirement> existingRequirements)
		{
			var r = new TileRequirement();
			var possibletiles = TilePrefabList.Instance.GetTilesWithTag(type);
			foreach (var existing in existingRequirements)
			{
				if(existing is TileRequirement)
				{
					var texist = existing as TileRequirement;
					possibletiles.Remove(texist.tile);
				}
			}
			if (possibletiles.Count > 0)
				r.tile = possibletiles[Random.Range(0, possibletiles.Count - 1)];
			else
				return null;

			r.count = Random.Range(1, (int)size * (int)r.tile.Rarity);

			return r;
		}

		public override bool Pass(Tile[,] grid)
		{
			int c = 0;
			foreach(var t in grid)
			{
				if(t.name == tile.name)
				{
					c++;
				}
			}
			return c >= count;
		}

		public override string ToString()
		{
			return count + "x " + tile.GetDisplayPluralName();
		}
	}

	public class TileExclusionRequirement : Requirement
	{
		public Tile tile;

		public override float Chance()
		{
			return 1f / 6f;
		}

		public override Requirement Create(ContractSize size, ContractType type, List<Requirement> existingRequirements)
		{
			TileExclusionRequirement r = new TileExclusionRequirement();
			var possibletiles = TilePrefabList.Instance.GetTilesWithoutTag(type);

			foreach(var exist in existingRequirements)
			{
				if(exist is TileExclusionRequirement)
				{
					var texist = exist as TileExclusionRequirement;
					possibletiles.Remove(texist.tile);
				}
			}

			if (possibletiles.Count > 0)
				r.tile = possibletiles[Random.Range(0, possibletiles.Count - 1)];
			else
				return null;
			return r;
		}

		public override bool Pass(Tile[,] grid)
		{
			foreach(var t in grid)
			{
				if (t.name == tile.name)
					return false;
			}
			return true;
		}

		public override string ToString()
		{
			return "No " + tile.GetDisplayPluralName();
		}
	}

	public class RoomRequirement : Requirement
	{
		public int count;

		public override float Chance()
		{
			return 0;
		}

		public override Requirement Create(ContractSize size, ContractType type, List<Requirement> existingRequirements)
		{
			throw new System.NotImplementedException();
		}

		public override bool Pass(Tile[,] grid)
		{
			return false;
		}

		public override string ToString()
		{
			return count > 1 ? count + " Rooms" : count + " Room";
		}
	}


	[UnityEditor.MenuItem("Utils/Generate Contract test")]
	static void test()
	{
		Contract c = GenerateRandomContract(ContractSize.Large, ContractType.Housing);
		Debug.Log(c.ToString());
	}

}
