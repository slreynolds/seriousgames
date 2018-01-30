using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	public string name;
	public string color;


	public void exitGame(){
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
	}

	public void changeVolume(int value){
		Debug.Log (value);
	}
}
