using UnityEngine;
using System.Collections.Generic;

public class ContractManager : Singleton<ContractManager>
{
	public delegate void ContractCreationCallback(Contract c);
	public static ContractCreationCallback OnContractCreated;
	const int MaxContracts = 6;

	static List<Contract> _contracts;
	public static List<Contract> Contracts
	{
		get
		{
			if(_contracts == null)
			{
				LoadContracts();
			}
			return _contracts;
		}
	}

	static List<ContractTemplate> _templates;
	public static List<ContractTemplate> Templates
	{
		get
		{
			if(_templates == null)
			{
				//load
			}
			return _templates;
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

	public static void LoadContracts()
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


	void Update()
	{
		if(Contracts.Count < MaxContracts)
		{
			GenerateContract();
		}
	}

	void GenerateContract()
	{
		var template = Templates[Random.Range(0, Templates.Count)];

		var contract = template.Create();

		_contracts.Add(contract);
		if (OnContractCreated != null)
			OnContractCreated(contract);
	}


#if UNITY_EDITOR
	[UnityEditor.MenuItem("Utils/Test contract manager")]
	static void test()
	{
		for (int i = 0; i < 6; i++)
		{
			Instance.GenerateContract();
		}
		SaveContracts();
		LoadContracts();
		foreach (var contract in Contracts)
			Debug.Log(contract.ToString());
		Debug.Log("test done");
	}
#endif
}
