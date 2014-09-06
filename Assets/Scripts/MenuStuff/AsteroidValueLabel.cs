using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class AsteroidValueLabel : MonoBehaviour
{
	UILabel label;

	void OnEnable()
	{
		label = GetComponent<UILabel>();
		SetValue(GameManager.AsteroidValue);
		GameManager.AsteroidValueCallback += SetValue;
	}

	void OnDisable()
	{
		GameManager.AsteroidValueCallback -= SetValue;
	}

	void SetValue(int value)
	{
		label.text = GlobalSettings.Currency + value.ToString();
	}
}
