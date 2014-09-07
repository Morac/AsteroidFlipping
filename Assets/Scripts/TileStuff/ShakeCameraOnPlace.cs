using UnityEngine;
using System.Collections;

public class ShakeCameraOnPlace : MonoBehaviour
{
	public float amount = 0.05f;
	public float duration = 0.2f;

	void PlacedByPlayer()
	{
		Camera.main.Shake(amount, duration);
	}
}
