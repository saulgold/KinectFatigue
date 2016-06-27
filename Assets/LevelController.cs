using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

	// Use this for initialization
	public Button specialExercise;
	void Start () {
		if(GameControl.control != null) {
		specialExercise.interactable = GameControl.control.specialBoxingExercise;
		}
		else specialExercise.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
