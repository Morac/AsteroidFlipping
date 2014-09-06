using UnityEngine;
using System.Collections;

public class MoveControl : MonoBehaviour {

	public float speed = 4;

	void Update()
	{
		rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
	}
}
