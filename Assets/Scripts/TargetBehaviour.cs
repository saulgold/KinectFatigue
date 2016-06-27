using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetBehaviour : MonoBehaviour {

	// Use this for initialization

	void Start () {
		gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) {


		if (other.gameObject.CompareTag (tag)) {
			if (tag == "PunchRight") {
				gameObject.tag = "PunchTouchedRight";
			} 
			if(tag == "PunchLeft"){
				gameObject.tag = "PunchTouchedLeft";
			}

			if (tag == "KneeRight") {
				gameObject.tag = "KneeTouchedRight";
			} 
			if(tag == "KneeLeft"){
				gameObject.tag = "KneeTouchedLeft";
			}
		}
	}
}
