using UnityEngine;
using System.Collections.Generic;

public static class GlobalSettings
{
	public static int Seed = 0;
	public static float Size = 0;

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

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Utils/Clear playerprefs")]
	static void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
#endif
}
