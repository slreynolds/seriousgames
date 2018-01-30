using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BallScript : NetworkBehaviour
{

    void Start()
	{
		Debug.Log ("ball created");
    }


    void OnCollisionEnter(Collision c)
	{

		GetComponent<Rigidbody>().AddForce(c.contacts[0].normal * 5, ForceMode.Acceleration);
    }

}
