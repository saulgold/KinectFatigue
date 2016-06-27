using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Initialize the connection with the server
public class Initializer : Listener {

	// Use this for initialization
	public Text testConnection;


	void Start () {
		if(GameControl.control != null) {
			GameControl.control.ConfigRead("config");
			GameControl.control.TestConnection(this);
		}
	}
	override public void EventAction(Component compo){}

	public void TestConnection() {
		GameControl.control.TestConnection(this);
	}

	override public void EventAction(MonoBehaviour mono = null) {
	
	}

	override public void EventActionTrue() {
		testConnection.text = "Test Connection Succeed";
		testConnection.color = Color.green;
	}

	override public void EventActionFalse() {
		if(testConnection != null) {
			testConnection.text = "Test Connection Failed";
			testConnection.color = Color.red;
		}
	}

	void ResultTestConnection() {

	}
	
	// Update is called once per frame
	void Update () {

	}
}
