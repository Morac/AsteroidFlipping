using UnityEngine;
using System.Collections;

public class TimeManager : Singleton<TimeManager>
{
	public int GameTime;
	public static int Now { get { return Instance.GameTime; } }

	public int OffsetFromUnityTime { get; private set; }

	void Update()
	{
		GameTime = (int)Time.time + OffsetFromUnityTime;
	}

	public string Save()
	{
		return Now.ToString();
	}

	public void Load(string s)
	{
		int t = int.Parse(s);
		OffsetFromUnityTime = t - (int)Time.time;
	}
}
