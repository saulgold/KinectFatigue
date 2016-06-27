using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class SceneControl : MonoBehaviour {

	public Text namePlayerText;
	public Text scorePlayerText;
	public Text timeText;
	public Text timeDisplay;
	public Text instructionText;

	private int score = 0;
	private double time = 120.0f;
	private bool hasStarted = false;
	private bool isRunning = false;
	private int decreaseCounter = 30;
	public string nameGame;

	// Use this for initialization
	void Start () {
		string p = "";
		if(GameControl.control != null)
			p = GameControl.control.namePlayer;
		if(!p.Equals(""))
			namePlayerText.text = p;
		else namePlayerText.text = "No name Player";

		if(GameControl.control != null)
		if(GameControl.control.gameName.Equals("")) GameControl.control.gameName = nameGame;

		if(GameControl.control != null && GameControl.control.isLevel())
			GameControl.control.PrepareDatas();
	}

	public void LaunchTimer() {
		isRunning = true;
		hasStarted = true;
	}

	public void StopTimer() {
		isRunning = false;
	}

	public void StartTimer() {
		isRunning = true;
	}

	public bool GetIsRunning() {
		return isRunning;
	}

	//Restart never used (may have some bugs)
	public void RestartGame() {
		time = 120.0f;
		score = 0;
		instructionText.text = "Raise right Hand to start\n" ; 
		hasStarted = false;
		isRunning = false;
		timeText.text = "Time : " + time.ToString("F0");

		Debug.Log ("restart");

	}

	public void SetRun(bool b) {
		isRunning = b;
	}

	public void PauseTime() {

		isRunning = !isRunning;
	}
	
	// Update is called once per frame
	void Update () {

		if (!hasStarted) {
			instructionText.text = "Raise right Hand to start\n" ; 
		}

		if (!timeDisplay.GetComponent<Animation> ().isPlaying) {
			timeDisplay.gameObject.SetActive(false);
		}

		if (hasStarted) {
			instructionText.gameObject.SetActive(false);
			hasStarted = false;
			timeDisplay.gameObject.SetActive(true);
			timeDisplay.GetComponent<Animation>().Play();

		}

		if (isRunning) {
			time -= Time.deltaTime;
			timeText.text = "Time : " + time.ToString("F0");
			if(time <= decreaseCounter) {
				Debug.Log(decreaseCounter.ToString());
				timeDisplay.gameObject.SetActive(true);

				timeDisplay.text = decreaseCounter.ToString() + " !";
				timeDisplay.GetComponent<Animation>().Play();
				decreaseCounter--;

			}
			if(time<=0.0f) {
				Finish();
			}
		}

	}

	public void scoreIncrease() {
		score++;
		scorePlayerText.text = "Score : " + score.ToString ();
	}

	public void Finish() {
		if(GameControl.control.isLevel()) {
		
			GameControl.control.AddDataLevel(score);
			Application.LoadLevel ("EndLevel");
		} else {
			KinectManager manager = KinectManager.Instance;
			Destroy(manager.gameObject);
			Application.LoadLevel ("MenuSportTypeLevel");

		}
	}
}
