using UnityEngine;
using System.Collections;

public class ChangeViewStatsClick : MonoBehaviour {

	public StatsController stats;
	private int currentLevel;

	void Start() {
		currentLevel = 1;
	}

	public void DisplayWindow() {
		stats.DisplayWindow();
	}

	public void ChangeView(int i) {
		currentLevel = stats.GetCurrentLevel();
		if(i < 0) {
			if(currentLevel+i >0) {
				currentLevel+=i;
				stats.SetCurrentLevel(currentLevel);
			}
		} else {
			if(currentLevel+i <= stats.GetMaxLevel()) {
				currentLevel+=i;
				stats.SetCurrentLevel(currentLevel);
			}
		}
	}
}
