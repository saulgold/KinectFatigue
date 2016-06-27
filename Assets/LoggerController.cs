using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoggerController : Listener {

	// Use this for initialization
	public Button logButton;
	public InputField fieldName;
	public Text info;
	private string dataBack;

	void Start () {
		logButton.interactable = false;
	}
	override public void EventAction(Component compo){}

	override public void EventAction(MonoBehaviour mono = null) {
		logButton.interactable = true;

		GameControl.control.Save(fieldName.text);

		Application.LoadLevel("MenuChooseSport");
	}

	override public void EventActionTrue() {

		info.color = Color.green;
		info.text = "OK";
		logButton.interactable = true;

	}
	override public void EventActionFalse() {
		if(info != null) {
			info.color = Color.red;
			info.text  = "Name not found";
		}

		if(logButton != null)
			logButton.interactable = false;

		if(GameControl.control.VerifyNameFile (fieldName.text)) {
			EventActionTrue();

		}

	}

	public void Load() {

		if(!GameControl.control.isConnectedToServer() && GameControl.control.VerifyNameFile(fieldName.text)) {
			GameControl.control.Load (fieldName.text);
			Application.LoadLevel("MenuChooseSport");
			return;
		}


		if(!GameControl.control.VerifyNameFile(fieldName.text)) {
			GameControl.control.GetDataServer(fieldName.text, this);

		} else {
			if (GameControl.control.Load (fieldName.text)) {
				Debug.Log ("Success Load");
				GameControl.control.SendData("VerifyDate.php", "name", fieldName.text, Result, null);
			}
		}
	}

	void Update() {

		if (Input.GetKey("return")) {

			if(logButton.interactable)
				Load();
		}
	}

	void Result(MonoBehaviour sender) {
		string data = GameControl.control.GetResultRequest();
		DateTime t = DateTime.ParseExact(data, "yyyy-MM-dd HH:mm:ss",null);

		if(GameControl.control.CompareDate(t) > 0) {
			Debug.Log ("Update");
			GameControl.control.GetDataServer(fieldName.text, this);
		
		}
	
		Application.LoadLevel("MenuChooseSport");
	}
	
	public void Verify() {

		GameControl.control.SendNameDatabase(this, fieldName.text);
	}
}
