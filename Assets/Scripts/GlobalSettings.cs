using UnityEngine;
using System.Collections.Generic;

public static class GlobalSettings
{
	//public static int Seed = 0;
	//public static float Size = 0;

	public const char Currency = '$';

	public static class Scene
	{
		public const string BuyScreen = "buyscreen";
		public const string MainLevel = "main";
	}

	static HashSet<Camera> uiCameras = new HashSet<Camera>();
	public static void RegisterUI(Camera c)
	{
		uiCameras.Add(c);
	}
	public static void DeregisterUI(Camera c)
	{
		uiCameras.Remove(c);
	}

	public static bool HitUI()
	{
		foreach(var ui in uiCameras)
		{
			var ray = ui.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, 10000, LayerMask.GetMask("UI")))
				return true;
		}
		return false;
	}

	public static int SaveSlot = 0;
	static bool DirCreated = false;
	public static string SavePath
	{
		get
		{
			string path = Application.dataPath + "/Saves/Slot" + SaveSlot + "/";
			if (!DirCreated)
			{
				System.IO.Directory.CreateDirectory(path);
				DirCreated = true;
			}
			return path;
		}
	}

	public const string ContractsSaveFileName = "Contracts.txt";

	public static bool IsReservedName(string file)
	{
		bool r = false;
		r |= System.IO.Path.GetFileName(file) == ContractsSaveFileName;
		return r;
	}

	#region EconomyVariables
	public const int BaseContractPayout = 10;
	public const int ContractVariation = 2;
	public const float ChanceOfRequirementExclusion = 1f / 6f;

	#endregion

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Utils/Clear playerprefs")]
	static void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	[UnityEditor.MenuItem("Utils/Add cash")]
	static void AddCash()
	{
		PlayerInventory.Funds += 1000000;
	}
#endif
}
