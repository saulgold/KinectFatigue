using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class SliderControl : MonoBehaviour {



	private Gradient gradient;

	public List<GameObject> objs;
	private List<Slider> sliders;
	private List<Image> images;
	private List<Text> texts;
	private List<Text> infos;
	private List<string[]> infoString;
	private List<bool> touched;


	void Awake() {
		sliders = new List<Slider>();
		touched = new List<bool>();
		images = new List<Image>();
		texts = new List<Text>();
		infos = new List<Text>();
		infoString = new List<string[]>();
		string [] breathString = { 
								"Nothing at all",
						  	  	"Very, very slight",
								"Very Slight",
								"Slight",
								"Moderate",
								"Somewhat Severe",
								"Severe",
								"Severe",
								"very Severe",
								"Very Severe",
								"Very, very Severe",
								"Maximal"
								};

		string [] tiredString = {
								"No exertion at all",
								"Extremely light",
								"Extremely light",
								"Very light",
								"Very light",
								"Light",
								"Light",
								"Somewhat Hard",
								"Somewhat Hard",
								"Hard (heavy)",
								"Hard (heavy)",
								"Very Hard",
								"Very Hard",
								"Extremely Hard",
								"Maximal Exertion"};


		infoString.Add(tiredString);
		infoString.Add(breathString);

		foreach(GameObject s in objs) {
			sliders.Add (s.GetComponentInChildren<Slider>());
			texts.Add(s.GetComponentsInChildren<Text>()[0]);
			images.Add(s.GetComponentsInChildren<Image> ()[1]);
			infos.Add(s.GetComponentsInChildren<Text>()[1]);
			touched.Add(false);
		}
		gradient = new Gradient ();
	
	}

	void Start() {

		GradientColorKey []gck = new GradientColorKey[2];
		gck[0].color = Color.green;
		gck[0].time = 0.0f;

		gck[1].color = Color.red;
		gck[1].time = 1.0f;

		GradientAlphaKey []gak = new GradientAlphaKey[2];
		gak[0].alpha = 1.0f;
		gak[0].time = 0.0f;
		gak[1].alpha = 1.0f;
		gak[1].time = 1.0f;

		gradient.SetKeys (gck, gak);


	}

	public int GetValue(int number) {
		return (int)sliders[number].value - (int)sliders[number].minValue;
	}

	public int GetValueStandard(int number) {
		return (int)sliders[number].value;
	}

	public void SaveResultSlider() {
		if(GameControl.control != null) {
			GameControl.control.SaveDifficulty((int)sliders[0].value);
			GameControl.control.SaveBreath((int)sliders[1].value);
		}
	}

	public void ChangeText(int number) {


		texts[number].text = sliders[number].value.ToString();
		images[number].color = gradient.Evaluate (sliders[number].value/sliders[number].maxValue);
		texts[number].color = images[number].color;
		infos[number].color = images[number].color;
		infos[number].text = infoString[number][(int)sliders[number].value - (int)sliders[number].minValue];

		if(number == 0) {
			if(GameControl.control != null)
				GameControl.control.SaveDifficulty((int)sliders[number].value);
			touched[number] = true;
		}
		if(number == 1) {
			if(GameControl.control != null)
				GameControl.control.SaveBreath((int)sliders[number].value);
			touched[number] = true;

		}
	}

	public bool AllChecked() {
		foreach(bool b in touched) {
			if(!b) return false;
		}
		return true;
	}

	public void ChangeColor(int number) {

		for(int i=0;i<sliders.Count;i++)
			sliders[i].image.color = Color.white;
		sliders[number].image.color = Color.red;
	}
	

	void Update() {

	}


	public void SetValue(int pos, int value) {
		sliders[pos].value = value;
		ChangeText(pos);

	}



}
