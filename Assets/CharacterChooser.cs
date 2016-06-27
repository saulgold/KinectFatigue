using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterChooser : MonoBehaviour {

	public List<GameObject> characters;
	private GameObject charac;
	void Awake () {
		
		if(GameControl.control != null) {
			foreach(GameObject character in characters)
				if(character.name.Equals(GameControl.control.nameCharacter)) {
					character.SetActive(true);
					charac = character;
			}
			else character.SetActive(false);
		} else {
			charac = characters[0];
			characters[0].SetActive(true);
			characters[1].SetActive(false);
		}
	}

	public GameObject GetCharacter() {
		return charac;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
