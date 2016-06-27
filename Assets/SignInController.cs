using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignInController : MonoBehaviour {

	public Text info;
	public InputField nameLine;
	private string dataBack;
	public Button saveButton;
	// Use this for initialization
	void Start () {
		
	}

	void Result(MonoBehaviour sender) {
		dataBack = GameControl.control.GetResultRequest();
		dataBack = dataBack.Remove(dataBack.LastIndexOf(","));

		if(int.Parse(dataBack)==1) {
			info.color = Color.red;
			info.text  = "Name already taken";
			saveButton.interactable = false;
		} else {
			info.color = Color.green;
			info.text = "Name never used";
			saveButton.interactable = true;
		}
	}

	public void Verify() {
		GameControl.control.SendData("verifyName.php","name",nameLine.text, Result, null);


	}

}
