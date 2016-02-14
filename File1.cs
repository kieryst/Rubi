using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine.UI;

public class File1 : MonoBehaviour {

	public Object arrow;
	public GameObject newArrow;
	public Canvas canvas;
	private bool found = false;
	private bool loadGame = false;
	private bool newGame = false;

	public string fileName;
	public Text file1Text;
	public Text file1Health;
	public Text file1Energy;
	public Image file1HealthImg;
	public Image file1EnergyImg;

	void Start () {
		if (File.Exists (Application.persistentDataPath + fileName)) {

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close ();

			int hours = Mathf.FloorToInt (data.elapsedTime/3600f);
			int minutes = Mathf.FloorToInt ((data.elapsedTime - (hours * 3600)) / 60f);
			int seconds = Mathf.FloorToInt (data.elapsedTime - (hours * 3600f) - (minutes * 60f));

			string niceTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

			file1Text.text = niceTime;
			file1Health.text = data.currentHealth.ToString () + " / " + data.maxHealth.ToString ();
			file1Energy.text = data.maxEnergy.ToString ();
			found = true;
		} else {
			file1Text.text = "Empty File";
			file1Health.text = "";
			file1Energy.text = "";
			file1HealthImg.color = new Color(1f, 1f, 1f, 0f);
			file1EnergyImg.color = new Color(1f, 1f, 1f, 0f);
		}
	}
	
	void FixedUpdate () {
		if (loadGame) {
				Application.LoadLevel ("Level 1");
		}

		if (newGame) {
				Application.LoadLevel ("Level 1");
		}
	}

	public void LoadGame () {
		GameVars.vars.fileName = fileName;
		if (found) {
			GameVars.vars.Load ();
			loadGame = true;
		} else {
			GameVars.vars.ResetVars ();
			newGame = true;
		}
	}

	public void onHighlight () {
		newArrow = Instantiate (arrow, new Vector3 (GetComponent<RectTransform> ().position.x - 90, GetComponent<RectTransform> ().position.y, GetComponent<RectTransform> ().position.z), Quaternion.identity) as GameObject;
		newArrow.transform.SetParent (canvas.transform);
	}
	public void onAway () {
		Destroy (newArrow);
	}

}
