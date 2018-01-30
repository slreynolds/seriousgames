using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IWatch
{

	Color m_startcolor = Color.red,
	m_endcolor = Color.black;

    // Use this for initialization
    void Start()
    {
		transform.GetComponent<Renderer> ().material.color = Color.HSVToRGB (Random.value,Random.value,Random.value);
    }

	float speed = 2f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.back * 0.1f;

        var rot = transform.rotation;
		float r = Random.value * speed;

		transform.rotation = rot * Quaternion.AngleAxis (r, Vector3.one);
    }


    public void Activate()
    {
		Destroy (gameObject);
    }

    public void IsGazedUpon()
    {

		float lerp = Mathf.PingPong(Time.time, 0.5f) * 5f;
		GetComponent<Renderer>().material.color = Color.Lerp(m_startcolor, m_endcolor, lerp);
    }
}
