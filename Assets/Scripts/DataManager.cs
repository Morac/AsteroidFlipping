using UnityEngine;
using System.Collections;

public class DataManager : Singleton<DataManager>
{

	protected override void Awake()
	{
		base.Awake();
		ContractManager.LoadContracts();
		GlobalSettings.LoadData();
	}

	void OnDestroy()
	{
		ContractManager.SaveContracts();
		GlobalSettings.SaveData();
	}
}
