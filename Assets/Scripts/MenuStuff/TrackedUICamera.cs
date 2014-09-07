using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class TrackedUICamera : MonoBehaviour {

	void Start()
	{
		GlobalSettings.RegisterUI(camera);
	}

	void OnDestroy()
	{
		GlobalSettings.DeregisterUI(camera);
	}
}
