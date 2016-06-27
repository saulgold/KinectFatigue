using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndLevelController : MonoBehaviour {

	// Use this for initialization
	public Text title;
	void Start () {
		if(GameControl.control!=null &&  GameControl.control.levelReachedPlayer>0)
			title.text = "You have finished the level " + GameControl.control.levelReachedPlayer.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
