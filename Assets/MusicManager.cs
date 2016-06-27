using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	// Use this for initialization

	private AudioClip clip;
	public AudioClip clip2;
	private GameObject music;
	void Start() {
		LoadMusic();
		music = GameObject.Find("Music");
		if (music.GetComponent<AudioSource> ().clip != clip) {
			music.gameObject.GetComponent<AudioSource> ().clip = clip;
			if(clip != null)
				music.gameObject.GetComponent<AudioSource> ().Play ();
		}
	}

	void LoadMusic() {

		string name = Application.loadedLevelName;
		clip = MusicController.instance.GetMusic(name);
		if(clip == null && clip2 != null) clip = clip2;
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
