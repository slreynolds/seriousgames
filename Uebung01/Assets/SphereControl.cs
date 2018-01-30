using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SphereControl : MonoBehaviour
{
    public Text caption;

    // Use this for initialization
    void Start()
    {
        caption.text = "This is ScreenText using the YSIWYG-UI-System.";
    }

    public void MoveUp()
    {
        transform.Translate(0, 1.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(new Vector3(0, .3f, 0));

        if (Input.GetKey(KeyCode.DownArrow))
            transform.position += new Vector3(0, -.3f, 0);
    }
}
