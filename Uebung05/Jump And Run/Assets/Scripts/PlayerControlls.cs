using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{
    public AudioClip m_jumpsound, m_pusound;

    private AudioSource m_audio;
    private Rigidbody m_rigid;

    private int m_curdirection;
    private bool m_isJumping = false;

    private GodScript god;

    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_rigid = GetComponent<Rigidbody>();

        // get the God Script for accessing the powerCounter
        GameObject go = GameObject.Find("Main Camera");
        god = (GodScript)go.GetComponent(typeof(GodScript));
    }

    void Update()
    {
        checkInput();
        move();
    }

    // Abfrage der Steuerung
    private void checkInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            m_curdirection = -1;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            m_curdirection = 1;
        else
            m_curdirection = 0;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (!m_isJumping)
            {
                m_audio.PlayOneShot(m_jumpsound);
                m_isJumping = true;

                m_rigid.AddForce(0, 10, 0, ForceMode.VelocityChange);
            }
        }
    }

    private void move()
    {
        // TODO: sometimes the powerups run out while the player is jumping
        //       maybe save the powercounter in a local variable and only update when
        //       not jumping?
        float speed = ((god.powerCounter + 1) * .018f + .18f) * m_curdirection;
        transform.position += new Vector3(speed, 0f, 0f);
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Platform")
        {
            // sometimes the player was jumping REALLY high
            // so lets check for high velocity
            if (GetComponent<Rigidbody>().velocity.y < 5)
            {
                m_isJumping = false;

                /*
				// detect if the player landed on the top of the platform
				// TODO: sometimes doesnt work when the player enters the collision
				//       from the sides of the platform

				Vector3 hit = col.contacts [0].normal;
				if (hit == Vector3.up) {
					m_isJumping = false;
				}
				*/
            }

        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PowerUP")
        {
            m_audio.PlayOneShot(m_pusound);
        }
    }


    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Platform")
        {
            m_isJumping = true;
        }

    }


}
