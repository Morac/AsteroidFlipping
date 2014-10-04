using UnityEngine;
using System.Collections;

public class DisableCamera : MonoBehaviour
{
	void OnEnable()
	{
		Player.Instance.Controller.enabled = false;
	}

	void OnDisable()
	{
		Player.Instance.Controller.enabled = true;
	}
}
