using UnityEngine;
using System.Collections;

public class ToggleBox : Component{

	private string[] options;
	private Rect position;
	private int selected;
	private int lastSelected;
	private Listener listener;

	public ToggleBox(Listener l, string[] options, Rect position, int selected) {
		this.options = options;
		this.selected = selected;
		this.position = position;
		lastSelected = selected;
		listener = l;
	}

	public int GetSelection() {
		return selected;
	}

	public void Draw() {
		selected = GUI.SelectionGrid(position, selected, options, options.Length, GUI.skin.toggle);
		if(lastSelected != selected) {
			listener.EventAction(this);
			lastSelected = selected;
		}
	}
}
