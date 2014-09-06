using UnityEngine;
using System.Collections;

public class AsteroidListItem : MonoBehaviour
{

	public UILabel NameLabel;
	public UILabel CostLabel;

	const int MinAsteroidSize = 5;
	const int MaxAsteroidSize = 40;

	float seed;
	float size;
	int cost;

	public void SetupFree()
	{
		seed = Random.Range(0, int.MaxValue);
		size = MinAsteroidSize;
		cost = 0;
	}

	public void Setup()
	{
		seed = Random.Range(0, int.MaxValue);
		size = Random.Range(MinAsteroidSize, MaxAsteroidSize);
		cost = Cost(size);
	}



	public void BuyAsteroid()
	{
		if (PlayerInventory.CanAfford(cost))
		{
			PlayerInventory.Funds -= cost;
			Application.LoadLevel(1); //todo: fix
		}
	}

	public static int Cost(float asteroidSize)
	{
		return (int)asteroidSize * 200 + Random.Range(0, 1000);
	}
}
