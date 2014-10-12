using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class AsteroidNameLabel : MonoBehaviour
{

	void Start()
	{
		var label = GetComponent<UILabel>();
		if (label != null)
			label.text = GameManager.Instance.AsteroidName;
	}
}
