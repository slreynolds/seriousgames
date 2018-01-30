using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkScript : NetworkManager
{

    public GameObject Paddle, Ball;

    public Text PointsClient, PointsHost;

    private GameObject m_host, m_client, m_ball;
	private bool isBallSpawned;

    // Use this for initialization
    void Start()
    {
        PointsClient.text = "0";
        PointsHost.text = "0";
		isBallSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
		// is there a ball yet?
		if (!isBallSpawned) {
			if (numPlayers == 2) {
				isBallSpawned = true;
				Debug.Log ("Ball Spawned");
				m_ball = (GameObject)Instantiate (Ball, new Vector3 (0, .3f, 0f), Quaternion.identity);

				NetworkServer.Spawn (m_ball);
				spawnNewBall();
			}
		} else {
			// check if ball is out of boundarys
			if(m_ball.transform.position.z < -5f)
			{
				// right player won
				PointsScript s = (PointsScript) PointsClient.GetComponent(typeof(PointsScript));
				s.RpcPlusOne ();
				spawnNewBall();
			}
			else if(m_ball.transform.position.z > 5f)
			{
				// left player won
				PointsScript s = (PointsScript) PointsHost.GetComponent(typeof(PointsScript));
				s.RpcPlusOne ();
				spawnNewBall();
			}
		}
    }

	void spawnNewBall(){

		Debug.Log ("Spawning new ball");

		// set ball to the middle
		float direction = Random.Range (-1, 1) < 0 ? -1f : 1f;
		float offset = Random.Range (2f, 5f);
		float force = 3f;
		m_ball.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range (-1, 1), 0f, direction * force);
		m_ball.transform.position = new Vector3(0, 0.3f, 0);
	}

    public override NetworkClient StartHost()
    {
        Debug.Log("Started Host");
        PointsClient.text = "0";
        PointsHost.text = "0";
        return base.StartHost();
	}


	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		
		base.OnServerAddPlayer (conn, playerControllerId);

		Debug.Log("OnServerAddPlayer, " + numPlayers);

		/*
		playerNumber = playerNumber + 1;

		if (playerNumber == 1)
			host = conn;
		if (playerNumber == 2)
			client = conn;

		PointsClient.text = "0";
		PointsHost.text = "0";

		// if two players are connected
		// this is off by one...
		if (playerNumber == 2) {
			// spawn paddles
			Debug.Log("two players conncted spawning stuff");

			m_host = Instantiate(Paddle, new Vector3(0, .25f, -4.5f), Quaternion.identity);
			m_client = Instantiate(Paddle, new Vector3(0, .25f, 4.5f), Quaternion.identity);


			m_ball = Instantiate(Ball, new Vector3(0, .25f, 0f), Quaternion.identity);
			m_ball.GetComponent<Rigidbody>().velocity = new Vector3(Random.value, Random.value, 0f);
		}*/

	}

    public override void OnClientConnect(NetworkConnection conn)
    {
		Debug.Log("OnClientConnect, " + numPlayers);
		// somehow this is never called when a player connects
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Client Disconnect");
		// TODO: despawn
        base.OnClientDisconnect(conn);
    }
}
