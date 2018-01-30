using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour, IWatch
{

    Color m_startcolor = Color.white,
          m_endcolor = Color.green;

    Renderer m_renderer;


    private float m_time;

    // Use this for initialization
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
		fade = false;
		graphic.CrossFadeAlpha (0, 2f, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_time + .5 < Time.time)
            m_renderer.material.color = m_startcolor;
    }

	public UnityEngine.UI.Graphic graphic;



	void StartGame(){
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	bool fade = false;

    ///// IWatch implementation ///////////////////////////////////////////////////////////////////
    public void Activate()
    {
        m_renderer.material.color = Color.red;
        Debug.Log("Activated");
        if(this.tag == "Start")
		{
			if (!fade) {
				Debug.Log("Chaning scene");
				graphic.CrossFadeAlpha (1, 2f, true);
				Invoke ("StartGame", 3);
			}
            fade = true;
		}else if(this.tag == "Exit"){
			
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		    Application.Quit ();
#endif
        }
    }

    public void IsGazedUpon()
    {
        float lerp = Mathf.PingPong(m_time = Time.time, 0.5f) * 2f;
        m_renderer.material.color = Color.Lerp(m_startcolor, m_endcolor, lerp);
    }
}
