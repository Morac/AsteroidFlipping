using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public enum LevelStartAction
	{
		Generate,
		Load,
		DefaultGenerate,
		DefaultLoad
	}

	public static LevelStartAction OnLevelStart = LevelStartAction.DefaultLoad;
	//public static LevelStartAction OnLevelStart = LevelStartAction.DefaultGenerate;
	static string _asteroidName = "";
	static int _seed = 0;
	static float _size = 0;

	public string AsteroidName = "";
	public TileGrid tileGrid;

	public delegate void AsteroidValueChanged(int newvalue);
	public static AsteroidValueChanged AsteroidValueCallback;
	static int _value = 0;
	public static int AsteroidValue
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			if (AsteroidValueCallback != null)
				AsteroidValueCallback(value);
		}
	}


	void Start()
	{
		tileGrid.Setup();
		
		AsteroidName = _asteroidName;

		switch (OnLevelStart)
		{
			case LevelStartAction.Generate:
				Random.seed = _seed;
				tileGrid.AsteroidRadius = _size;
				tileGrid.Generate();
				break;
			case LevelStartAction.Load:
				Load(_asteroidName);
				break;
			default:
			case LevelStartAction.DefaultGenerate:
				AsteroidName = "Asteroid" + Random.seed;
				tileGrid.Generate();
				break;
			case LevelStartAction.DefaultLoad:
				Load(CurrentAsteroids().First());
				break;
		}
		
		//if (GlobalSettings.Seed != 0)
		//{
		//	Random.seed = GlobalSettings.Seed;
		//}

		//if (GlobalSettings.Size != 0)
		//{
		//	tileGrid.AsteroidRadius = GlobalSettings.Size;
		//}

		//tileGrid.Setup();

		////loading called from here?
		////tileGrid.Generate();
		//Load(CurrentAsteroids().First());
	}

	[ContextMenu("Save")]
	public void Save()
	{
		System.IO.Directory.CreateDirectory(GlobalSettings.SavePath);
		System.IO.StreamWriter writer = new System.IO.StreamWriter(GlobalSettings.SavePath + AsteroidName + ".txt");

		writer.WriteLine(Random.seed);
		var pos = tileGrid.player.transform.position;
		writer.WriteLine(pos.x + "," + pos.y + "," + pos.z);
		writer.WriteLine(tileGrid.Save());

		writer.Close();
	}

	public void Load(string asteroidname)
	{
		AsteroidName = asteroidname;
		System.IO.StreamReader reader = new System.IO.StreamReader(GlobalSettings.SavePath + asteroidname + ".txt");

		Random.seed = int.Parse(reader.ReadLine());

		var pos = reader.ReadLine().Split(',');
		float x = float.Parse(pos[0]);
		float y = float.Parse(pos[1]);
		float z = float.Parse(pos[2]);
		tileGrid.player.transform.position = new Vector3(x, y, z);

		tileGrid.Load(reader.ReadLine());

		reader.Close();
	}

	public static List<string> CurrentAsteroids()
	{
		var files = System.IO.Directory.GetFiles(GlobalSettings.SavePath, "Asteroid*.txt");
		List<string> r = new List<string>();
		foreach(var file in files)
		{
			string mod = System.IO.Path.GetFileNameWithoutExtension(file);
			r.Add(mod);
		}
		return r;
	}

	public static void LoadAsteroid(string asteroidname)
	{
		_asteroidName = asteroidname;
		OnLevelStart = LevelStartAction.Load;
		Application.LoadLevel(GlobalSettings.Scene.MainLevel);
	}

	public static void NewAsteroid(string asteroidname, int seed, float size)
	{
		_asteroidName = asteroidname;
		_seed = seed;
		_size = size;
		OnLevelStart = LevelStartAction.Generate;
		Application.LoadLevel(GlobalSettings.Scene.MainLevel);
	}
}
