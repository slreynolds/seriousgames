using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallScript : MonoBehaviour
{
    private Rigidbody m_rigid;

    // Use this for initialization
    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        //m_rigid.AddForce(new Vector3(10, 0, 0), ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision c)
    {
       // m_rigid.AddForce(c.contacts[0].normal * 10, ForceMode.Impulse);
    }

    void OnCollisionExit(Collision c)
    {
        if(c.gameObject.tag == "Pitch")
        {
            if(transform.position.x < 0)
            {
                // right player won

            }
            else
            {
                // left player won

            }

            // TODO: Sleep maybe?


            // set ball to the middle
            transform.position = new Vector3(0, 0.25f, 0);

            GetComponent<Rigidbody>().AddForce(
                new Vector3(Random.value*3, 0, Random.value*3),
                ForceMode.VelocityChange);
        }
    }
}
