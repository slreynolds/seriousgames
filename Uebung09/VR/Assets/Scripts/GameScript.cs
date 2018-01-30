using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {

    public GameObject Enemy;


	bool isPlaying = false;
	List<GameObject> enemies;
	float points;
	public TextMesh tm;
	public UnityEngine.UI.Graphic graphic;

	// Use this for initialization
	void Start () {
		graphic.CrossFadeAlpha (0, 2f, true);
		Invoke ("StartGame", 3);
	}

	void StartGame(){
		enemies = new List<GameObject> ();
		Debug.Log ("Starting game");
		isPlaying = true;
		time = 4f;
		points = 0f;
		spawnEnemy ();
		tm.text = "Points: " + points;
	}

	float time;

	void spawnEnemy(){
		Vector2 circle = Random.insideUnitCircle * 5f;

		enemies.Add(Instantiate(Enemy,  new Vector3(circle.x, circle.y+5f, 50), Quaternion.identity));

		float timeToNext = 5f + Mathf.Cos (Time.deltaTime);
		if(isPlaying)
			Invoke ("spawnEnemy", timeToNext);
	}

	void ExitGame(){
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isPlaying) {
			points += enemies.RemoveAll (e => e == null);
			tm.text = "Points: " + points;
			foreach (GameObject e in enemies) {
				if (e.transform.position.z < -10) {
					isPlaying = false;
					graphic.CrossFadeAlpha (1, 2f, true);
					Invoke ("ExitGame", 3);

				}
			}
		} else {
			// dead
		}
	}
}
