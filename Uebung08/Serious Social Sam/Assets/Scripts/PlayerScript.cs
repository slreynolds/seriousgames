using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {


	private float turnDirection = 90f;
	private float movementSpeed = 0f;
	float maxSpeed = 0.25f;
	float speedIncrease = 0.01f;
	float turnAngle = 5f;
	float jumpPower = 10f;
	private bool isJumping = false;
	public bool isPlaying = false;

	// Update is called once per frame
	void Update () {
		if (isPlaying) {
			checkDeath ();
			checkInput ();
			move ();	
		}
	}

	public void setPlaying(bool b){
		isPlaying = b;
	}


	void checkDeath(){
		// If they player fall of the platform, reset his postion
		if (transform.position.y < -5f) {
			transform.position = new Vector3 (0f, 2f, 0f);
			this.GetComponent<Rigidbody> ().rotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
			this.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 3f, 0f);
			isJumping = false;
		}
	}

	// Abfrage der Steuerung
	private void checkInput()
	{
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			turnDirection += -turnAngle;
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			turnDirection += turnAngle;


		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			movementSpeed += speedIncrease;

			if (movementSpeed > maxSpeed)
				movementSpeed = maxSpeed;
		}else {
			if (movementSpeed > 0)
				movementSpeed -= 5 * speedIncrease;
			if (movementSpeed <= 0)
				movementSpeed = 0;
		}

		if (Input.GetKey(KeyCode.Space))
		{
			if (!isJumping)
			{
				isJumping = true;
				if(this.GetComponent<Rigidbody>().velocity.y < jumpPower)
					this.GetComponent<Rigidbody>().AddForce(0, jumpPower, 0, ForceMode.VelocityChange);
			}
		}
	}

	private void move()
	{
		transform.rotation = Quaternion.Euler (new Vector3 (0, turnDirection, 0f));
		transform.position += Quaternion.Euler (new Vector3 (0, -90f+turnDirection, 0f)) * new Vector3 (movementSpeed, 0f, 0f);
	}

	void OnCollisionEnter(Collision col)
	{
		isJumping = false;
	}
}
