using UnityEngine;
using System.Collections;


public class RunnerControl : MonoBehaviour {

	public bool running = false;
	public Vector3 direction = new Vector3(0, 0, 1);
	float speed = 100;
	float maxSpeed = 6;

	float maxJumpingHeight = 8;

	public float maxJumpTime = 20;
	float airTime;

	float rotation;
	Rigidbody rigidbody;
	public GameObject text;

	public GameObject rleft, rright, rmid, rtrigger, rdis;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (running) {

			transform.rotation = Quaternion.AngleAxis (rotation, Vector3.up);

			checkInput ();

			processOrientation ();

			freezePosition ();

			if (rigidbody.velocity.magnitude < maxSpeed) {
				rigidbody.AddForce (direction * speed);
			}

			if (transform.position.y == maxJumpingHeight && airTime > 0) {
				airTime -= Time.deltaTime;
			}

			if (transform.position.y == maxJumpingHeight && airTime < 0) {
				transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
			}


			checkDeath ();
		} else {
			if (InputSimulation.touchCount > 0) {
				// reset most stuff
				die ();

				running = true;
				text.SetActive (false);
				Debug.Log ("Started Game");
			}
		}
	}

	bool swipedLeft = false;
	bool swipedRight = false;
	bool swipedUp = false;
	bool consumed = false;
	float swipeIntensity = 10f;

	/**
	 * verarbeitet die Touch-Eingaben
	 */
	void processTouches(){
		swipedLeft = false;
		swipedRight = false;
		swipedUp = false;

		if (InputSimulation.touchCount > 0) {
			if (!consumed) {
				if (InputSimulation.touches [0].deltaPosition.x > swipeIntensity) {
					//right swipe
					swipedRight = true;
					consumed = true;
				}
				if (InputSimulation.touches [0].deltaPosition.x < -swipeIntensity) {
					// left swipe
					swipedLeft = true;
					consumed = true;
				}
				if (InputSimulation.touches [0].deltaPosition.y > 2*swipeIntensity) {
					// up swipe
					swipedUp = true;
					consumed = true;
				}
			}
		} else {
			consumed = false;
		}

	}

	const int LEFT = -1, MIDDLE = 0, RIGHT = +1;
	int currentPos = MIDDLE;
	float rotateItensity = 0.3f;

	/**
	 * checks the orientation of the smartphone and moves the runner according to the actual orientation 
	 */
	void processOrientation() {
		if (InputSimulation.acceleration.x > rotateItensity) {
			if (currentPos != LEFT) {
				// move left
				transform.position = (Quaternion.AngleAxis (90, Vector3.up) * direction) + transform.position;
			}
			currentPos = LEFT;
		} else if (InputSimulation.acceleration.x < -rotateItensity) {
			// move right
			if (currentPos != RIGHT) {
				transform.position = (Quaternion.AngleAxis (-90, Vector3.up) * direction) + transform.position;	
			}
			currentPos = RIGHT;
		} else {
			if (currentPos == RIGHT) {
				transform.position = (Quaternion.AngleAxis (90, Vector3.up) * direction) + transform.position;	
			}
			if (currentPos == LEFT) {
				transform.position = (Quaternion.AngleAxis (-90, Vector3.up) * direction) + transform.position;	
			}
			currentPos = MIDDLE;
			
		}
	}

	//sagt, ob ein entsprechender swipe vom Spieler ausgeführt wurde.

	bool swipeLeft(){
		return swipedLeft;
	}

	bool swipeRight(){
		return swipedRight;
	}

	bool swipeUp(){
		return swipedUp;
	}

	int savedSwipe = 0;

	//Input interpretieren
	void checkInput(){

		processTouches();

		if (turnPhase == 1) {
			
			if (swipeLeft()) {
				savedSwipe = 1;
				transform.GetComponent<Renderer> ().material.color = Color.blue;
			}
			if (swipeRight()){
				savedSwipe = 2;
				transform.GetComponent<Renderer> ().material.color = Color.green;
			}

		}

		if (swipeUp()) {
			Debug.Log ("Jumped");
			if (transform.position.y < 2) {
				transform.position = new Vector3(transform.position.x, maxJumpingHeight, transform.position.z);
			}
		}

	}

	void beginTurn(){
		Debug.Log ("Begin Turning, " + savedSwipe);
		if (savedSwipe == 1) {
			turnLeft ();
			transform.position = turnPosition + Vector3.up;
		}
		if (savedSwipe == 2) {
			turnRight ();
			transform.position = turnPosition + Vector3.up;
		}
		transform.GetComponent<Renderer> ().material.color = Color.white;
		savedSwipe = 0;
		turnPhase = 0;
	}

	void turnLeft(){
		direction = Quaternion.AngleAxis(-90, Vector3.up) * direction;
		rotation -= 90;
		float upwardsMovement = rigidbody.velocity.y;
		rigidbody.velocity = Vector3.zero;
		rigidbody.velocity += new Vector3(0, upwardsMovement, 0);
	}
	void turnRight(){
		direction = Quaternion.AngleAxis(+90, Vector3.up) * direction;
		rotation += 90;
		float upwardsMovement = rigidbody.velocity.y;
		rigidbody.velocity = Vector3.zero;
		rigidbody.velocity += new Vector3(0, upwardsMovement, 0);
	}
	

	void die(){
		rigidbody.velocity = Vector3.zero;
		direction = new Vector3(0, 0, 1);
		rotation = 0;
		transform.position = new Vector3(0, 1, 0);
		running = false;


		transform.GetComponent<Renderer> ().material.color = Color.white;
		text.SetActive (true);
		currentPos = MIDDLE;
		InputSimulation.acceleration.x = 0f;
		turnPhase = 0;

		/*
		 * Somehow reset all the platforms?
		 */

		/*
		var t = new GameObject[] {m_left, m_right, m_mid, m_trigger, m_dis};
		foreach (GameObject m in t) {	
			var obj = GameObject.FindObjectsOfType (m);
			foreach (GameObject o in obj) {
				Destroy (o);
			}
		}*/
	}


	void checkDeath() {
		if (transform.position.y < 0.0f) {
			Debug.Log ("Player Died by falling off platform");
			die ();
		}
	}

	/**
	 * make sure the player runs only in one direction.
	 */
	void freezePosition(){
		if (direction == new Vector3(0, 0, 1) || direction == new Vector3(0, 0, -1)) {
			rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
		} else {
			rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		}
		rigidbody.constraints |= RigidbodyConstraints.FreezeRotation;
	}

	TurnTrigger nextTurn;
	Vector3 turnPosition;
	int turnPhase = 0;

	void OnCollisionEnter(Collision col){

		if (col.gameObject.tag == "Distraction") {
			Debug.Log ("Player Died by Distraction");
			die ();
		}
		// TODO: Wenn man gerade hüpft wird collisionEnter nicht ausgeführt
		// stattdessen sollte man vll lieber auf Distanz bei x und z Werte im Update prüfen
		if (turnPhase == 1) {
			Debug.Log (col.gameObject.name + " - " +
				(col.gameObject == nextTurn.left) + " - " +
				(col.gameObject == nextTurn.middle) + " - " +
				(col.gameObject == nextTurn.right) + " - " + currentPos);
			
			if (col.gameObject == nextTurn.left) {
				turnPhase = 2;
				// bin mir unsicher wo sich die positonen vertauschen
				// aber wenn hier links steht ist es immer die falsche platform
				turnPosition = nextTurn.leftPos;
				beginTurn ();
			}
			if (col.gameObject == nextTurn.middle) {
				turnPhase = 2;
				turnPosition = nextTurn.midPos;
				beginTurn ();
			}
			if (col.gameObject == nextTurn.right) {
				turnPhase = 2;
				// genauso hier mit rechts, kein plan
				turnPosition = nextTurn.rightPos;
				beginTurn ();
			}
		}

	}

	void OnTriggerEnter(Collider col){
		if (turnPhase == 0 && col.gameObject.tag == "TurnTrigger") {
			nextTurn = col.gameObject.GetComponent<TurnTrigger> ();
			turnPhase = 1;
			Debug.Log ("Enter TurnPhase 1");
			transform.GetComponent<Renderer> ().material.color = Color.red;
		}
	}
}
