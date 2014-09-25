using UnityEngine;
using System.Collections;

public class BuyAsteroid : MonoBehaviour
{
	public int BuyCost = 200;

	UILabel _label;
	UILabel label
	{
		get
		{
			if (_label == null)
				_label = GetComponentInChildren<UILabel>();
			return _label;
		}
	}


	void Update()
	{
		label.text = "Buy Asteriod\n" + GlobalSettings.Currency + BuyCost;
		if(!PlayerInventory.CanAfford(BuyCost))
		{
			GetComponent<UIButton>().isEnabled = false;
			label.color = Color.red;
		}
	}

	void OnClick()
	{
		PlayerInventory.Funds -= BuyCost;

		GameManager m = FindObjectOfType<GameManager>();
		m.Save();

		int seed = Random.Range(0, int.MaxValue);
		string name = "Asteroid" + ((int)(seed / 1e5)).ToString();
		GameManager.NewAsteroid(name, seed, 16);
	}
}
