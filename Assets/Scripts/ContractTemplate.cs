using UnityEngine;
using System.Collections.Generic;

public class ContractTemplate : ScriptableObject
{
	public string Name;
	public int PayoutMin;
	public int PayoutMax;

	public List<RoomTemplate> Rooms = new List<RoomTemplate>()
	{
		new RoomTemplate()
		{
			Type = RoomManager.RoomType.LifeSupport,
			MinQuality = RoomManager.RoomQuality.Moderate,
			MaxQuality = RoomManager.RoomQuality.Exquisite,
			Min = 1,
			Max = 1
		}
	};

	[System.Serializable]
	public class RoomTemplate
	{
		public RoomManager.RoomType Type;
		public RoomManager.RoomQuality MinQuality;
		public RoomManager.RoomQuality MaxQuality;
		public int Min;
		public int Max;
	}

	public Contract Create()
	{
		Contract c = new Contract();
		c.Name = Name;
		c.Payout = Random.Range(PayoutMin, PayoutMax + 1);
		c.StartingAmount = c.Payout;

		foreach(var room in Rooms)
		{
			var type = room.Type;
			int qual_int = Random.Range((int)room.MinQuality, (int)room.MaxQuality + 1);
			var quality = (RoomManager.RoomQuality)qual_int;
			int num = Random.Range(room.Min, room.Max + 1);

			if (num != 0)
			{
				var require = new Contract.Requirement(type, quality, num);
				c.Requirements.Add(require);
			}
		}

		c.BidEndTime = TimeManager.Now + Random.Range(GlobalSettings.ContractTimeMin, GlobalSettings.ContractTimeMax) * GlobalSettings.ContractTimeIncr;

		return c;
	}




#if UNITY_EDITOR
	[UnityEditor.MenuItem("Assets/Create/Contract Template")]
	static void CreateTemplate()
	{
		ScriptableObjectUtility.CreateAsset<ContractTemplate>();
	}
#endif
	
}
