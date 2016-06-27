using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Toggle : MonoBehaviour {

	// Use this for initialization
	public Text text;
	public void OnValueChanged(bool value) {
		if (value)
			text.text = "Window mode";
		else
			text.text = "FullScreenMode";

		SettingsData.Settings.isWindowed = value;
	}
}
