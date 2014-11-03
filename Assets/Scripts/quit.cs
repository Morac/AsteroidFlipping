using UnityEngine;
using System.Collections;

public class quit : MonoBehaviour {

	void Start()
	{
		if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
			gameObject.SetActive(false);
	}

	void OnClick()
	{
		Application.Quit();
	}
}
