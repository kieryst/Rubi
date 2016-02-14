using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;


public class GameVars : MonoBehaviour {

	// This keeps track of all the 'savable' variables within the game. Also handles saving/loading of files.

	public static GameVars vars = new GameVars();

	public string fileName;
	public float elapsedTime;

	// Saved Location
	public string currentLevel;
	public float currentX;
	public float currentY;

	// Saved Rubi Stats
	public int maxHealth;
	public int currentHealth;
	public int maxEnergy;
	public bool haveGraviton;
	public bool haveNeutrinoPhase;
	public bool haveQuartz;

	// Saved Consumables
	public bool[] healthPickups = new bool[2] ;

	// Saved Events
	public bool boss1;
	public bool mith1;
	public bool rubiAirStrange;

	// Singleton Vars.
	void Awake () {
		if (vars == null) {
			DontDestroyOnLoad (gameObject);
			vars = this;
		} else if (vars != this) {
			Destroy(gameObject);
		}
	}

	public GameVars () {
		fileName = "";
		elapsedTime = 0f;
		currentLevel = "Level 1";
		currentX = 70f;
		currentY = -5.6f;
		currentHealth = 10;
		maxHealth = 10;
		maxEnergy = 10;
		haveGraviton = false;
		haveNeutrinoPhase = false;
		haveQuartz = false;
		healthPickups [0] = false;
		healthPickups [1] = false;
		boss1 = false;
		mith1 = false;
		rubiAirStrange = false;
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + fileName);

		PlayerData data = new PlayerData ();
		data.fileName = fileName;
		data.elapsedTime = elapsedTime;
		data.currentLevel = currentLevel;
		data.currentX = currentX;
		data.currentY = currentY;
		data.maxHealth = maxHealth;
		data.currentHealth = currentHealth;
		data.maxEnergy = maxEnergy;
		data.haveGraviton = haveGraviton;
		data.haveNeutrinoPhase = haveNeutrinoPhase;
		data.haveQuartz = haveQuartz;
		data.healthPickups = healthPickups;
		data.boss1 = boss1;
		data.mith1 = mith1;
		data.rubiAirStrange = rubiAirStrange;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
        if (File.Exists (Application.persistentDataPath + fileName)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close ();

			fileName = data.fileName;
			elapsedTime = data.elapsedTime;
			currentLevel = data.currentLevel;
			currentX = data.currentX;
			currentY = data.currentY;
			maxHealth = data.maxHealth;
			currentHealth = data.currentHealth;
			maxEnergy = data.maxEnergy;
			haveGraviton = data.haveGraviton;
			haveNeutrinoPhase = data.haveNeutrinoPhase;
			haveQuartz = data.haveQuartz;
			healthPickups = data.healthPickups;
			boss1 = data.boss1;
			mith1 = data.mith1;
			rubiAirStrange = data.rubiAirStrange;
		}
	}

	public void ResetVars()
	{
		elapsedTime = 0f;
		currentLevel = "Level 1";
		currentX = 70f;
		currentY = -5.6f;
		currentHealth = 10;
		maxHealth = 10;
		maxEnergy = 10;
		haveGraviton = false;
		haveQuartz = false;
		healthPickups [0] = false;
		healthPickups [1] = false;
		boss1 = false;
		mith1 = false;
		rubiAirStrange = false;
	}
}

[System.Serializable]
class PlayerData
{
	public string fileName;
	public float elapsedTime;
	public string currentLevel;
	public float currentX;
	public float currentY;
	public int maxHealth;
	public int currentHealth;
	public int maxEnergy;
	public bool haveGraviton;
	public bool haveNeutrinoPhase;
	public bool haveQuartz;
	public bool[] healthPickups = new bool[2];
	public bool boss1;
	public bool mith1;
	public bool rubiAirStrange;
}