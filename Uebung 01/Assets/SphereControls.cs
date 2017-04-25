using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SphereControls : MonoBehaviour {

    public Text caption;

	// Use this for initialization
	void Start () {
        caption.text = "This is ScreenText using YSIWYG-UI System";
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0.3f, 0));
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 delta = new Vector3(0, -0.3f, 0);
            transform.position += delta;
        }
	}
}
