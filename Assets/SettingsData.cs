using UnityEngine;
using System.Collections;

public class SettingsData : MonoBehaviour {


	public static SettingsData Settings;
	public Resolution res;
	public bool isWindowed = false;

	private ColorS colorButton = new ColorS(0.0f,0.6f,0.25f);
	private ColorS colorText = new ColorS(0.0f,0.6f,0.25f);
	public string themeName = "";
	void Awake() {
		if (Settings == null) {
			DontDestroyOnLoad (gameObject);
			Settings = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		if(GameControl.control != null) {
			colorButton = GameControl.control.colorTheme.button;
			colorText = GameControl.control.colorTheme.text;
		}
	}

	public ColorS GetColorButton() {
		return colorButton;
	}

	public ColorS GetColorText() {
		return colorText;
	}



	public void SetColors(string nom) {
		themeName = nom;
	switch(nom) {
		case "Normal" :
			colorButton = new ColorS(0.0f,0.6f,0.25f);
			 colorText = new ColorS(0.0f,0.6f,0.25f);
			GameControl.control.SaveColor(colorButton, colorText);

		break;
		case "Oblivion" : 
			colorButton = new ColorS(1.0f, 0.0f, 0.0f);
			colorText = new ColorS(0.9f,0.0f,0.0f);
			GameControl.control.SaveColor(colorButton, colorText);
		break;
		default : break;
		}
	}

	public void SetColors() {

		switch(themeName) {
		case "Normal" :
			colorButton = new ColorS(0.0f,0.6f,0.25f);
			colorText = new ColorS(0.0f,0.6f,0.25f);
			
			break;
		case "Oblivion" : 
			colorButton = new ColorS(1.0f, 0.0f, 0.0f);
			colorText = new ColorS(0.9f,0.0f,0.0f);
			break;
		default : break;
		}
	}

	public void Apply() {
		Screen.SetResolution (res.width, res.height, isWindowed);
	}
}
