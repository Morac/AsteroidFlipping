using UnityEngine;
using System.Collections;

public class SellAsteroid : MonoBehaviour
{

	void OnClick()
	{
		PlayerInventory.Funds += GameManager.AsteroidValue;
		Application.LoadLevel(GlobalSettings.Scene.BuyScreen);
	}
}
