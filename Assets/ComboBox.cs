using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ComboBox : MonoBehaviour {

	// Use this for initialization
	private string selectedItem="";
	private ArrayList items;
	private bool isEditing = false;
	public GUISkin mySkin;
	public float x;
	public float y;
	public int index;
	public int pos;
	public Listener listener;

	public int itemInit = -1;
	public RectTransform reference;
	public List<string> listElements = new List<string>();


	void Awake() {
		//statsController = stats;
		GameObject s = GameObject.FindWithTag("Listener");
		listener = s.GetComponent<Listener>();
		items = new ArrayList ();



		if(listElements.Count == 0)
		foreach(Resolution res in Screen.resolutions) {
			items.Add(res.width.ToString()+"x"+res.height.ToString());
		}



	}

	void Start() {
		if(itemInit != -1 && itemInit < listElements.Count)
			selectedItem = listElements[itemInit];
	}

	public string getSelect() {
		return selectedItem;
	}

	public void SetSelection(string name) {
		for(int i=0; i< listElements.Count; i++) {
			if(listElements[i].Equals(name)) {
				selectedItem = listElements[i];
				index = i;
			}
		}
	}

	public void SetListener() {

	}

	void Update() {
	
	}

	private void OnGUI() {

		GUI.skin = mySkin;
		if (GUI.Button (GUITools.ResizeGUI (new Rect(x, y +30*pos, 200, 30)), selectedItem)) {
			isEditing = !isEditing;
		}

		if (isEditing) {
			//Special elements for resolution
			if (listElements.Count == 0) {
				for (int i=0; i<items.Count; i++) {
					string item = items [i] as string;
					if (GUI.Button (GUITools.ResizeGUI (new Rect(x, y + 30 * (i + 1 + pos) , 200, 30)), item)) {
						isEditing = false;
						selectedItem = item;
						index = i;
						SettingsData.Settings.res = Screen.resolutions [index];
					}
				}
			}
			else {
				for (int i=0; i<listElements.Count; i++) {
					string item = listElements [i] as string;
					if (GUI.Button (GUITools.ResizeGUI ( new Rect (x, y + 30 * (i + 1 + pos), 200, 30)), item)) {
						isEditing = false;
						selectedItem = item;
						index = i;
						listener.EventAction(this);
					}
				}
			}
		} 
	}
}
