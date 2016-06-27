using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

//Permits to get a shortcut automatically (just put this script in a scene)
public class ShorcutController : MonoBehaviour {

	// Use this for initialization
	private List<GameObject> bobjects;
	private List<Button> buttons= new List<Button>();
	private PointerEventData pointer;
	private int index = 0;
	private int indexP = 0;
	void Start () {

		bobjects = GameObject.FindGameObjectsWithTag("ThemeButton").ToList();
		OrderLayer();
		foreach(GameObject o in bobjects)
			if(o.GetComponent<Button>() != null)
				buttons.Add(o.GetComponent<Button>());


	}

	void IndexPlus() {
		indexP = index;
		ExecuteEvents.Execute(buttons[indexP].gameObject, pointer, ExecuteEvents.pointerExitHandler);
		index++;
		if(index >= buttons.Count) {
			index = 0;
		}

	}

	void IndexMinus() {
		indexP = index;
		ExecuteEvents.Execute(buttons[indexP].gameObject, pointer, ExecuteEvents.pointerExitHandler);
		index--;
		if(index <0)
			index = buttons.Count - 1;
	}
	
	// Update is called once per frame
	void Update () {
		pointer = new PointerEventData(EventSystem.current);
		if(Input.GetKeyUp("up")) {
			IndexPlus();
		}

		if(Input.GetKeyUp("down"))
			IndexMinus();

		if(Input.GetKeyUp("return")) {
			ExecuteEvents.Execute(buttons[index].gameObject, pointer, ExecuteEvents.submitHandler);
		}
	}
	

	void LateUpdate() {
		if(buttons.Count >0)
			Hover();

	}

	private void OrderLayer() {
		bobjects = bobjects.OrderBy(o=>o.name).ToList();
	}

	void Hover() {

		if(buttons[index] != null)
		ExecuteEvents.Execute(buttons[index].gameObject, pointer, ExecuteEvents.pointerEnterHandler);

	}


}
