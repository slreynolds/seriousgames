using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PointsScript : NetworkBehaviour
{
	[SyncVar]
	public int points = 0;

	void Update(){
		GetComponent<Text>().text = points.ToString();
	}

	[ClientRpc]
	public void RpcPlusOne(){
		points = points + 1;
	}

	void Start()
	{
		Debug.Log ("bla created");
	}

}
