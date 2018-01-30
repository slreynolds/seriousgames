using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
	public float lifetime;
	public bool isDying;

    // Use this for initialization
    void Start()
    {
		lifetime = 10f;
		isDying = false;
    }

	void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			GetComponent<Renderer> ().enabled = false;
			GetComponent<Rigidbody> ().detectCollisions = false;
			isDying = true;
			Destroy (gameObject, lifetime);
        }
    }

}
