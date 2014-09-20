using UnityEngine;
using System.Collections;

public class RotateOverTime : MonoBehaviour {

	public Vector3 Axis = Vector3.up;
	public float DegPerSec = 1;

	Vector3 startRotation;

	void Start()
	{
		startRotation = transform.rotation.eulerAngles;
	}

	void Update()
	{
		transform.rotation = Quaternion.Euler(startRotation + Axis * Time.time * DegPerSec);
	}
}
