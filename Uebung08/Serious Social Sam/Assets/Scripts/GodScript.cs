using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary; 
using System;
using System.IO;
using System.ComponentModel;

public class GodScript : MonoBehaviour {

	public GameObject player;
	public bool isPlaying = false;
	public GameObject topic;
	public GameObject topicDetailsWindow;
	public GameObject topicCreateWindow;
	public GameObject inGameExitWindow;
	public GameObject mainMenuContinueWindow;

	public GameObject camera;

	public GameObject topicCreateTitle;
	public GameObject topicCreateText;

	public GameObject topicDetailTitle;
	public GameObject topicDetailText;
	public GameObject topicDetailAuthor;
	public GameObject topicDetailLikes;
	public GameObject topicDetailDislikes;

	public GameObject playerNamesList;
	public GameObject playerButton;

	// Player Data
	public string playerName = "Nobody";
	public Color playerColor = Color.black;

	private GameObject playerObj;
	private float maxTopicDistance = 20f;
	private List<TopicScript> topics;
	private TopicScript last;
	private Vector3 orginalCameraPos;


	BinaryFormatter bf = new BinaryFormatter ();

	void Start(){
		loadStuff ();
		updatePlayerNameList ();
	}

	public void updatePlayerNameList(){
		List<string> test = new List<string> ();

		if (topics != null) {
			
			foreach (TopicScript ts in topics) {
				test.AddRange(ts.getNames ());
			}
			var unique = new HashSet<string> (test);

			foreach (string u in unique) {
				var butt = Instantiate (playerButton);
				butt.GetComponentInChildren<Text> ().text = u;

				butt.transform.SetParent (playerNamesList.transform);

				butt.GetComponent<Button>().onClick.AddListener (delegate {
					startGameWithPlayer(u);
					mainMenuContinueWindow.SetActive(false);
				});
			}
		}
	}

	void Update () {
		if (isPlaying) {
			checkMouse ();	
		}
	}

	public void deleteSaveFile(){
		topics = new List<TopicScript> ();
		saveStuff ();
	}

	public void changeGameState(string state){
		topicDetailsWindow.SetActive (false);
		topicCreateWindow.SetActive (false);
		inGameExitWindow.SetActive (false);
		isPlaying = false;

		if (playerObj != null)
			playerObj.GetComponent<PlayerScript> ().setPlaying (false);

		if (state == "mainmenu") {
			updatePlayerNameList ();
		} else if (state == "playing") {
			isPlaying = true;
			inGameExitWindow.SetActive (true);
			playerObj.GetComponent<PlayerScript> ().setPlaying (true);
		} else if (state == "details") {
			topicDetailsWindow.SetActive (true);
		} else if (state == "create") {
			topicCreateWindow.SetActive (true);
		} else {
			Debug.LogError ("state changed to: " + state);
		}
	}

	public void startGame(InputField name){
		playerName = name.text;
		name.text = "";
		playerObj = Instantiate(player, new Vector3 (0f, 2f, 0f), Quaternion.identity);
		orginalCameraPos = camera.transform.position;

		camera.transform.SetParent (playerObj.transform);
		camera.transform.rotation = Quaternion.Euler (new Vector3 (20f, 0f, 0f));

		// load game file
		changeGameState("playing");
	}

	public void startGameWithPlayer(string name){
		playerName = name;

		playerObj = Instantiate(player, new Vector3 (0f, 2f, 0f), Quaternion.identity);
		orginalCameraPos = camera.transform.position;

		camera.transform.SetParent (playerObj.transform);
		camera.transform.rotation = Quaternion.Euler (new Vector3 (20f, 0f, 0f));

		// load game file
		changeGameState("playing");
	}

	public void exitGame(){
		changeGameState("mainmenu");
		saveStuff ();
		Debug.Log ("Deleting everything from scene");
		camera.transform.SetParent(this.transform);
		camera.transform.position = orginalCameraPos;
		DestroyObject (playerObj);
		GameObject[] l = GameObject.FindGameObjectsWithTag ("Topic");
		foreach (GameObject i in l) {
			Destroy(i);
		}
		topics = new List<TopicScript> ();
		loadStuff ();
	}

	public void setColor(){
		// TODO: implement
		playerColor = Color.red;
		playerObj.transform.GetComponent<Renderer> ().material.color = playerColor;
	}

	public void saveDetailWindow(){
		last.initTopic (playerName, "white");
		last.setTitle (topicCreateTitle.GetComponent<Text> ().text);
		last.setText (topicCreateText.GetComponent<Text> ().text);
		topics.Add (last);
		topicCreateTitle.GetComponent<Text> ().text = "";
		topicCreateText.GetComponent<Text> ().text = "";
		changeGameState("playing");
	}

	public void cancelCreateTopic(){
		if (last != null) {
			last.transform.GetComponent<Rigidbody> ().AddForce (0, 5f, 0);

			Destroy (last.GetComponent<GameObject> (), 2f);
		}

	}


	public void saveStuff(){

		MemoryStream ms = new MemoryStream ();



		List<TopicData> data = new List<TopicData> ();
		foreach(TopicScript ts in topics){
			data.Add (ts.data);
		}
		bf.Serialize (ms, data);
		string tmp = System.Convert.ToBase64String (ms.ToArray ());

		PlayerPrefs.SetString ("save", tmp);



		Debug.Log ("saved topics: " + topics.Capacity);
	}

	public void upvoteTopic(){
		if (last != null) {
			var pr = new Profile ();
			pr.name = playerName;
			pr.vote = Profile.VoteDirection.upvoted;
			last.addVote (pr);

			topicDetailLikes.GetComponent<Text>().text = last.GetComponent<TopicScript> ().getLikes();
			topicDetailDislikes.GetComponent<Text>().text = last.GetComponent<TopicScript> ().getDislikes();
		}
	}

	public void downvoteTopic(){
		Debug.Log ("Downvoting");
		if (last != null) {
			var pr = new Profile ();
			pr.name = playerName;
			pr.vote = Profile.VoteDirection.downvoted;
			last.addVote (pr);

			topicDetailLikes.GetComponent<Text>().text = last.GetComponent<TopicScript> ().getLikes();
			topicDetailDislikes.GetComponent<Text>().text = last.GetComponent<TopicScript> ().getDislikes();

			Debug.Log ("Downvoted ");
		}
	}

	public void loadStuff(){
		topics = new List<TopicScript> ();
		if (!PlayerPrefs.HasKey ("save")) {
			Debug.Log ("Created new topics: " + topics.Capacity);
		} else {
			string data = PlayerPrefs.GetString ("save");
			MemoryStream ds = new MemoryStream(System.Convert.FromBase64String(data));
			List<TopicData> td = (List<TopicData>) bf.Deserialize(ds);
			Debug.Log ("Loadded topics: " + td.Capacity);
			foreach(TopicData t in td){
				var newTopic = Instantiate (topic, Vector3.up, Quaternion.identity);

				newTopic.GetComponent<TopicScript> ().load (t);
				topics.Add (newTopic.GetComponent<TopicScript> ());
			}
		}
	}

	void checkMouse(){
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(ray,out hit, maxTopicDistance)){
				Debug.Log("You selected "+ hit.transform.name);

				if (hit.transform.name == "Terrain") {

					// create new object at spot
					var newTopic = Instantiate(topic,
						hit.point + Vector3.up,
						Quaternion.identity);
					newTopic.GetComponent<TopicScript> ().data.ownerName = playerName;
					newTopic.GetComponent<TopicScript> ().data.color = "black";
					newTopic.GetComponent<TopicScript> ().data.text = "";
					newTopic.GetComponent<TopicScript> ().data.x = hit.point.x;
					newTopic.GetComponent<TopicScript> ().data.z = hit.point.z;
					last = newTopic.GetComponent<TopicScript> ();
					changeGameState ("create");
				}
				if (hit.transform.tag == "Topic") {
					// Open Editor for Topic
					changeGameState ("details");
					topicDetailText.GetComponent<Text>().text = hit.transform.GetComponent<TopicScript> ().data.text;
					topicDetailTitle.GetComponent<Text>().text = hit.transform.GetComponent<TopicScript> ().data.title;
					topicDetailAuthor.GetComponent<Text>().text = hit.transform.GetComponent<TopicScript> ().data.ownerName;
					topicDetailLikes.GetComponent<Text>().text = hit.transform.GetComponent<TopicScript> ().getLikes();
					topicDetailDislikes.GetComponent<Text>().text = hit.transform.GetComponent<TopicScript> ().getDislikes();
					last = hit.transform.GetComponent<TopicScript> ();
				}
				if (hit.transform.tag == "Player") {
					playerObj.GetComponent<Rigidbody>().AddForce(0, 2f, 0, ForceMode.VelocityChange);
				}
			}
		}
	}
}
