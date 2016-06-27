using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class SettingController : Listener {
	
	private List<GameObject> panels;
	private int index;

	private ToggleBox targetOptions;
	private ToggleBox activateOptions;
	private ToggleBox randomOption;

	public GUISkin mySkin;

	public ComboBox colorBox;
	public ComboBox mapBox;
	public ComboBox characterBox;

	public ComboBox colorBlindBox;

	private string colorSelected = "";
	public Slider timeSlider;
	public Text valueSlider;


	public RawImage mapImage;
	public RawImage characterImage;

	//public RawImage characterImage;
	private Dictionary<string, Texture2D> maps;
	private Dictionary<string, Texture2D> characters;
	public List<RawImage> images;

	//public ColorController colorController;
	// Use this for initialization
	void Start () {
		if(GameControl.control != null)
			colorBlindBox.listElements = GameControl.control.getListColorBlind();
		maps = new Dictionary<string, Texture2D>();
		maps.Add("Lunar", new Texture2D(20,20));
		maps.Add("Hills", new Texture2D(20,20));

		foreach(KeyValuePair<string, Texture2D> map in maps) {
			TextAsset t = Resources.Load(map.Key+".png") as TextAsset;
			maps[map.Key].LoadImage(t.bytes);

			mapImage.texture = maps[map.Key];

		}

		characters = new Dictionary<string, Texture2D>();
		characters.Add("Robot", new Texture2D(20,20));
		characters.Add("Lola", new Texture2D(20,20));
		
		foreach(KeyValuePair<string, Texture2D> character in characters) {
			TextAsset t = Resources.Load(character.Key+".png") as TextAsset;
			characters[character.Key].LoadImage(t.bytes);
			
			characterImage.texture = characters[character.Key];
			
		}

		randomOption = new ToggleBox(this,new string[] { "Random", "not Random" },
		new Rect(Screen.width*0.4f, Screen.height*0.4f, 500, 50),1);

		targetOptions = new ToggleBox(this,new string[] { "Punch only", "Knee only", "Both" },
									new Rect(Screen.width*0.3f, Screen.height*0.5f, 500, 50),2);
		index = 0;

		activateOptions = new ToggleBox(this,new string[] { "Yes", "No" }, 
										new Rect(Screen.width*0.35f, Screen.height*0.3f, 300, 40),1);

		timeSlider.gameObject.SetActive(false);
		valueSlider.gameObject.SetActive(false);
		panels = GameObject.FindGameObjectsWithTag("Panel").ToList();
		OrderLayer();

		foreach(GameObject panel in panels) {
			panel.SetActive(false);
		}

		panels[index].SetActive(true);

		colorBox.SetSelection(SettingsData.Settings.themeName);
		ChangeColorBlind(GameControl.control.ColorBlindName);
		mapBox.SetSelection(GameControl.control.nameMap);
		colorBlindBox.SetSelection(GameControl.control.ColorBlindName);
		characterBox.SetSelection(GameControl.control.nameCharacter);
		valueSlider.text = timeSlider.value.ToString();
		mapImage.texture = maps[GameControl.control.nameMap];
		characterImage.texture = characters[GameControl.control.nameCharacter];
	}

	override public void EventActionTrue() {

	}
	
	override public void EventActionFalse() {
	
	}

	public void ApplyMap() {

	}

	override public void EventAction(Component compo){
		if(randomOption.Equals(compo)) {
			if(randomOption.GetSelection() == 1) {
				GameControl.control.isRandom = false;
			} else {
				GameControl.control.isRandom = true;
			}
		}
		if(activateOptions.Equals(compo)) {
			if(activateOptions.GetSelection() == 0) {
				timeSlider.gameObject.SetActive(true);
				valueSlider.gameObject.SetActive(true);
				targetOptions.Draw();
				GameControl.control.specialBoxingExercise = true;
			} else {
				GameControl.control.specialBoxingExercise = false;
				timeSlider.gameObject.SetActive(false);
				valueSlider.gameObject.SetActive(false);
				
			}
		}

	}

	private void ChangeColor() {
		SettingsData.Settings.SetColors(colorSelected);
		GameControl.control.Save(GameControl.control.namePlayer);
	}

	private void ChangeColorBlind(string name) {
		if(GameControl.control != null) {
			GameControl.control.ColorBlindName = name;

			images[0].color = GameControl.control.colors[name].pl.getColor();
			images[1].color = GameControl.control.colors[name].pr.getColor();
			images[2].color = GameControl.control.colors[name].kl.getColor();
			images[3].color = GameControl.control.colors[name].kr.getColor();
		}
			


	}

	public override void EventAction(MonoBehaviour mono = null) {
		if(colorBox.Equals(mono)) {
			colorSelected = colorBox.getSelect();
			ChangeColor();
		}

		if(mapBox.Equals(mono)) {
			GameControl.control.nameMap = mapBox.getSelect();
			mapImage.texture = maps[GameControl.control.nameMap];
			//mapImage.texture = 
		}

		if(characterBox.Equals(mono)) {
			GameControl.control.nameCharacter = characterBox.getSelect();
			characterImage.texture = characters[GameControl.control.nameCharacter];
		}

		if(colorBlindBox.Equals(mono)) {
			ChangeColorBlind(colorBlindBox.getSelect());
		}


	}

	private void OrderLayer() {
		panels = panels.OrderBy(o=>o.name).ToList();
	}

	public void IndexPlus() {
		index++;
		if(index >= panels.Count) index = panels.Count - 1;
		Activate();
	}

	public void IndexMinus() {
		index--;
		if(index < 0) index = 0;
		Activate();
	}

	private void Activate() {
		foreach(GameObject panel in panels) {
			panel.SetActive(false);
		}
		panels[index].SetActive(true);
	}

	public void Save() {
		GameControl.control.Save();
	}

	void OnGUI() {

		GUI.skin = mySkin;
		if(index == 1) {
			activateOptions.Draw();
			if(activateOptions.GetSelection() == 0) {
				targetOptions.Draw();
			}
		} else if(index == 2) {

			randomOption.Draw();


		}
	}

	public void ChangeDifficulty() {
		valueSlider.text = timeSlider.value.ToString();
		GameControl.control.sportBoxingSetting.difficulty = (int)timeSlider.value;
	}

	public void ApplySportBoxing() {
		int selected = targetOptions.GetSelection();
		if(selected == 2) {
			GameControl.control.sportBoxingSetting.knee = true;
			GameControl.control.sportBoxingSetting.punch = true;
		}
		if(selected == 1) {
			GameControl.control.sportBoxingSetting.knee = true;
			GameControl.control.sportBoxingSetting.punch = false;
		}
		if(selected == 0) {
			GameControl.control.sportBoxingSetting.knee = false;
			GameControl.control.sportBoxingSetting.punch = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
