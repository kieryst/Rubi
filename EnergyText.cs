using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyText : MonoBehaviour {

	Text energyText;
	
	void Start () {
		energyText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		energyText.text = RubiControllerScript.rubiControl.GetComponent<RubiEnergy> ().currentEnergy.ToString ();
	}
}