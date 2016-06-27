using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour {

	public List<GameObject> maps;
	void Awake () {

		if(GameControl.control != null) {
			foreach(GameObject map in maps)
				if(map.name.Equals(GameControl.control.nameMap))
					map.SetActive(true);
			else map.SetActive(false);
		} else maps[1].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
