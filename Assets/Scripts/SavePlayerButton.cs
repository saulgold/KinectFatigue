using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SavePlayerButton : MonoBehaviour {

	// Use this for initialization
	public GameObject gameControl;
	public InputField namePlayerField;

	public void SaveName() {
		GameControl.control.namePlayer = namePlayerField.text;
	}

	public void LogName() {
		if (GameControl.control.Load (namePlayerField.text)) {
			Debug.Log ("Success Load");
			Application.LoadLevel("MenuChooseSport");
		}
	}
}
