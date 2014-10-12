using UnityEngine;
using System.Collections.Generic;

public class AI_Bidding : MonoBehaviour
{
	const float TimeBetweenBids = 1;

	const float MinBidPercent = 0.5f;

	static List<string> CompanyNames = new List<string>()
	{
		"Ashiok Industrial",
		"Mjolnir Construction",
		"Olympus Excavations",
		"Ted's Housing",
		"Build It Rite",
	};

	void Start()
	{
		InvokeRepeating("Bid", 0, TimeBetweenBids);
	}

	void Bid()
	{
		string company = CompanyNames[Random.Range(0, CompanyNames.Count)];
		var contracts = ContractManager.Contracts;

		foreach(var contract in contracts)
		{
			int bidamt = GetBid(contract.StartingAmount);
			if (contract.Bid(company, bidamt))
				break;
		}
	}

	int GetBid(int startValue)
	{
		int min = (int)(startValue * MinBidPercent);
		int max = startValue;
		return Random.Range(min, max);
	}
}
