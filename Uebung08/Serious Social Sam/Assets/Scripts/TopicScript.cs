using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Profile {
	public string name = "";
	public VoteDirection vote;
	public enum VoteDirection {upvoted, notvoted, downvoted};
}

[System.Serializable]
public class TopicData{
	public string text;
	public string title;
	public List<Profile> votes;
	public string ownerName;
	public string color;
	public float x;
	public float z;

}


public class TopicScript : MonoBehaviour {

	public TopicData data = new TopicData ();

	public void load(TopicData td){
		data = td;
		transform.position = new Vector3 (td.x, 1, td.z);
		setObjectSize ();
	}

	public void initTopic(string owner, string color){
		data.votes = new List<Profile>();
		data.color = color;
		data.ownerName = owner;
		data.text = "hallo welt";
		//GetComponent<Renderer> ().material.color = Color.white;
	}

	public void setText(string text){
		data.text = text;
	}
	public void setTitle(string title){
		data.title = title;
	}

	public List<string> getNames(){

		List<string> test = new List<string> ();
		test.Add (data.ownerName);

		foreach(var ele in data.votes){
			test.Add (ele.name);
		}
		return test;
	}

	public void setObjectSize(){
		// Calculate new difference
		int dif = 0;
		foreach(var ele in data.votes){
			if(ele.vote == Profile.VoteDirection.upvoted)
				dif += 1;
			else if(ele.vote == Profile.VoteDirection.downvoted)
				dif -= 1;
		}
		// Difference cant be lower then 1
		dif = dif < 1 ? 1 : dif;

		transform.localScale = Vector3.one * (1f + (0.2f * dif));
	}

	public void addVote(Profile player){
		// Player is owner do nothing
		if (player.name == data.ownerName)
			return;

		// If player has already votes remove him
		data.votes.RemoveAll(e => e.name== player.name);
		data.votes.Add (player);

		setObjectSize ();
	}

	public bool isOwner(string name){
		return data.ownerName == name;
	}

	public bool hasVoted(string name){
		bool hasVoted = false;
		foreach(var ele in data.votes){
			hasVoted = hasVoted || ele.name == name;
		}
		return hasVoted;
	}

	public string getLikes(){
		string res = "";
		foreach (var ele in data.votes) {
			if(ele.vote == Profile.VoteDirection.upvoted)
				res += "\n" + ele.name;
		}
		return res;
	}

	public string getDislikes(){
		string res = "";
		foreach (var ele in data.votes) {
			if(ele.vote == Profile.VoteDirection.downvoted)
				res += "\n" + ele.name;
		}
		return res;
	}
}
