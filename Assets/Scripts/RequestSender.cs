using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RequestSender : MonoBehaviour{

	private string _url = "http://www.localhost/test";
	private string dataBack;




	public void SendData(string url, string var, string data) {

		WWWForm form = new WWWForm();
		form.AddField(var,data);


		WWW www = new WWW(_url+"/"+url, form);
		
		StartCoroutine(WaitForRequest(www));
	}

	 

	

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		if (www.error == null) {
			Debug.Log("ok");
		} else {
			Debug.Log("error");
		}    
	}    


}
