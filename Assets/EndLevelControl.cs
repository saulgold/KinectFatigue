using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class EndLevelControl : MonoBehaviour {


	public InteractionManager interact;
	private bool isStopped = false;
	private bool old = true;
	public SliderControl sliderControl;
	private SpeechManager speechManager;

	private int value;

	private List<GameObject> panels;
	private int index = 0;
	public bool activateSpeechRecognition = true;
	public List<string[]> infoString;
	public GUISkin mySkin;
	public Vector2 scrollPosition = Vector2.zero;


	void Start () {
		if( KinectManager.Instance != null) {
			KinectManager.Instance.displayColorMap = false;
			KinectManager.Instance.displayUserMap = false;
			KinectManager.Instance.displaySkeletonLines = false;

			KinectManager.Instance.DisplayMapsWidthPercent = 0;
		}
		panels = GameObject.FindGameObjectsWithTag("Panel").ToList();

		if(GameControl.control != null) {
			GameControl.control.SaveDifficulty(6);
			GameControl.control.SaveBreath(0);
		}


		OrderLayer();

		foreach(GameObject panel in panels) {
			panel.SetActive(false);
		}
		
		panels[0].SetActive(true);

		infoString = new List<string[]>();
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
		

		
		
		infoString.Add(tiredString);
		infoString.Add(breathString);


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

	void OnGUI() {
		GUI.skin = mySkin;
		float size = infoString[index].Length*50+200;

		if(size < Screen.height/2) size = Screen.height/2+1;

		scrollPosition.y = (int)Math.Abs (sliderControl.GetValue(index)*40);
		GUI.skin.label.fontSize =(int) Math.Round (18 * Screen.width / (800 * 1.0));
		scrollPosition = GUI.BeginScrollView(GUITools.ResizeGUI(new  Rect(400, 60, 300, 400)), 
		                                     scrollPosition,
		                                     new Rect(0, 0, Screen.width*0.8f, size),false,false);


		for(int i = 0; i < infoString[index].Length;i++) {
			if(sliderControl.GetValue(index) == i) {
				GUI.skin.label.normal.textColor = Color.red;
			}

			GUI.Label (new Rect (10,100 + i*40, 600, 30), infoString[index][i]);
			GUI.skin.label.normal.textColor = Color.white;

		}
		GUI.EndScrollView();
	}



	void Update () {

		if(Input.GetKeyDown("escape"))
			isStopped =! isStopped;

		if (isStopped == old) {
			interact.gameObject.SetActive(!isStopped);
			old = !isStopped;
		}
	}

	void LateUpdate() {
		if(activateSpeechRecognition)
			RecognitionAction();
	}

	void RecognitionAction() {

		if(speechManager == null) {
			speechManager = SpeechManager.Instance;
		}
		
		if(speechManager != null && speechManager.IsSapiInitialized()) {


			if(speechManager.IsPhraseRecognized()) {
				string sPhraseTag =""; 
				sPhraseTag = speechManager.GetPhraseTagRecognized();
				Debug.Log (sPhraseTag);

				for(int i=0;i<=20;i++) {
					int n;
					if(int.TryParse(sPhraseTag, out n)) {
						if(n == i) {
							sliderControl.SetValue(index,i);

						}
					}
				}
				speechManager.ClearPhraseRecognized();
			}
		}
	}


}
