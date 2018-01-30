using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
	private const float step = .2f;
	Rigidbody localRigidBody;

	void Start(){
		localRigidBody = GetComponent<Rigidbody> ();
	}



    // Update is called once per frame
    void Update()
    {
		if (isLocalPlayer) {
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				DoMove (transform.position + Vector3.right * step);
			}else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
				DoMove (transform.position - Vector3.right * step);
		}
    }


	void DoMove(Vector3 pos){
		localRigidBody.MovePosition (pos);
	}
}
