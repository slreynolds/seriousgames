using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkScript : NetworkManager
{

    public GameObject Paddle, Ball;
    public Text PointsClient, PointsHost;

    private const float step = .2f;
    private GameObject m_host, m_client, m_ball;

    // Use this for initialization
    void Start()
    {
        PointsClient.text = "0";
        PointsHost.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            m_host.transform.position = m_host.transform.position + Vector3.right * step;
        else if (Input.GetKey(KeyCode.DownArrow))
            m_host.transform.position = m_host.transform.position - Vector3.right * step;

        if (Input.GetKey(KeyCode.W))
            m_client.transform.position = m_client.transform.position + Vector3.right * step;
        else if (Input.GetKey(KeyCode.S))
            m_client.transform.position = m_client.transform.position - Vector3.right * step;
    }

    public override NetworkClient StartHost()
    {
        Debug.Log("Started Host");
        PointsClient.text = "0";
        PointsHost.text = "0";
        return base.StartHost();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Client Connected");

        PointsClient.text = "0";
        PointsHost.text = "0";

        // spawn paddles
        m_host = Instantiate(Paddle, new Vector3(0, .25f, -4.5f), Quaternion.identity);
        m_client = Instantiate(Paddle, new Vector3(0, .25f, 4.5f), Quaternion.identity);
        m_ball = Instantiate(Ball, new Vector3(0, .25f, 0f), Quaternion.identity);
        m_ball.GetComponent<Rigidbody>().velocity = new Vector3(Random.value, Random.value, 0f);

        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Client Disconnect");

        base.OnClientDisconnect(conn);
    }
}
