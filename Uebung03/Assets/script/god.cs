using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class god : MonoBehaviour {

    public Text m_text_points;

    private int m_points;

    // Use this for initialization
    void Start()
    {
        m_points = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPoints(int points)
    {
        m_points += points;
        m_text_points.text = points.ToString("000");
    }
}
