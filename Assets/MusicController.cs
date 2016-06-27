using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;

[Serializable]
public struct MusicScene {
	public string name;
	public AudioClip clip;
}

public class MusicController : MonoBehaviour {

	// Use this for initialization
	public static MusicController instance;


	public MusicScene[] musics;
	void Awake() {

		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

	}

	public AudioClip GetMusic(string name) {
		foreach(MusicScene m in musics) {
			if(m.name.Equals(name)) {
				return m.clip;
			}
		}
		return null;

	}
}
