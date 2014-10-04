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
	public int Payout = 0;
	public int BidEndTime = 0;
	public string LowBidder = "";
	public int ReservedBid = 0;

	public bool BiddingEnded
	{
		get
		{
			return BidEndTime < TimeManager.Now;
		}
	}

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

	public bool Bid(string bidder, int amount)
	{

		bool bidsuccessful = false;

		if(amount <= 0)
		{
			bidsuccessful = false;
		}
		else if (LowBidder == bidder && amount < ReservedBid)
		{
			ReservedBid = amount;
			bidsuccessful = true;
		}
		else if (ReservedBid > 0)
		{
			if (amount < ReservedBid)
			{
				bidsuccessful = true;
				Payout = ReservedBid--;
				ReservedBid = amount;
				LowBidder = bidder;
			}
			else
			{
				Payout = amount;
				bidsuccessful = false;
			}
		}
		else if (amount < Payout)
		{
			Payout--;
			ReservedBid = amount;
			LowBidder = bidder;
			bidsuccessful = true;
		}


		return bidsuccessful;
	}

	public static Contract GenerateRandomContract()
	{
		var sizear = System.Enum.GetValues(typeof(ContractSize));
		var size = (ContractSize)sizear.GetValue(Random.Range(0, sizear.Length));

		var typear = System.Enum.GetValues(typeof(ContractType));
		var type = (ContractType)typear.GetValue(Random.Range(0, typear.Length));

		return GenerateRandomContract(size, type);
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

		contract.BidEndTime = TimeManager.Now + 5 * 60;

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

	#region requirements
	public abstract class Requirement
	{
		public abstract float Chance();
		public abstract bool Pass(Tile[,] grid);
		public abstract Requirement Create(ContractSize size, ContractType type, List<Requirement> existing);
		public abstract string SaveCode();

		public abstract string Save();
		protected abstract void LoadData(string[] s);

		public static Requirement Load(string s)
		{
			string[] split = s.Split('.');
			
			foreach(var type in Subclasses)
			{
				if(split[0] == type.SaveCode())
				{
					Requirement r = (Requirement)System.Activator.CreateInstance(type.GetType());
					r.LoadData(split);
					return r;
				}
			}
			return null;
		}

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

		public override string SaveCode()
		{
			return "tr";
		}

		public override string Save()
		{
			return SaveCode() + "." + count + "." + tile.SaveCode;
		}

		protected override void LoadData(string[] s)
		{
			count = int.Parse(s[1]);
			tile = TilePrefabList.Instance.GetAllTiles().Find(item => item.SaveCode == s[2]);
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

		public override string SaveCode()
		{
			return "ter";
		}

		public override string Save()
		{
			return SaveCode() + "." + tile.SaveCode;
		}

		protected override void LoadData(string[] s)
		{
			tile = TilePrefabList.Instance.GetAllTiles().Find(item => item.SaveCode == s[1]);
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

		public override string SaveCode()
		{
			throw new System.NotImplementedException();
		}

		public override string Save()
		{
			throw new System.NotImplementedException();
		}

		protected override void LoadData(string[] s)
		{
			throw new System.NotImplementedException();
		}
	}

	#endregion


	public string Save()
	{
		string s = "";
		s += Size + "," + Type + "," + Payout + "," + BidEndTime + "," + LowBidder + "," + ReservedBid;
		foreach(var r in Requirements)
		{
			s += "," + r.Save();
		}
		return s;
	}

	public static Contract Load(string s)
	{
		string[] split = s.Split(',');
		Contract c = new Contract();
		c.Size = (ContractSize)System.Enum.Parse(typeof(ContractSize), split[0]);
		c.Type = (ContractType)System.Enum.Parse(typeof(ContractType), split[1]);
		c.Payout = int.Parse(split[2]);
		c.BidEndTime = int.Parse(split[3]);
		c.LowBidder = split[4];
		c.ReservedBid = int.Parse(split[5]);
		
		for (int i = 6; i < split.Length; i++)
		{
			c.Requirements.Add(Requirement.Load(split[i]));
		}

		return c;
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Utils/Generate Contract test")]
	static void test()
	{
		Contract c = GenerateRandomContract(ContractSize.Large, ContractType.Housing);
		Debug.Log(c.ToString());
		string s = c.Save();
		Debug.Log(s);

		Contract c2 = Load(s);
		Debug.Log(c2.ToString());
	}
#endif

}
