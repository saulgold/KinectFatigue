using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeatUpDownController : MonoBehaviour, KinectGestures.GestureListenerInterface, VisualGestureListenerInterface {
	
	public GameObject target;
	public Text GestureInfo;
	public SceneControl scene;
	public GameObject particle;
	public GameObject character;
	
	public AudioClip punchSound;
	private AudioSource source;
	
	private float volLowRange = 0.8f;
	private float volHighRange = 1.0f;
	
	private bool start = false;
	private bool hasTouched  = false;
	private bool hasTouchedLeft = false;
	private bool hasTouchedRight = false;
	
	

	
	private float timerSpawnMax = 5.0f;

	private float timerChangeMovementMax = 5.0f;
	private float timerChangeMovement = 0.0f;


	private bool isSpawnable = true;
	public Pause pause;

	private bool isSeated = false;
	private bool begin = false;
	private bool nextMoveAvailable = false;

	public Text timeSameMovement;

//	private float timeMovementSeat = 0.0f;
//	private float timeMovementSeatMax = 5.0f;

//	private float timeMovementHit = 0.0f;
//	private float timeMovementHitMax = 5.0f;

	
	public enum Side {
		right,
		left,
		none
	};

	public enum Seat {
		up,
		down,
		none
	}
	
	public bool detectionByTraining = false;
	
	private float discreteGestureTime;
	private float continuousGestureTime;
	
	private Seat movement = Seat.down;
	
	void Awake() {
		source = GetComponent<AudioSource> ();
	}
	
	public void Restart() {
		start = false;
	}
	
	void Start() {
		Time.timeScale = 1;
		GestureInfo.text = "Waiting user";
		if(GameControl.control != null)
		if(GameControl.control.levelReachedPlayer!=0)
			timerSpawnMax /= GameControl.control.levelReachedPlayer*3;
	}
	
	public void HasTouched(Side s) {
		Debug.Log ("touched " + s);
	}
	
	private bool isReady() {
		KinectManager manager = KinectManager.Instance;
		return target && manager && manager.IsInitialized () && manager.IsUserDetected ();
	}
	
	
	void Update () {
		
		if (Input.GetKeyDown ("escape"))
			pause.PauseGame ();

		if (isReady () && start) {

			if(begin) {
				GestureInfo.color = Color.red;
				GestureInfo.text = "Seat";
				begin = false;
			}
			timerChangeMovement += Time.deltaTime;

		/*	if(isSeated) {
				timeMovementSeat +=Time.deltaTime;
			//	timeSameMovement.text = timeMovementSeat.ToString("F2");
			} else {
				timeMovementSeat = 0.0f;
			}*/
			
			if(timerChangeMovement >= timerChangeMovementMax) {
				timerChangeMovement = 0.0f;
				nextMoveAvailable = true;
			}

			if(movement == Seat.up && hasTouched && nextMoveAvailable) {
				Debug.Log("hit");
				movement = Seat.down;
				GestureInfo.color = Color.red;
				GestureInfo.text = "Seat";
				isSeated = false;
				nextMoveAvailable = false;
				hasTouched = false;
			}


			if(movement == Seat.down && isSeated && nextMoveAvailable) {
				Debug.Log("seat");
				isSpawnable = true;
				movement = Seat.up;
				GestureInfo.color = Color.red;
				GestureInfo.text = "Hit";
				nextMoveAvailable = false;
			}

			
			if (movement == Seat.up && isSpawnable && !hasTouched) {
				SpawnTarget (-0.3f, -0.2f,"PunchRight", new Color(1.0f,0.0f,0.0f,0.5f));
				SpawnTarget (0.2f, 0.3f,"PunchLeft", new Color(0.0f,1.0f,0.0f,0.5f));
				isSpawnable = false;

			}
		}
	}
	
	void LateUpdate() {
		var sensedObjectsRight = GameObject.FindGameObjectsWithTag ("PunchTouchedRight");

		foreach (GameObject deadObject in sensedObjectsRight) { 
			DesactivateObject (deadObject);		
			hasTouchedRight = true;
		}
			
		var sensedObjectsLeft = GameObject.FindGameObjectsWithTag ("PunchTouchedLeft");
			
		foreach (GameObject deadObject in sensedObjectsLeft) {
			DesactivateObject (deadObject);		
			hasTouchedLeft = true;

		}
		if(hasTouchedLeft && hasTouchedRight) {
			hasTouched = true;
			hasTouchedLeft = false;
			hasTouchedRight = false;
			GestureInfo.color = Color.green;
			GestureInfo.text = "Hit";
		}


	}
	
	private void DesactivateObject(GameObject deadObject) {
		float vol = Random.Range (volLowRange, volHighRange);
		source.PlayOneShot (punchSound, vol);
		Color c = deadObject.GetComponent<Renderer>().material.color;
		deadObject.SetActive(false);

		scene.scoreIncrease ();
		particle.SetActive (true);
		particle.GetComponent<ParticleSystem>().startColor = c;
		Instantiate(particle,deadObject.transform.position,Quaternion.identity);
	}
	
	
	private void SpawnTarget(float positionMin,float positionMax, string tag, Color c) {
	
		float addXPos = Random.Range(positionMin, positionMax);
		Vector3 spawnPos = new Vector3(addXPos, 
		                               character.transform.position.y + 2.2f, 
		                               character.transform.position.z);
		
		GameObject t = Instantiate(target, spawnPos, Quaternion.identity) as GameObject;
		t.tag = tag;
		t.SetActive(true);
		
		t.GetComponent<Renderer> ().material.color = c;
	}
	
	public void UserDetected(long userId, int userIndex) {
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;
		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);

		manager.DetectGesture(userId, KinectGestures.Gestures.Tpose);
	}
	
	public void UserLost(long userId, int userIndex) {
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;
		
		
	}
	
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{
		// the gestures are allowed for the primary user only
		
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;
		
		
	}
	
	
	public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                             KinectInterop.JointType joint, Vector3 screenPos)
	{
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return false;
		
		bool isGood = false;
		
		string sGestureText = gesture.ToString();
		if(!detectionByTraining)
			GestureInfo.text = sGestureText;
		
		if (!start && sGestureText.Contains ("RaiseRightHand")) {
			start = sGestureText.Contains ("RaiseRightHand");
			Debug.Log ("start" + start);
			scene.LaunchTimer();
			isGood = true;
			begin = true;
		}
		
		if (sGestureText.Contains ("Tpose") && isGood == false) {
			pause.PauseGame();
		}
		
	
	
	
		
		return true;
	}
	
	public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                             KinectInterop.JointType joint)
	{
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return false;
		
		
		return true;
	}
	
	public void GestureInProgress(long userId, int userIndex, string gesture, float progress)
	{

	}
	
	public bool GestureCompleted(long userId, int userIndex, string gesture, float confidence)
	{
		if (GestureInfo != null) {
			string sGestureText = gesture;
			//discreteInfo.GetComponent<GUIText>().text = sGestureText;
			
			if (detectionByTraining) {
				

				if (sGestureText.Contains ("Seated")) {


					isSeated = true;
					if(GestureInfo.text.Equals("Seat")) {
						GestureInfo.color = Color.green;
						GestureInfo.text = sGestureText;
					}

				} else {
					isSeated = false;
				}
			}
		}
		
		// reset the gesture
		return true;
	}
	
	
	
}
