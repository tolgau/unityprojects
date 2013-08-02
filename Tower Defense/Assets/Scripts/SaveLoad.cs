using UnityEngine; 
using System.Collections; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 
using System;

public class SaveLoad: MonoBehaviour { 
	string fileLocation; 
	string fileNameGameData;
	public GameObject levelManager;
	PlayerData playerData;
	LevelData levelData;

	void Start () { 
		fileLocation = Application.dataPath; 
		fileNameGameData = "GameData.xml";
	} 
	
	void SaveGameProgress(GameScript gameScript){
		playerData = gameScript.playerData; 
		string data = SerializeObject(playerData);
		CreateXML(fileNameGameData, data); 
	}
	
	bool LoadGameProgress(GameScript gameScript){
		string data = LoadXML(fileNameGameData); 
		if(data.ToString() != "") 
		{ 
			Type dataType = playerData.GetType();
			playerData = (PlayerData)DeserializeObject(data, dataType); 
			gameScript.playerData = playerData;
			return true;
		} 
		else
		{
			return false;
		}
	}	
	
	void LoadLevel(int level){
		string data = LoadXML("Level" + level + ".xml"); 
		if(data.ToString() != "") 
		{ 
			Type dataType = levelData.GetType();
			levelData = (LevelData)DeserializeObject(data, dataType);
			GameObject tempLevMan = (GameObject)Instantiate(levelManager);
			tempLevMan.GetComponent<LevelScript>().BuildMapUsingLevelData(levelData);
		} 
	}
	
	string UTF8ByteArrayToString(byte[] characters) 
	{      
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString(characters); 
		return (constructedString); 
	} 
 
	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(pXmlString); 
		return byteArray; 
	} 
	
	// Here we serialize our UserData object of myData 
	string SerializeObject(object pObject) 
	{ 
		Type dataType = pObject.GetType();
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		XmlSerializer xs = new XmlSerializer(dataType); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
	
	// Here we deserialize it back into its original form 
	object DeserializeObject(string pXmlizedString, Type dataType) 
	{ 
		XmlSerializer xs = new XmlSerializer(dataType); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(xmlTextWriter.BaseStream); 
	} 
	
	// Finally our save and load methods for the file itself 
	void CreateXML(string fileName, string data) 
	{ 
		StreamWriter writer; 
		FileInfo t = new FileInfo(fileLocation+"\\"+ fileName); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete(); 
			writer = t.CreateText(); 
		} 
		writer.Write(data); 
		writer.Close(); 
	} 
	
	string LoadXML(string fileName) 
	{ 
		StreamReader r = File.OpenText(fileLocation+"\\"+ fileName); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		return _info;
	} 
} 
