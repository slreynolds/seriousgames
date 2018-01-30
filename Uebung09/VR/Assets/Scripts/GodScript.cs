using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodScript : MonoBehaviour
{
    public GameObject m_hitpoint;

    //private GameObject 

    // Use this for initialization
    void Start()
    {
        m_hitpoint = Instantiate(m_hitpoint);
    }


    GameObject last;
    float timestamp;


    // Update is called once per frame
    void Update()
    {
        Transform cam = Camera.main.transform;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(cam.position, cam.forward), out hit))
        {
            //Debug.Log(hit.transform.tag + " hit");

            hit.transform.GetComponent<IWatch>().IsGazedUpon();

            m_hitpoint.transform.position = hit.point;

            var current = hit.transform.gameObject;

            bool test = last != null && last.Equals(current);

            //Debug.Log((timestamp + 2f).ToString() + " / "
            //    + Time.time + " - " + last != null + " | ");

            if (test)
            {
                if (timestamp + 2f < Time.time)
                    hit.transform.GetComponent<IWatch>().Activate();
                
            }
            else
            {
                last = current;
                timestamp = Time.time;
            }

        }
        else
        {
			
			Debug.Log (cam.transform.rotation);
			m_hitpoint.transform.position =
				cam.transform.position + cam.transform.forward * 10f;
            last = null;
        }
    }
}
