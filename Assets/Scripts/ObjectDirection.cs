using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectDirection : MonoBehaviour {

	// Use this for initialization
	public GameObject object1;
	public GameObject object2;
	public Text angleText;
	private float angle;
//	private Quaternion angleOffset;
//	private Vector3 dir;

	void Start () {
		//angleOffset = transform.rotation;

		//dir =  (object1.transform.position - object2.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (object2.transform.position);


	}
}
