using UnityEngine;
using System.Collections;

public static class PlayerInventory 
{

	const string FundsKey = "Player.Funds";
	static int _funds = -1;
	public static int Funds
	{
		get
		{
			if(_funds == -1)
			{
				_funds = PlayerPrefs.GetInt(FundsKey, 0);
			}
			return _funds;
		}
		set
		{
			_funds = value;
			PlayerPrefs.SetInt(FundsKey, value);
		}
	}

	public static bool CanAfford(int val)
	{
		return Funds - val > 0;
	}
}
