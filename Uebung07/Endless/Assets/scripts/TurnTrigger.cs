using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTrigger : MonoBehaviour {

	public GameObject left, middle, right;
	public Vector3 leftPos, midPos, rightPos;

	public void init(GameObject left, GameObject middle, GameObject right, Vector3 leftPos, Vector3 midPos, Vector3 rightPos){
		this.left = left;
		this.middle = middle;
		this.right = right;
		this.leftPos = leftPos;
		this.midPos = midPos;
		this.rightPos = rightPos;
	}
}
