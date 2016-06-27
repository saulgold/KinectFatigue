using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	

	//Load level
	public void LoadScene(int level) {
		if (level == -1) {
			Application.Quit ();
			return;
		}
		Application.LoadLevel(level);
	}

	public void Send() {
		GameControl.control.SerializeXML();
	}

	public void LoadLevelDifficulty(LevelDataArguments l) {
		l.ActivateLoadingScreen();

		GameControl.control.levelReachedPlayer = l.level + GameControl.control.LEVEL*(GameControl.control.typeSport-1);

		LoadScene (l.scene);
	}

	public void LoadLevelType(LevelDataArguments l) {
		l.ActivateLoadingScreen();
		
		GameControl.control.typeSport = l.level;
		LoadScene (l.scene);
	}


	public void SaveAndLoadLevel(int level) {

		bool isSaved = false;
		if(!GameControl.control.namePlayer.Equals("")) {
			if(!GameControl.control.hasPassed) {
			GameControl.control.ScoreTotal();
			isSaved = GameControl.control.Save (GameControl.control.namePlayer);
			} else isSaved = true;
		}

		if (isSaved) {
			GameControl.control.hasPassed = false;

			KinectManager manager = KinectManager.Instance;
			Destroy(manager.gameObject);
			LoadScene (level);
		}
	}

	public void SaveScore() {

		GameControl.control.ScoreTotal();
	}

	public void LoadGame(string name) {
		GameControl.control.gameName = name;
	}

	public void ApplySettings(int level) {
		SettingsData.Settings.Apply ();
	}

	public void ApplyColor() {

	}
 




}