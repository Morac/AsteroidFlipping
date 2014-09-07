using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Tile))]
public class Valuable : MonoBehaviour {

	public int value = 10;

	void PlacedByPlayer()
	{
		GameManager.AsteroidValue += value;
	}

	void RemovedByPlayer()
	{
		GameManager.AsteroidValue -= value;
	}
}
