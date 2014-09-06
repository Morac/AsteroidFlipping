using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

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
		if (GlobalSettings.Seed != 0)
		{
			Random.seed = GlobalSettings.Seed;
		}

		if (GlobalSettings.Size != 0)
		{
			tileGrid.AsteroidRadius = GlobalSettings.Size;
		}

		//loading called from here?
		tileGrid.Generate();
	}

}
