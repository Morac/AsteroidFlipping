using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tile))]
public class Valuable : MonoBehaviour {

	public int value = 10;

	void Start()
	{
		GameManager.AsteroidValue += value;
	}

	void OnDestroy()
	{
		GameManager.AsteroidValue -= value;
	}
}
