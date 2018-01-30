using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour {
	
	float sensitivity = 3;
	float rotationY = 0F;

	void Start () {

	}

	// Update is called once per frame
	void Update () {
		float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivity;


		rotationY += Input.GetAxis ("Mouse Y") * sensitivity;
		rotationY = Mathf.Clamp (rotationY, -80F, 80F);

		transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
	}
}
