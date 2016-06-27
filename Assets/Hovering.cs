using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hovering : MonoBehaviour {

	private float time = 0;
	public float timeMax;
	//private Rect rect;
	private bool isHovering = false;
	public Image bar;
	private Vector3 offset;
	void Start () {
		Time.timeScale = 1.0f;
		offset = new Vector3(40,-5,0);
		bar.color = Color.green;
		bar.fillAmount = 0;
	//	rect = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		bar.transform.position = Input.mousePosition + offset;

		if (isHovering) {

			time += Time.deltaTime;

			bar.fillAmount = time/timeMax;
			if(time >timeMax) {
				time = 0;
				MouseControl.MouseClick();
				bar.fillAmount = time/timeMax;
			}
		} else {
			time = 0;
			bar.fillAmount = time/timeMax;
		}
	}

	public void LaunchTimer() {
		isHovering = true;
	}

	public void Reset() {
		isHovering = false;
		time = 0;
	}

	public void ResetTimer() {

		time = 0;
	}
}
