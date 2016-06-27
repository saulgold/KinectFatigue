using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class StatsController :  Listener {

	// Use this for initialization
	public GameObject PlayerText;
	public GameObject StatsText;
	public Canvas canvas;
	public Font font;
	public PlotManager manager;
	public ComboBox boxSportName;
	private string nameGame;
	public float x;
	public float y;
	public GUISkin mySkin;
	public Vector2 scrollPosition = Vector2.zero;
	private int currentLevel;
	private bool isDisplayed = false;
	public Rect windowRect ;
	public Rect windowRectResult ;
	public bool showWindowResult = false;
	private List<int> levelScore;
	public Button sendButton;
	public Texture checkImage;
	public Texture crossImage;

	private float xNamePos;
	private float yNamePos;

	public Text reference;
	private Vector2 guiScaler;

	private float ratio;
	private float native;
	float centerHeight;
	float centerWidth;

	void Start () {

		native  = 16.0f/9.0f;
		manager.SetPlot(Color.green);
		manager.SetSize(800,400);
	
		windowRect = new Rect(0, 0, 800, 400);
		windowRectResult = new Rect(Screen.width/2-200, Screen.height/2-150, 400, 300);
		currentLevel = 1;

		nameGame =  boxSportName.getSelect();

		sendButton.interactable = GameControl.control.VerificationUpdate();

	}

	override public void EventActionTrue() {
	
	}
	
	override public void EventActionFalse() {

	}

	override public void EventAction(Component compo){}

	public void SendDatas() {
		GameControl.control.UpdateDatasServerSave();

		ShowSendResult();
		sendButton.interactable = false;
	}

	public override void EventAction(MonoBehaviour mono = null) {
		if(boxSportName.Equals(mono)) {
			nameGame = boxSportName.getSelect();
			currentLevel = 1;
		}

	}

	public void ShowSendResult() {
		showWindowResult = !showWindowResult;
	}

	public void SetCurrentLevel(int level) {
		currentLevel = level; 
		levelScore = GameControl.control.GetValuesScoreLevel(nameGame,currentLevel);

		manager.AddValue(levelScore);
	}

	public void DisplayWindow() {
		levelScore = GameControl.control.GetValuesScoreLevel(nameGame,currentLevel);

		manager.AddValue(levelScore);
		isDisplayed = !isDisplayed;

	
	}

	public int GetMaxLevel() {
		if(GameControl.control.VerifyNameGame(nameGame))
			return GameControl.control.GetMaxLevel(nameGame);
		else return 1;
	}

	public int GetCurrentLevel() {
		return currentLevel;
	}

	void CreateText(float x, float y, float w, float h, string message) {
		GameObject name = new GameObject ("name");
		name.transform.SetParent (canvas.transform);
		name.layer = 5;
		
		RectTransform trans = name.AddComponent<RectTransform>();
		
		trans.sizeDelta.Set(w, h);
		trans.anchoredPosition3D = new Vector3(0, 0, 0);
		trans.anchoredPosition = new Vector2(x,y);
		trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		trans.localPosition.Set(0.0f,0.0f,0.0f);
		
		name.AddComponent<CanvasRenderer>();
		
		Text text = name.AddComponent<Text>();
		text.supportRichText = true;
		text.text = message;
		text.fontSize = 14;
		text.font = font;
		text.alignment = TextAnchor.MiddleCenter;
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.color = new Color(1, 1, 1);
	}

	void CreateLabel(float x, float y, float w, float h, string msg) {
		GUI.Label (new Rect (x,y, w, h), msg);
	}

	public int GetSizeSession() {
		if(!GameControl.control.datas.ContainsKey(nameGame)) return 1;
		return GameControl.control.datas[nameGame].levelTime.Count;
	}


	
	// Update is called once per frame
	void Update () {
		
		ratio = (float)Screen.width /Screen.height;
		
		xNamePos = 0.42f*Screen.width;
		yNamePos = 0.36f*Screen.height + (native-ratio)*Screen.height/15;
	}

	void OnGUI() {
		GUI.skin = mySkin;
		GUI.skin.label.fontSize =(int) Math.Round (14 * Screen.width / (800 * 1.0));


		if(GameControl.control != null) {

		if(!GameControl.control.namePlayer.Equals(""))
				CreateLabel (xNamePos , yNamePos, 200, 50, GameControl.control.namePlayer);
			int size = 400;
			if( !nameGame.Equals("") && GameControl.control.datas.ContainsKey(nameGame)) {
				if(GameControl.control.datas[nameGame].levelTime.ContainsKey(currentLevel)) {
					size = GameControl.control.datas[nameGame].levelTime[currentLevel].Count*40;

					if(size < 200) size = 201;
				}
			}
			scrollPosition = GUI.BeginScrollView(GUITools.ResizeGUI(new Rect(x, y, 550, 200)), scrollPosition, GUITools.ResizeGUI( new Rect(0, 0, 1200, size)),true,true);

		if( !nameGame.Equals("") && GameControl.control.isASport(nameGame)) {
			int i=2;
			if(GameControl.control.isALevelSport(nameGame,currentLevel))
				GUI.skin.label.normal.textColor = Color.green;
			else GUI.skin.label.normal.textColor = Color.red;

			CreateLabel(300,0, 200,30,"Level " + currentLevel);
			GUI.skin.label.normal.textColor = Color.white;

			if(GameControl.control.isALevelSport(nameGame,currentLevel)) {
				
				foreach(LevelData value in GameControl.control.datas[nameGame].levelTime[currentLevel]) {
				
						CreateLabel(GUI.skin.label.fontSize, i*GUI.skin.label.fontSize, 200,30,"Level: " + currentLevel);
						CreateLabel(GUI.skin.label.fontSize*7, i*GUI.skin.label.fontSize, 200,30,"Score: "+ value.score.ToString());
						CreateLabel(GUI.skin.label.fontSize*15, i*GUI.skin.label.fontSize, 300,30,"Breathlessness: "+ value.breath.ToString());
						CreateLabel(GUI.skin.label.fontSize*29 ,i*GUI.skin.label.fontSize, 200,30,"Tired: "+ value.tired.ToString());
					i++;
					if(value.hasBeenSent == 1)
							GUI.DrawTexture(new Rect(GUI.skin.label.fontSize*35, i*GUI.skin.label.fontSize-35, 40, 60), checkImage,ScaleMode.ScaleToFit, true, 5.0F);
						else GUI.DrawTexture(new Rect(GUI.skin.label.fontSize*35, i*GUI.skin.label.fontSize-35, 40, 60), crossImage,ScaleMode.ScaleToFit, true, 5.0F);
				}
			}
		}
		GUI.EndScrollView();
		}

		if(isDisplayed) {
			windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
		}

		if(showWindowResult) {
			windowRectResult = GUI.Window(1, windowRectResult, DoMyWindow, "Result");
		}

	}

	void DoMyWindow(int windowID) {
		if(windowID == 0) {
			CreateLabel(300, 20, 300,40,"Graph for level " + currentLevel);

			manager.Draw ();
			GUI.DragWindow();

		}else if(windowID == 1) {
			if(GUI.Button(new Rect(20,windowRectResult.height-50,windowRectResult.width-50,20), "Close")) {
				showWindowResult = false;
			}
			GUI.Label(new Rect(50,100,400,100),GameControl.control.errorMessage);
		}
	}

}
