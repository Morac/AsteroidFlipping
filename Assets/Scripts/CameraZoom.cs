using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{

	public float min = -5f;
	public float max = 10f;
	public float speed = 3;

	void Update()
	{
		transform.position -= Vector3.up * Input.GetAxis("Mouse ScrollWheel") * speed;
		if (transform.position.y < min)
		{
			transform.position = new Vector3(transform.position.x, min, transform.position.z);
		}
		if (transform.position.y > max)
		{
			transform.position = new Vector3(transform.position.x, max, transform.position.z);
		}
	}
}
