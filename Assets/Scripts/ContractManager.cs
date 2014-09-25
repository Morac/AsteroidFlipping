using UnityEngine;
using System.Collections.Generic;

public static class ContractManager
{
	static List<Contract> _contracts;
	public static List<Contract> Contracts
	{
		get
		{
			if(_contracts == null)
			{
				LoadContracts();
			}
			if(_contracts == null)
			{
				GenerateContracts();
			}
			return _contracts;
		}
	}

	public static void SaveContracts()
	{
		var file = GlobalSettings.SavePath + GlobalSettings.ContractsSaveFileName;
		System.IO.StreamWriter writer = new System.IO.StreamWriter(file);

		foreach (var contract in _contracts)
		{
			writer.WriteLine(contract.Save());
		}

		writer.Close();
	}

	static void LoadContracts()
	{
		string path = GlobalSettings.SavePath + GlobalSettings.ContractsSaveFileName;
		if (System.IO.File.Exists(path) == false)
			return;

		_contracts = new List<Contract>();
		System.IO.StreamReader reader = new System.IO.StreamReader(path);

		while(reader.EndOfStream == false)
		{
			Contract c = Contract.Load(reader.ReadLine());
			_contracts.Add(c);
		}

		reader.Close();
	}

	static void GenerateContracts()
	{
		_contracts = new List<Contract>();
		_contracts.Add(Contract.GenerateRandomContract(Contract.ContractSize.Small, Contract.ContractType.Housing));
		_contracts.Add(Contract.GenerateRandomContract(Contract.ContractSize.Small, Contract.ContractType.Housing));
		_contracts.Add(Contract.GenerateRandomContract(Contract.ContractSize.Medium, Contract.ContractType.Housing));
		_contracts.Add(Contract.GenerateRandomContract(Contract.ContractSize.Medium, Contract.ContractType.Housing));
		_contracts.Add(Contract.GenerateRandomContract(Contract.ContractSize.Large, Contract.ContractType.Housing));
		_contracts.Add(Contract.GenerateRandomContract(Contract.ContractSize.Large, Contract.ContractType.Housing));
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Utils/Test contract manager")]
	static void test()
	{
		GenerateContracts();
		SaveContracts();
		LoadContracts();
		foreach (var contract in Contracts)
			Debug.Log(contract.ToString());
		Debug.Log("test done");
	}
#endif
}
