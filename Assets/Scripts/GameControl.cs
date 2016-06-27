using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;



//Redefine Dictionnary to be serializable
[Serializable]
	[XmlRoot("Dictionary")]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable {
		private const string DefaultTagItem = "Item";
		private const string DefaultTagKey = "Key";
		private const string DefaultTagValue = "Value";
		private static readonly XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
		private static readonly XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));
		
		public SerializableDictionary() : base(){}
		
		protected SerializableDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context){
		}
		
		protected virtual string ItemTagName {
			get { return DefaultTagItem; }
		}
		
		protected virtual string KeyTagName {
			get { return DefaultTagKey; }
		}
		
		protected virtual string ValueTagName {
			get { return DefaultTagValue; }
		}
		
		public XmlSchema GetSchema() {
			return null;
		}
		
		public void ReadXml(XmlReader reader) {
			bool wasEmpty = reader.IsEmptyElement;
			
			reader.Read();
			
			if (wasEmpty) {
				return;
			}
			
			try {
				while (reader.NodeType != XmlNodeType.EndElement) {
					reader.ReadStartElement(this.ItemTagName);
					try {
						TKey tKey;
						TValue tValue;
						
						reader.ReadStartElement(this.KeyTagName);
						try {
							tKey = (TKey)KeySerializer.Deserialize(reader);
						}
						finally {
							reader.ReadEndElement();
						}
						
						reader.ReadStartElement(this.ValueTagName);
						try {
							tValue = (TValue)ValueSerializer.Deserialize(reader);
						}
						finally {
							reader.ReadEndElement();
						}
						
						this.Add(tKey, tValue);
					}
					finally {
						reader.ReadEndElement();
					}
					
					reader.MoveToContent();
				}
			}
			finally {
				reader.ReadEndElement();
			}
		}
		
		public void WriteXml(XmlWriter writer) {
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this) {
				writer.WriteStartElement(this.ItemTagName);
				try {
					writer.WriteStartElement(this.KeyTagName);
					try {
						KeySerializer.Serialize(writer, keyValuePair.Key);
					}
					finally {
						writer.WriteEndElement();
					}
					
					writer.WriteStartElement(this.ValueTagName);
					try {
						ValueSerializer.Serialize(writer, keyValuePair.Value);
					}
					finally {
						writer.WriteEndElement();
					}
				}
				finally {
					writer.WriteEndElement();
				}
			}
		}
	}


//Contains all about the end of level
[Serializable]
public class LevelData {
	public int score;
	public int tired;
	public int breath;
	public int hasBeenSent;

	public LevelData() {
		
	}
	
	public LevelData(int s, int d, int b) {
		score = s;
		tired = d;
		breath = b;
		hasBeenSent = 0;
	}
}

//Settings of a sport
public class SportBoxingSetting {
	public bool knee = true;
	public bool punch = true;
	public int difficulty = 1; 

	public SportBoxingSetting() {}


}

//Color (float) instead of Color type which is not serializable
[Serializable]
public class ColorS {
	public float r;
	public float g;
	public float b;

	public ColorS() {}

	public ColorS(float r, float g, float b) {
		this.r = r;
		this.g = g;
		this.b = b;
	}

	public Color getColor(float a) {
		return new Color(r,g,b,a);
	}

	public Color getColor() {
		return new Color(r,g,b);
	}
}


[Serializable]
public class ColorTheme {
	public ColorS button;
	public ColorS text;

	public ColorTheme() {
		button = new ColorS(0.0f,0.6f,0.25f);
		text = new ColorS(0.0f,0.6f,0.25f);
	}

}

//It controls all the data of the game
//The flow with the server
//The save and load
public class GameControl : MonoBehaviour {
	
	// Use this for initialization
	public static GameControl control;
	
	public string namePlayer = "";
	public SerializableDictionary <string,PlayerData> datas  = new SerializableDictionary<string, PlayerData>();
	public string gameName = "";
	public int levelReachedPlayer;
	private string valueToSend;
	public string errorMessage;
	private string _url = "localhost/test";
	private string dataBack;
	public delegate void MyDelegate(Listener sender);
	public MyDelegate myDelegate;
	private const string message = "Datas have been sent";
	public SportBoxingSetting sportBoxingSetting;
	public ColorTheme colorTheme = new ColorTheme();
	public bool specialBoxingExercise = false;
	public int typeSport;
	public bool hasPassed = false;
	public bool isRandom = false;
	public bool nameDatabase = false;
	public string timeUpdated;
	public bool isConnected = true;
	public string ColorBlindName="Color1";
	//The number of level for each type
	public int LEVEL = 5;
	public string nameMap;
	public string nameCharacter;

	public Dictionary<string, ColorBlind> colors = new Dictionary<string, ColorBlind>();

	void Awake () {
		colors.Add("Color1", new ColorBlind(new ColorS(1.0f,0.0f,0.0f),
		                                    new ColorS(0.0f,1.0f,0.0f),
		                                    new ColorS(0.0f,1.0f,1.0f),
		                                    new ColorS(0.0f,0.0f,1.0f)));

		colors.Add("Color2", new ColorBlind(new ColorS(1.0f,0.0f,0.0f),
		                                    new ColorS(1.0f,1.0f,0.0f),
		                                    new ColorS(1.0f,1.0f,1.0f),
		                                    new ColorS(1.0f,0.0f,1.0f)));

		colors.Add("Color3", new ColorBlind(new ColorS(0.0f,0.0f,0.0f),
		                                    new ColorS(1.0f,0.0f,0.0f),
		                                    new ColorS(1.0f,1.0f,1.0f),
		                                    new ColorS(0.0f,0.0f,1.0f)));


		nameMap = "Hills";
		nameCharacter = "Robot";
		levelReachedPlayer = 0;
		sportBoxingSetting = new SportBoxingSetting();
		//LoadColor();
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}
	}

	public ColorBlind GetColorBlind() {
		return colors[ColorBlindName];
	}

	public List<string> getListColorBlind() {
		List<string> s = new List<string>();
		foreach(KeyValuePair<string,ColorBlind> entry in colors) {
			s.Add(entry.Key);
		}
		return s;
	}

	public void TestConnection(Listener l) {
		SendData("TestConnection.php","","",TestConnectionResult, l); 
	}

	private void TestConnectionResult(Listener l) {
		if(!isConnected) l.EventActionFalse();
		else l.EventActionTrue();
	}

	public void SaveColor(ColorS button, ColorS text) {
		colorTheme.button = button;
		colorTheme.text = text;
	}

	//Verify if all the data have have been sent
	public bool VerificationUpdate() {

		SerializeXML();
		if(!isConnectedToServer()) return false;

		SendData("verify.php","xml",valueToSend, UpdateDataWithDataBase, null);

		foreach(KeyValuePair<string, PlayerData> data in datas) {
			foreach(KeyValuePair<int, List<LevelData>> listLevel in data.Value.levelTime) {
				foreach(LevelData l in listLevel.Value) {
					if(l.hasBeenSent == 0) return true;
				}
			}
		}
		return false;
	}



	//Read a configuration file to get address of the server
	public void ConfigRead(string name) {
		XmlDocument xmlDoc = new XmlDocument();
		TextAsset configFile = Resources.Load(name) as TextAsset;

		if(configFile) {

			xmlDoc.LoadXml(configFile.text);
			XmlNode network = xmlDoc.SelectNodes("Game/Network")[0];
			_url = network.SelectSingleNode("IP").InnerText.Replace("\n", "");

			Debug.Log ("url " + _url);

		} else {
			Debug.Log ("No config file");
			_url = "localhost/test";
		}
	}

	public string GetResultRequest() {
		return dataBack;
	}

	public int CompareDate(DateTime t) {
		DateTime time = DateTime.ParseExact(timeUpdated, "yyyy-MM-dd HH:mm:ss",
		                                    System.Globalization.CultureInfo.InvariantCulture);
		return DateTime.Compare(t, time);
	}

	// Get some values with this format (0,1,1,0) if it's a 0 the data is not in the server, and it will display a red cross
	public void UpdateDataWithDataBase(Listener sender = null) {

		if(isConnectedToServer()) {

			dataBack = dataBack.Remove(dataBack.LastIndexOf(","));

			string[] TagIds = dataBack.Split(',');
			List<string> id = new List<string>(TagIds);
			List<int> a = new List<int>();
			foreach(string i in id) a.Add(Int32.Parse(i));
				UpdateData(a);
		}

	}

	//Get all score for a particular sport and level
	public List<int> GetValuesScoreLevel(string sport, int level) {
		List<int> values = new List<int>();
		if(datas.ContainsKey(sport)  ) {
			if(datas[sport].levelTime.ContainsKey(level)) {
				foreach(LevelData l in datas[sport].levelTime[level]) {
					values.Add(l.score);
				}
			}
		}
		return values;
	}

	public bool isALevelSport(string nameGame, int level) {
		return datas[nameGame].levelTime.ContainsKey(level);
	}

	public void UpdateData(List<int> values) {

		int i=0;

		foreach(KeyValuePair<string, PlayerData> data in datas) {
			foreach(KeyValuePair<int, List<LevelData>> listLevel in data.Value.levelTime) {
				foreach(LevelData l in listLevel.Value) {
					l.hasBeenSent = values[i];
					i++;
				}
			}
		}
	}

	//Formula to get a score total
	public void ScoreTotal() {
		int score = datas[gameName].levelTime [levelReachedPlayer][GetCurrentSession()].score;

		int tired = datas[gameName].levelTime [levelReachedPlayer][GetCurrentSession()].tired;
		int breath = datas[gameName].levelTime [levelReachedPlayer][GetCurrentSession()].breath;
		score = (score*15)/((tired)+(breath));


		datas[gameName].levelTime [levelReachedPlayer][GetCurrentSession()].score = score;
	}


	public void UpdateData(int value) {
		Debug.Log("update");
		foreach(KeyValuePair<string, PlayerData> data in datas) {
			foreach(KeyValuePair<int, List<LevelData>> listLevel in data.Value.levelTime) {
				foreach(LevelData l in listLevel.Value) {
					l.hasBeenSent = value;
				}
			}
		}
	}
	//Save the data in a .dat file (no need of name)
	public bool Save() {
		timeUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		
		Debug.Log ("Save " + Application.persistentDataPath);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/"+namePlayer+".dat");
		GameData gameData = new GameData (namePlayer, datas, timeUpdated, colorTheme, nameMap, nameCharacter,ColorBlindName);
		bf.Serialize (file, gameData);
		file.Close ();
		return true;
		
	}

	public bool isASport(string nameGame) {
		return datas.ContainsKey(nameGame);
	}

	//Save the data in a .dat file
	public bool Save(string name) {
		timeUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

		Debug.Log ("Save " + Application.persistentDataPath);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/"+name+".dat");
		GameData gameData = new GameData (namePlayer, datas, timeUpdated, colorTheme, nameMap, nameCharacter,ColorBlindName);
		bf.Serialize (file, gameData);
		file.Close ();
		return true;
		
	}


	public int GetCurrentSession() {
		return datas[gameName].levelTime [levelReachedPlayer].Count-1;
	}


	//Serialize the data to a XML file to send it to the server
	public void SerializeXML() {
		GameData gameData = new GameData (namePlayer, datas, timeUpdated, colorTheme, nameMap, nameCharacter,ColorBlindName);
		XmlSerializer xs = new XmlSerializer(typeof(GameData));
		using (StreamWriter wr = new StreamWriter(Application.persistentDataPath + "/" +"games.xml")) {
			xs.Serialize(wr, gameData);
		}
		
		string line = "";
		valueToSend = "";
		using (StreamReader sr = new StreamReader(Application.persistentDataPath + "/" +"games.xml")){
			while ((line = sr.ReadLine()) != null) {
				valueToSend += line;
			}
		}
	}


	/// <summary>
	/// Sends the data.
	/// </summary>
	/// <param name="urlFile">URL file.</param>
	/// <param name="var">Variable.</param>
	/// <param name="data">Data.</param>
	/// <param name="myDelegate">My delegate (the function will be caled at the end)</param>
	/// <param name="sender">Sender.</param>
	public void SendData(string urlFile, string var, string data, MyDelegate myDelegate, Listener sender) {
		
		WWWForm form = new WWWForm();
		form.AddField(var,data);
		
		string path = _url+"/"+urlFile;

		Debug.Log (path);
		WWW www = new WWW(path, form);
		
		StartCoroutine(WaitForRequest(www,myDelegate, sender));
	}

	public void Send() {
		SerializeXML();
		SendData("index.php","xml",valueToSend, null, null);

		if(errorMessage.Equals(message)) {
			UpdateData(1);
		} 
		Save(namePlayer);
	}

	public bool isConnectedToServer() {
		return errorMessage.Equals(message);
	}

	public int GetMaxLevel(string name) {
		return datas[name].levelTime.Keys.Max();
	}

	public bool VerifyNameGame(string name) {
		return datas.ContainsKey(name);
	}
	/// <summary>
	/// Waits for request.
	/// </summary>
	/// <returns>The for request.</returns>
	/// <param name="www">Www.</param>
	/// <param name="myDelegate">My delegate.</param>
	/// <param name="sender">Sender.</param>
	IEnumerator WaitForRequest(WWW www, MyDelegate myDelegate, Listener sender)
	{

		yield return www;
		if (www.error == null) {
			errorMessage = message;
			dataBack = www.text;
			isConnected = true;

		} else {
			isConnected = false;
			errorMessage = "Error " + www.error;
		}
		if(myDelegate != null) {
			myDelegate(sender);
		}
	}    
	
	public bool VerifyNameFile(string name) {
		return File.Exists (Application.persistentDataPath + "/" + name + ".dat");
	}
	/// <summary>
	/// Sends the name to the database.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="name">Name.</param>
	public void SendNameDatabase(Listener sender,  string name) {
		SendData("verifyName.php","name",name, VerifyNameDatabase, sender);
	}

	public void GetDataServer(string name, Listener l) {
		SendData("Datas.php","name",name, SaveDataFromServer, l);
		namePlayer = name;
	}

	//Get all data from server in a XML file
	//Read the XML file
	public void SaveDataFromServer(Listener sender = null) {
		XmlDocument xmlDoc = new XmlDocument();

		xmlDoc.LoadXml(dataBack);
		datas  = new SerializableDictionary<string, PlayerData>();
		XmlNode dateNode = xmlDoc.SelectNodes("Games")[0];
		string date = dateNode.SelectSingleNode("Date").InnerText;
		timeUpdated = date;

		foreach(XmlNode sportNode in xmlDoc.SelectNodes("Games/Game")) {
			string sport = sportNode.Attributes.GetNamedItem("name").Value;
			datas.Add(sport, new PlayerData(1,new SerializableDictionary<int,List<LevelData>>()));

			foreach(XmlNode level in sportNode.SelectNodes("Level")) {
				int id = Convert.ToInt32(level.Attributes.GetNamedItem("id").Value);
				datas[sport].levelTime.Add(id, new List<LevelData>());

				foreach(XmlNode session in level.SelectNodes("Session")) {
					//int sessionId = Convert.ToInt32(session.Attributes.GetNamedItem("id").Value);
					int rpe = Convert.ToInt32(session.SelectSingleNode("RPE").InnerText);
					int score = Convert.ToInt32(session.SelectSingleNode("Score").InnerText);
					int breathScore = Convert.ToInt32(session.SelectSingleNode("BreathScore").InnerText);

					datas[sport].levelTime[id].Add(new LevelData(score, rpe, breathScore));
				}
			}

		}
		Debug.Log("Finish");
		sender.EventAction();
	}

	public void VerifyNameDatabase(Listener sender = null) {

		if(dataBack == "1,") sender.EventActionTrue();

		else if(!isConnectedToServer() || dataBack != "1,") sender.EventActionFalse();

	}

	public bool isLevel() {
		return levelReachedPlayer > 0;
	}
	/// <summary>
	/// Load the specified name.
	/// </summary>
	/// <param name="name">Name.</param>
	public bool Load(string name) {
		if (File.Exists (Application.persistentDataPath + "/" + name + ".dat")) {
			Debug.Log(Application.persistentDataPath);

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + name + ".dat", FileMode.Open);

			GameData data = (GameData)bf.Deserialize (file);
			namePlayer = data.namePlayer;
			timeUpdated = data.timeUpdated;
			nameMap = data.nameMap;
			ColorBlindName = data.ColorBlindName;
			if(data.nameCharacter != null)
				nameCharacter = data.nameCharacter;

			if(data.colorTheme != null) {

				colorTheme = data.colorTheme;
			}

			datas = data.datas;
			file.Close();
			return true;
		} 
		return false;
	}

	public void UpdateDatasServerSave() {

		Send();
		Save(namePlayer);
	}

	public void PrepareDatas() {
		if(!datas.ContainsKey(gameName)) {
			datas.Add(gameName,new PlayerData(levelReachedPlayer,new SerializableDictionary<int,List<LevelData>>()));
		}
	}
	

	public void SaveDifficulty(int diff) {
		if(datas[gameName].levelTime.ContainsKey(levelReachedPlayer))
			datas[gameName].levelTime [levelReachedPlayer][GetCurrentSession()].tired = diff;
	}

	public void SaveBreath(int breath) {
		if(datas[gameName].levelTime.ContainsKey(levelReachedPlayer))
			datas[gameName].levelTime [levelReachedPlayer][GetCurrentSession()].breath = breath;
	}

	public void AddDataLevel(int score) {
		LevelData p = new LevelData (score, 0, 0);
		List<LevelData> existing;

		if (!datas[gameName].levelTime.TryGetValue(levelReachedPlayer, out existing)) {
			existing = new List<LevelData>();
			existing.Add (p);
			datas[gameName].levelTime[levelReachedPlayer] = existing;
		} else {
			datas[gameName].levelTime[levelReachedPlayer].Add(p);
		}
	}
}





[Serializable]
public class PlayerData {

	public int levelReachedPlayer;
	public SerializableDictionary <int,List<LevelData>> levelTime;

	public PlayerData() {}

	public PlayerData(int level, SerializableDictionary<int,List<LevelData>> dic) {
		levelReachedPlayer = level;
		levelTime = dic;
	}
}



[Serializable]
public class GameData {

	public string namePlayer;
	public string timeUpdated;
	public string nameMap;
	public SerializableDictionary<string,PlayerData> datas;
	public ColorTheme colorTheme;
	public string nameCharacter;
	public string ColorBlindName;

	public GameData() {
		datas = new SerializableDictionary<string,PlayerData>();
	}

	public GameData(string name, SerializableDictionary<string,PlayerData> datas,
	                string timeUpdated, ColorTheme colorTheme, 
	                string nameMap, string nameCharacter, string ColorBlindName) {
		namePlayer = name;
		this.datas = datas;
		this.timeUpdated = timeUpdated;
		this.colorTheme = colorTheme;
		this.nameMap = nameMap;
		this.nameCharacter = nameCharacter;
		this.ColorBlindName = ColorBlindName;
	}
	
}

