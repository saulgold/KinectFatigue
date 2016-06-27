using UnityEngine;
using System.Collections;

public class LevelDataArguments : MonoBehaviour {

	public int scene = 4;
	public int level;
	public GameObject loadingScreen;

	public void ActivateLoadingScreen() {
		if(loadingScreen != null)
			loadingScreen.SetActive(true);
	}
}
