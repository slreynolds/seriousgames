using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformBuilder : MonoBehaviour
{
    public GameObject left, right, middle;
    public GameObject box;
	public GameObject turnTrigger;
    public Vector3 currentPosition;
	Quaternion q;
    float offset = 150f;

    Vector3 perspective,
            turn_left,
            turn_right;

    /// <summary>
    /// the current turn index must be between -1 and +1.
    /// for a left turn it will be decreased and for a right turn it will be increased.
    /// This field controls the flow and avoids loops in the lane by only allows two left turns (or right turns) in a row.
    /// </summary>
    int current_turn_index; // [-1; +1]

	//Color lcolor, mcolor, rcolor;

    // Use this for initialization
    void Start()
    {
        perspective = Vector3.forward;
        current_turn_index = 0;

        q = Quaternion.Euler(RotatePerspective());

		//lcolor = new Color (Random.value, Random.value, Random.value, 1);
		//rcolor = new Color (Random.value, Random.value, Random.value, 1);
		//mcolor = new Color (Random.value, Random.value, Random.value, 1);

        // first 3 straight lanes
        for (int i = 0; i < 3; i++)
        {
			createNormal ();
            currentPosition = currentPosition + perspective * 5;
		}
    }

    // Update is called once per frame
    void Update()
    {
		
		if (Vector3.Distance(transform.position, currentPosition) < offset)
        {
            //Debug.Log ("Created Platform"+transform.position.ToString() + ", " +  currentPosition.ToString());

            q = Quaternion.Euler(RotatePerspective());

            int type = Random.Range(0, 14);
            if (type > 8)
            {
				createNormal ();
            }
            else if (type > 6)
            {
				createSingle ();

            }
            else if (type > 3)
            {
                // create turn platform
                // wenn wert = 0, dann wird eine beliebige Richtung gewählt, sonst links bei +1, oder rechts bei -1
                int turn = current_turn_index == 0 ? (int)Mathf.Sign(Random.value - 0.5f) : (int)Mathf.Sign(current_turn_index) * -1;

                if (turn < 0)
                {
					createLeftTurn ();
                }
                else
                {
					createRightTurn ();

                }

                current_turn_index += turn;
            }
            else
            {
				createJump ();
            }

            currentPosition = currentPosition + perspective * 5;
        }
    }


	void createNormal(){

		// create normal platform
		Instantiate(left, currentPosition + turn_left, q);
		Instantiate(right, currentPosition + turn_right, q);
		Instantiate(middle, currentPosition, q);
		//l.gameObject.GetComponent<Renderer> ().material.color = lcolor;
		//r.gameObject.GetComponent<Renderer> ().material.color = rcolor;
		//m.gameObject.GetComponent<Renderer> ().material.color = mcolor;
	}
	void createSingle(){

		// create single platform
		float coolwhere = Random.Range(0, 3);
		if (coolwhere > 2f)
			Instantiate(left, currentPosition + turn_left, q);
		else if (coolwhere > 1f)
			Instantiate(right, currentPosition + turn_right, q);
		else
			Instantiate(middle, currentPosition, q);
	}
	void createLeftTurn(){

		var trigger = Instantiate (turnTrigger,
			currentPosition - 2 * perspective,
			Quaternion.Euler(Quaternion.AngleAxis(-90, Vector3.up) * perspective));
		// turn left
		var linner = Instantiate(left, currentPosition + turn_left - perspective * 1.5f, q);
		var lmiddle = Instantiate(middle, currentPosition - perspective, q);
		var louter = Instantiate(right, currentPosition + turn_right - perspective * .5f, q);

		louter.transform.localScale = new Vector3(4f, 1f, 1f);
		lmiddle.transform.localScale = new Vector3(3f, 1f, 1f);
		linner.transform.localScale = new Vector3(2f, 1f, 1f);

		currentPosition = currentPosition + perspective;
		perspective = turn_left;
		q = Quaternion.Euler(RotatePerspective());
		currentPosition = currentPosition + 2 * perspective;

		linner = Instantiate(left, currentPosition + turn_left, q);
		lmiddle = Instantiate(middle, currentPosition - perspective * .5f, q);
		louter = Instantiate(right, currentPosition + turn_right - perspective, q);

		linner.transform.localScale = new Vector3(3f, 1f, 1f);
		lmiddle.transform.localScale = new Vector3(4f, 1f, 1f);

		// init trigger so Players know when to turn
		trigger.GetComponent<TurnTrigger> ().init (linner,
			lmiddle,
			louter,
			linner.transform.position - perspective,
			lmiddle.transform.position - 1.5f*perspective,
			louter.transform.position - 2f*perspective);
		currentPosition = currentPosition - perspective;
	}
	void createRightTurn(){

		var trigger = Instantiate (turnTrigger,
			currentPosition - 2 * perspective,
			Quaternion.Euler(Quaternion.AngleAxis(-90, Vector3.up) * perspective));
		// turn right
		var linner = Instantiate(left, currentPosition + turn_left - perspective * .5f, q);
		var lmiddle = Instantiate(middle, currentPosition - perspective, q);
		var louter = Instantiate(right, currentPosition + turn_right - perspective * 1.5f, q);

		louter.transform.localScale = new Vector3(2f, 1f, 1f);
		lmiddle.transform.localScale = new Vector3(3f, 1f, 1f);
		linner.transform.localScale = new Vector3(4f, 1f, 1f);

		currentPosition = currentPosition + perspective;
		perspective = turn_right;
		q = Quaternion.Euler(RotatePerspective());
		currentPosition = currentPosition + 2 * perspective;

		linner = Instantiate(left, currentPosition + turn_left - perspective, q);
		lmiddle = Instantiate(middle, currentPosition - perspective * .5f, q);
		louter = Instantiate(right, currentPosition + turn_right, q);

		louter.transform.localScale = new Vector3(3f, 1f, 1f);
		lmiddle.transform.localScale = new Vector3(4f, 1f, 1f);
		trigger.GetComponent<TurnTrigger> ().left = linner;
		trigger.GetComponent<TurnTrigger> ().middle = lmiddle;
		trigger.GetComponent<TurnTrigger> ().right = louter;

		// init Trigger so player knows when to turn
		trigger.GetComponent<TurnTrigger> ().init (linner,
			lmiddle,
			louter,
			linner.transform.position - 2f*perspective,
			lmiddle.transform.position - 1.5f*perspective,
			louter.transform.position - perspective);
		currentPosition = currentPosition - perspective;
		
	}
	void createJump(){

		// create hüpfen platform
		Instantiate(left, currentPosition + turn_left, q);
		Instantiate(right, currentPosition + turn_right, q);
		Instantiate(middle, currentPosition, q);

		Instantiate(box, currentPosition + randomPos() + Vector3.up, q);
	}

    Vector3 randomPos()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return turn_left;
            case 1:
                return turn_right;
            default:
                return Vector3.zero;
        }

    }

    Vector3 RotatePerspective()
    {
        turn_left = Quaternion.AngleAxis(-90, Vector3.up) * perspective;
        turn_right = Quaternion.AngleAxis(90, Vector3.up) * perspective;

        float y = 0; // perspective == Vector3.right
        if (perspective == Vector3.forward) y = 90f;
        else if (perspective == Vector3.back) y = 270f;
        else if (perspective == Vector3.right) y = 180f;

        return new Vector3(0, y, 0);
    }

}