using UnityEngine;
using System.Collections;
using  System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using UnityEngine.UI;

	public class HeartRateController : MonoBehaviour
	{

	// Use this for initialization
	private FacetrackingManager manager;
	private KinectManager kinectManager;
	private KinectInterop.SensorData sensorData = null;
	private StringBuilder csv = null;
	private float time;
	private bool isRunning = true;
	private string path;
	private int result;
	private bool hasFinished = false;
	private float timeApplication = 0;
	private bool isOver = false;
	public Text timeDisplay;
	private int decreaseCounter;
	private const int timeMax = 30;
	private float decreasedTimer;
	public Text resultText;

	void Start () {
		resultText.text = "Tracking face ..."; 
		decreaseCounter = timeMax;
		decreasedTimer = timeMax;
		Time.timeScale = 1.0F;
		time = 0;
		csv = new StringBuilder ();
		kinectManager = KinectManager.Instance;


		InitFile();
		InitKinect();
	}

	void InitKinect() {
	
		kinectManager.displayColorMap = true;
	}

	private void InitFile() {
		//FindApplication();

		path = Application.persistentDataPath +"/" + "NormalHeartRate.csv";
		                   

		UnityEngine.Debug.Log(path);
		//string header = "nAlpha,nRed,nGreen,nBlue,nIr\n";
		string header = "nMillisecondsElapsed, nRed,nGreen, nBlue, nIr\n";
		csv.Append(header);
	}

	private void LaunchApplication(string name) {
		Process wunder = new Process();
		wunder.StartInfo.FileName = name;
		wunder.Start();
		wunder.WaitForExit();
	}
	
	private void FindApplication() {
		string dirPath = @"./HeartCalculator/".Replace("\\","/");
		UnityEngine.Debug.Log(dirPath);

		DirectoryInfo dir = new DirectoryInfo(dirPath);
		FileInfo[] info = dir.GetFiles("*.*");
		foreach (FileInfo f in info) {
			if(f.Extension.Equals(".exe")) {
				LaunchApplication(f.FullName);
			}
		}

	}

	private void GetResultBPM() {
		try {
		string text = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + "Result.txt");
			result = (int)float.Parse(text.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);
		
		}catch {
			result = -1;

		}
		UnityEngine.Debug.Log ("Result is" + result);

		kinectManager.displayColorMap = false;
		resultText.text = result.ToString() + " " + "BPM";
	}
	
	private void WriteResult() {
		File.WriteAllText(path, csv.ToString());
		FindApplication();
	}

	private bool GetProcessNameFinished(string name) {
		var procList = Process.GetProcesses();

		for (int i = 0; i < procList.Length; i++) {
			try {
				string Name = procList[i].ProcessName;
				if(Name.Equals(name)) return false;
			} catch {}
		}
		return true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(hasFinished && isOver == false) {
			timeApplication += Time.deltaTime;
			if(timeApplication > 3) {
				isOver = true;
				GetResultBPM();
			}
		}


		// get the face-tracking manager instance
		if (manager == null) {
			manager = FacetrackingManager.Instance;
		}

		if (kinectManager && kinectManager.IsInitialized ()) {
			sensorData = kinectManager.GetSensorData ();
		}
		
		if (manager && manager.IsTrackingFace ()) {
			if (isRunning) {
				resultText.text = "Processing ...";
				time += Time.deltaTime;
				decreasedTimer -= Time.deltaTime;
				if(decreasedTimer <= decreaseCounter) {

					timeDisplay.gameObject.SetActive(true);
					
					timeDisplay.text = decreaseCounter.ToString() + " !";

					decreaseCounter--;
					
				}
	
				Rect face = manager.GetFaceColorRect (manager.GetFaceTrackingID ());
				

				float size = face.width * face.height;

				float alpha = 0;
				float red = 0;
				float green = 0;
				float blue = 0;
				float ir = 0;

				if(size != 0 ) {

					for (int i = (int)face.x; i < face.x + face.width; i++) {
						for (int j = (int)face.y; j < face.y + face.height; j++) {

							if(i < sensorData.colorImageWidth && j < sensorData.colorImageHeight) {
								red += sensorData.colorImageTexture.GetPixel (i, j).r;
								green += sensorData.colorImageTexture.GetPixel (i, j).g;
								blue += sensorData.colorImageTexture.GetPixel (i, j).b;
								alpha += sensorData.colorImageTexture.GetPixel (i, j).a;
//								UnityEngine.Debug.Log (sensorData.infraredImage.Length);

								if(KinectManager.Instance.inframedReady && sensorData.infraredImage != null) {
								//	Debug.Log (sensorData.infraredImage.Length);
								//	Debug.Log (sensorData.colorImageWidth*sensorData.colorImageHeight);

									int dx = i;
									int dy = j;
									
									int pos = dx + dy * sensorData.depthImageWidth;
									if(pos < sensorData.infraredImage.Length)
										ir += sensorData.infraredImage[pos];
								}

						
							}

						}
					}

				
					float avg_alpha = alpha / size;
					float avg_red = red / size;
					float avg_green = green / size;
					float avg_blue = blue / size;
					float avg_ir = ir / size;
				

					double std_alpha = 0;
					double	 std_red = 0;
					double std_green = 0;
					double std_blue = 0;
					double std_ir = 0;

					
					double var_alpha = 0;
					double var_red = 0;
					double var_green = 0;
					double var_blue = 0;
					double var_ir = 0;

					for (int i = (int)face.x; i < face.x + face.width; i++) {
						for (int j = (int)face.y; j < face.y + face.height; j++) {
							if( i < sensorData.colorImageWidth && j < sensorData.colorImageHeight) { 
								Color c = sensorData.colorImageTexture.GetPixel(i,j);
							
							
								//if(KinectManager.Instance.inframedReady && sensorData.infraredImage != null) {

									var_blue = (c.b - avg_blue);
									std_blue += System.Math.Pow(var_blue, 2.0);
									
									var_green = (c.g - avg_green);
									std_green += Math.Pow(var_green, 2);
									
									var_red = (c.r - avg_red);
									std_red += Math.Pow(var_red, 2);
									
									var_alpha = (c.a - avg_alpha);
									std_alpha += Math.Pow(var_alpha, 2);

								if(KinectManager.Instance.inframedReady && sensorData.infraredImage != null) {
									//	Debug.Log (sensorData.infraredImage.Length);
									//	Debug.Log (sensorData.colorImageWidth*sensorData.colorImageHeight);
									
									int dx = i;
									int dy = j;
									
									int pos = dx + dy * sensorData.depthImageWidth;
									if(pos < sensorData.infraredImage.Length)
										var_ir = sensorData.infraredImage[pos] - avg_ir;
									else var_ir = avg_ir;
									std_ir += Math.Pow(var_ir, 2);
								}

							}					
							
						}
					}


					std_alpha = Math.Sqrt(std_alpha / size);
					std_red = Math.Sqrt(std_red / size);
					std_green = Math.Sqrt(std_green / size);
					std_blue = Math.Sqrt(std_blue / size);
					std_ir = Math.Sqrt(std_ir / size);



					std_ir /= ushort.MaxValue;

					var newLine = string.Format (new System.Globalization.CultureInfo("en-GB"),"{0},{1},{2},{3},{4}{5}", 
					                             Mathf.Round (time*1000), 
					                             std_red.ToString(), 
					                             std_green.ToString(), 
					                             std_blue.ToString(),
					                             std_ir.ToString(),
					                             "\n");
					csv.Append (newLine); 
				//	Debug.Log(time);
					if (time > timeMax) {
						isRunning = false;
						hasFinished = true;
						WriteResult();
					}
				}
			}
			
		}
	}
}
