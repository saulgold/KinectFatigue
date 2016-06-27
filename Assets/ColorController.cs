using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ColorController : MonoBehaviour {

	// Use this for initialization

	private List<Button> buttons = new List<Button>();
	private List<Text> texts = new List<Text>();


	void Awake () {
		Init();
	}

	public void Init() {
		buttons.Clear();
		List<GameObject> os = GameObject.FindGameObjectsWithTag("ThemeButton").ToList();
		
		foreach(GameObject o in os) buttons.Add(o.GetComponent<Button>());
		
		List<GameObject> osT = GameObject.FindGameObjectsWithTag("ThemeText").ToList();
		foreach(GameObject o in osT) texts.Add(o.GetComponent<Text>());
		ChangeTheme();
	}
	
	public void ChangeTheme() {

		foreach(Button button in buttons) {
			if(button != null) {
			ColorBlock c = button.colors;
			
			ColorS co = GameControl.control.colorTheme.button;
			c.normalColor = new Color(co.r, co.g, co.b);
			button.colors = c;
			}
		}

		foreach(Text text in texts) {
			ColorS c = GameControl.control.colorTheme.text;
			text.color = new Color(c.r, c.g, c.b);
		}
	}
}
