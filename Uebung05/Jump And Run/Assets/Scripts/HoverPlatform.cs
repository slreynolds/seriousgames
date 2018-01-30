using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPlatform : MonoBehaviour {

	float hoverForce = 10f;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PowerUP") {
			other.GetComponent<Rigidbody> ().AddForce (Vector3.up * hoverForce, ForceMode.VelocityChange);
		}
	}
}
