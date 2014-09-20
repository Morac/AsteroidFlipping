using UnityEngine;
using System.Collections;

public class AsteroidListItem : MonoBehaviour
{

	public UILabel NameLabel;
	public UILabel CostLabel;
	public UILabel SizeLabel;
	public UIButton BuyBtn;

	const int FreeAsteroidSize = 8;
	const int MinAsteroidSize = 10;
	const int MaxAsteroidSize = 40;

	public int seed;
	public float size;
	public int cost;

	public void SetupFree()
	{
		seed = Random.Range(0, int.MaxValue);
		size = FreeAsteroidSize;
		cost = 0;
		UpdateLabels();
	}

	public void Setup()
	{
		seed = Random.Range(0, int.MaxValue);
		size = Random.Range(MinAsteroidSize, MaxAsteroidSize);
		cost = Cost(size);
		UpdateLabels();
	}

	public void UpdateLabels()
	{
		NameLabel.text = "Asteroid " + (int)(seed / 1e5);
		SizeLabel.text = size.ToString();
		CostLabel.text = GlobalSettings.Currency + cost.ToString();
		if(!PlayerInventory.CanAfford(cost))
		{
			BuyBtn.isEnabled = false;
			CostLabel.color = Color.red;
		}
		else
		{
			CostLabel.color = Color.green;
		}
	}

	public void BuyAsteroid()
	{
		if (PlayerInventory.CanAfford(cost))
		{
			PlayerInventory.Funds -= cost;
			//GlobalSettings.Seed = seed;
			//GlobalSettings.Size = size;
			GameManager.AsteroidValue = cost / 10;
			GameManager.NewAsteroid("Asteroid" + seed, seed, size);

			Application.LoadLevel(GlobalSettings.Scene.MainLevel);
		}
	}

	public static int Cost(float asteroidSize)
	{
		return (int)asteroidSize * 100 + Random.Range(0, 1000);
	}
}
