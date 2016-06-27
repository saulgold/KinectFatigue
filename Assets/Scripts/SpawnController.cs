using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour, KinectGestures.GestureListenerInterface, VisualGestureListenerInterface {

	public GameObject target;
	public Text GestureInfo;
	public Text positionPlayerText;
	public SceneControl scene;
	public GameObject particle;
	private GameObject character;

	public AudioClip punchSound;
	private AudioSource source;

	private float volLowRange = 0.8f;
	private float volHighRange = 1.0f;

	private bool start = false;
	private bool hasTouched  = true;

	private float spawnTime = 0.0f;
	private float timerMovementMax = 1.0f;
	private float timerMovement = 0.0f;

	private float timerSpawnMax = 5.0f;
	private float timerSpawn = 0.0f;
	private bool isSpawnable = true;
	public Pause pause;

	private float targetZPos = -0.7f;
	private int falseTouched = 0;
	private CharacterData dataCharacter;
	private InitDataCharacter init;

	public enum Side {
		punchRight,
		punchLeft,
		kneeRight,
		kneeLeft,
		none
	};

	public bool detectionByTraining = false;


	
	private float discreteGestureTime;
	private float continuousGestureTime;

	private Side targetRunning = Side.punchRight;
	private Side movementRunning = Side.none;
	private Side oldTarget = Side.punchRight;

	private float posX = 0.2f;
	private float rotationY = 0.0f;

	private float positionUser = 0.0f;
	private float posXKnee = 0.15f;
	private float distanceHands;
	public CharacterChooser characterChooser;


	void Awake() {
		source = GetComponent<AudioSource> ();
		character = characterChooser.GetCharacter();
		//character = debugCharac;
		init = character.GetComponent<InitDataCharacter>();
		dataCharacter = new CharacterData(init.Hips,
		                                  init.Ribs, 
		                                  init.RightKnee, 
		                                  init.LeftKnee,
		                                  init.LeftShoulder,
		                                  init.RightShoulder,
		                                  init.HipsOffset,
		                                  init.RibsOffset, 
		                                  init.RightKneeOffset, 
		                                  init.LeftKneeOffset,
		                                  init.LeftShoulderOffset,
		                                  init.RightShoulderOffset
		                                  );
		distanceHands = dataCharacter.GetLengthArm();

		//Debug.Log (dataCharacter.Hips.position.x);
	}

	public void Restart() {
		start = false;
	}

	//The different level are defined there
	void Start() {

		targetZPos = -distanceHands - 0.38f;

		if(GameControl.control != null)
			spawnTime = GameControl.control.levelReachedPlayer/GameControl.control.typeSport;
		else{
			timerSpawnMax = 0.1f; 
		}
		Time.timeScale = 1;
		GestureInfo.text = "Waiting user";
		if(GameControl.control!=null && GameControl.control.levelReachedPlayer!=0) {
			if(GameControl.control.levelReachedPlayer >=1 && GameControl.control.levelReachedPlayer <=5) {
				GameControl.control.sportBoxingSetting.knee = false;
				GameControl.control.sportBoxingSetting.punch = true;
				timerSpawnMax /= spawnTime*3;

				targetZPos = -distanceHands -0.38f;

			}
			if(GameControl.control.levelReachedPlayer >=6 && GameControl.control.levelReachedPlayer <=10) {
				GameControl.control.sportBoxingSetting.knee = false;
				GameControl.control.sportBoxingSetting.punch = true;
				timerSpawnMax /= spawnTime*3;
				posX *=-1;
				rotationY = 20.0f;
				targetZPos = -distanceHands -0.35f;
			}
			if(GameControl.control.levelReachedPlayer >=11 && GameControl.control.levelReachedPlayer <=15) {
				GameControl.control.sportBoxingSetting.knee = false;
				GameControl.control.sportBoxingSetting.punch = true;
				targetZPos = -distanceHands - 0.9f;
				timerSpawnMax /= spawnTime*3;
			}
			if(GameControl.control.levelReachedPlayer >=16 && GameControl.control.levelReachedPlayer <=20) {
				GameControl.control.sportBoxingSetting.knee = true;
				GameControl.control.sportBoxingSetting.punch = true;
				timerSpawnMax /= spawnTime*3;
				targetZPos = -distanceHands - 0.9f;
			}
			if(GameControl.control.levelReachedPlayer < 0) {
				targetZPos = -distanceHands -0.38f;
				Debug.Log ("special");
				timerSpawnMax = 5.0f;
				timerSpawnMax /= GameControl.control.sportBoxingSetting.difficulty*3;
			}
		}
		Debug.Log (timerSpawnMax);
	}

	public void HasTouched(Side s) {
	//	Debug.Log ("touched " + s);
	}

	//If kinect is available and if a user is detected it returs true
	private bool isReady() {
		KinectManager manager = KinectManager.Instance;

		return target && manager && manager.IsInitialized () && manager.IsUserDetected ();
		
	}

	private bool isUserPlaced() {
		return  positionUser > 2.0f;
	}

	public float GetPositionUserZ() {
		return positionUser;
	}

	private void UpdatePosition() {
		KinectManager manager = KinectManager.Instance;
		if(manager && manager.IsInitialized () && manager.IsUserDetected ()) {
			positionUser = manager.GetUserPosition(manager.GetPrimaryUserID()).z;
		}
	}
	
	void Update () 
	{

		if (Input.GetKeyDown ("escape"))
			pause.PauseGame ();


		UpdatePosition();
		if(!start) {
			if(isUserPlaced()) positionPlayerText.color = Color.green;
			else positionPlayerText.color = Color.red;
			positionPlayerText.text = "Distance must be 2m ( " +positionUser.ToString("F2") +" )";

		}
		if (isReady () && start) {

			if(isUserPlaced()) {
				if(!scene.GetIsRunning()) {
					scene.StartTimer();
				}
				positionPlayerText.text = "";
			if (movementRunning != Side.none) {
				timerMovement += Time.deltaTime;
				if (timerMovement >= timerMovementMax) {
					timerMovement = 0;
					movementRunning = Side.none;
					
				}
			}


			timerSpawn += Time.deltaTime;
			if(timerSpawn >= timerSpawnMax) {
				isSpawnable = true;
				timerSpawn = 0;
			}

			if (hasTouched && isSpawnable) {
				isSpawnable = false;
				Color pl = new Color(1.0f,0.0f,0.0f,0.5f);
				Color pr = new Color(0.0f,1.0f,0.0f,0.5f);
				Color kl = new Color(0.0f,1.0f,1.0f,0.5f);
				Color kr = new Color(0.0f,0.0f,1.0f,0.5f);
				if(GameControl.control != null) {
						ColorBlind colorBlind = GameControl.control.GetColorBlind();
						pl = colorBlind.pl.getColor(0.5f);
						pr = colorBlind.pr.getColor(0.5f);
						kl = colorBlind.kl.getColor(0.5f);
						kr = colorBlind.kr.getColor(0.5f);
				}
				hasTouched = false;
				targetRunning = ChoiceTarget();
				switch (targetRunning) {
				case Side.punchLeft:
					SpawnTarget (-posX, dataCharacter.LeftShoulder.y + dataCharacter.LeftShoulderOffset.y, 
						             targetZPos,"PunchRight", pl,rotationY );
					break;

				case Side.punchRight:
						SpawnTarget (posX, dataCharacter.RightShoulder.y + dataCharacter.RightShoulderOffset.y, 
						             targetZPos,"PunchLeft",pr, -rotationY);
					break;

				case Side.kneeLeft:
					SpawnTarget (-posXKnee, dataCharacter.LeftKnee.y + dataCharacter.LeftKneeOffset.y, 
						             -0.6f, "KneeRight", kl, rotationY);
					break;

				case Side.kneeRight:
						SpawnTarget (posXKnee,dataCharacter.RightKnee.y + dataCharacter.RightKneeOffset.y,
						             -0.6f, "KneeLeft", kr, -rotationY);
					break;
				
				default:
					break;
				
				}
			}
			} else {
				if(scene.GetIsRunning())
					scene.StopTimer();
				positionPlayerText.color = Color.red;
				positionPlayerText.text = "Distance must be 2m ( " +positionUser.ToString("F2") +" )";
			}
		} 

	}

	/// <summary>
	/// Choices the target.
	/// </summary>
	/// <returns>The target.</returns>
	private Side ChoiceTarget() {
		Side targetRunning = oldTarget;
		bool knee = false;
		bool punch = true;
		if(GameControl.control != null) {
			knee = GameControl.control.sportBoxingSetting.knee;
			punch = GameControl.control.sportBoxingSetting.punch;
		}

		float r = Random.value;
		if(!punch) r = 0.7f;
		if(!knee) r = 0.2f;
		 
		if(punch && r <0.5) {
			if(oldTarget == Side.kneeLeft || oldTarget == Side.kneeRight || oldTarget == Side.none) {
				if(Random.value <0.5) targetRunning = Side.punchRight;
				else targetRunning = Side.punchLeft;
			} else if(oldTarget == Side.punchRight) targetRunning = Side.punchLeft;
			else targetRunning = Side.punchRight;
		}

		else if(knee && r>0.5f) {
			if(oldTarget == Side.punchLeft || oldTarget == Side.punchRight || oldTarget == Side.none) {
				if(Random.value <0.5) targetRunning = Side.kneeRight;
				else targetRunning = Side.kneeLeft;
			} else if(oldTarget == Side.kneeRight) targetRunning = Side.kneeLeft;
			else targetRunning = Side.kneeRight;
		}
		GestureInfo.text = targetRunning.ToString();
		return targetRunning;
	}

	void LateUpdate() {

		if (movementRunning != Side.none) {
			var sensedObjectsRight = GameObject.FindGameObjectsWithTag ("PunchTouchedRight");
		
			foreach (GameObject deadObject in sensedObjectsRight) { 
				if (movementRunning == Side.punchRight  && isUserPlaced()) {
					DesactivateObject (deadObject);		

				} else {
					deadObject.tag = "PunchRight";
					falseTouched++;
				}
			
			}
		
			var sensedObjectsLeft = GameObject.FindGameObjectsWithTag ("PunchTouchedLeft");
		
			foreach (GameObject deadObject in sensedObjectsLeft) {
				if (movementRunning == Side.punchLeft  && isUserPlaced()) {
					DesactivateObject (deadObject);		
				} else {
					deadObject.tag = "PunchLeft";
					falseTouched++;
				}
			}

			var sensedObjectsKneeRight = GameObject.FindGameObjectsWithTag ("KneeTouchedRight");
			
			foreach (GameObject deadObject in sensedObjectsKneeRight) { 
				if (movementRunning == Side.kneeRight && isUserPlaced()) {
					DesactivateObject (deadObject);		
					
				} else {
					deadObject.tag = "KneeRight";
				}
				
			}
			
			var sensedObjectsKneeLeft = GameObject.FindGameObjectsWithTag ("KneeTouchedLeft");
			
			foreach (GameObject deadObject in sensedObjectsKneeLeft ) {
				if (movementRunning == Side.kneeLeft && isUserPlaced()) {
					DesactivateObject (deadObject);		
				} else {
					deadObject.tag = "KneeLeft";
				}
			}
		}
	}
	/// <summary>
	/// Desactivates the object which are tagged .
	/// </summary>
	/// <param name="deadObject">Dead object.</param>
	private void DesactivateObject(GameObject deadObject) {
		float vol = Random.Range (volLowRange, volHighRange);
		source.PlayOneShot (punchSound, vol);
		Color c = deadObject.GetComponent<Renderer>().material.color;
		deadObject.SetActive(false);
		timerSpawn = 0;
		if(targetRunning != Side.none)
			oldTarget = targetRunning;

		targetRunning = Side.none;
		if(GameControl.control != null && GameControl.control.isRandom)
			oldTarget = Side.none;
		//targetRunning = Side.none;
		GestureInfo.text = string.Empty;
		movementRunning = Side.none;
		Destroy(deadObject);
		hasTouched = true;
		scene.scoreIncrease ();
		particle.SetActive (true);
		particle.GetComponent<ParticleSystem>().startColor = c;
		Instantiate(particle,deadObject.transform.position,Quaternion.identity);

	}



	
	/// <summary>
	/// Spawns the target.
	/// </summary>
	/// <param name="positionX">Position x.</param>
	/// <param name="positionY">Position y.</param>
	/// <param name="positionZ">Position z.</param>
	/// <param name="tag">Tag.</param>
	/// <param name="c">C.</param>
	/// <param name="rotationZ">Rotation z.</param>
	private void SpawnTarget(float positionX, float positionY, float positionZ, 
	                         string tag, Color c, float rotationZ) {

		Vector3 spawnPos = new Vector3(positionX, 
		                               positionY, 
		                               positionZ);
			
		GameObject t = Instantiate(target, spawnPos, Quaternion.identity) as GameObject;
		t.transform.Rotate(0, rotationZ, 0);
		t.tag = tag;
		t.SetActive(true);

		t.GetComponent<Renderer> ().material.color = c;
		//hasTouched = false;

	}


	
	public void UserDetected(long userId, int userIndex)
	{
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;

		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
		manager.DetectGesture(userId, KinectGestures.Gestures.Tpose);
		manager.DetectGesture(userId, KinectGestures.Gestures.PunchRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.PunchLeft);
	
		
	if(GestureInfo != null)
		{
			//GestureInfo.text = "PunchLeft or PunchRight";
		}
	}
	
	public void UserLost(long userId, int userIndex)
	{
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
			scene.LaunchTimer();
			isGood = true;
			//Debug.Log ("start");
		}

		if (sGestureText.Contains ("Tpose") && isGood == false) {
			pause.PauseGame();
		}

		if (!detectionByTraining) {

			if (sGestureText.Contains ("PunchRight")) {
				movementRunning = Side.punchLeft;
				timerMovement = 0;
				isGood = true;
			}

			if (sGestureText.Contains ("PunchLeft")) {
				movementRunning = Side.punchRight;
				timerMovement = 0;
				isGood = true;

			}

			if (!isGood){
				//GestureInfo.text = "Bad Movement";
			}
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
		if(GestureInfo != null)
		{
//			string sGestureText = string.Format ("{0} {1:F0}%", gesture, progress * 100f);
			//continuousInfo.GetComponent<GUIText>().text = sGestureText;
			//GestureInfo.text = sGestureText;
		//	continuousGestureDisplayed = true;
		//	continuousGestureTime = Time.realtimeSinceStartup;
		}
	}
	
	public bool GestureCompleted(long userId, int userIndex, string gesture, float confidence)
	{
		if (GestureInfo != null) {
			string sGestureText = gesture;
			//discreteInfo.GetComponent<GUIText>().text = sGestureText;

			if (detectionByTraining) {
				//GestureInfo.text = sGestureText;

				bool isGood = false;

			
				if (sGestureText.Contains ("punch_Right")) {
					movementRunning = Side.punchLeft;
					timerMovement = 0;
					isGood = true;
				}
			
				if (sGestureText.Contains ("punch_Left")) {
					movementRunning = Side.punchRight;
					timerMovement = 0;
					isGood = true;
		
				}

				if (sGestureText.Contains ("Knee_Right")) {
					movementRunning = Side.kneeLeft;
					timerMovement = 0;
					isGood = true;
				}
				
				if (sGestureText.Contains ("Knee_Left")) {
					movementRunning = Side.kneeRight;
					timerMovement = 0;
					isGood = true;
					
				}
			
				if (!isGood) {
					//GestureInfo.text = "Bad Movement";
				}

				//discreteGestureDisplayed = true;
				//discreteGestureTime = Time.realtimeSinceStartup;
			}
		}
		
		// reset the gesture
		return true;
	}



}
