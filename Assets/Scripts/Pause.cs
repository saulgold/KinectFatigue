using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	// Use this for initialization
	bool isPaused = false;
	public GUIStyle myStyle;
	public GUISkin mySkin;
	//public SceneControl control;
	void Start () {


	}

	public void PauseGame() {
		Time.timeScale	= (Time.timeScale ==0) ? 1:0;
		isPaused = !isPaused;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		GUI.skin = mySkin;

		if (isPaused) {
			CreateGui ();
		
		}
	}

	private void CreateGui() {
		GUI.BeginGroup (new Rect (Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 300));
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.
		
		// We'll make a box so you can see where the group is on-screen.
		GUI.Box (new Rect (0,0,400,300), "");

		GUI.Label(new Rect(150,0,100,25),"Game is paused!",myStyle);
		if(GUI.Button(new Rect(100,50,200,25),"Click me to unpause"))
				PauseGame();
		if(GUI.Button(new Rect(100,100,200,25),"Skip")) {
			GameControl.control.hasPassed = true;
			KinectManager manager = KinectManager.Instance;
			Destroy(manager.gameObject);
			if(GameControl.control.isLevel())
				Application.LoadLevel ("MenuLevels");
			else Application.LoadLevel ("MenuSportTypeLevel");

		}


		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
	}
}
