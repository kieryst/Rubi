using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class EnergyBar : MonoBehaviour {
	
	public Transform energyBar;

	// Draw energy bar wrt current energy levels

	void Update () {
		float maxEnergy = GameVars.vars.maxEnergy;
		float currEnergy = RubiControllerScript.rubiControl.GetComponent<RubiEnergy> ().currentEnergy;
		float theScale = 1 - (currEnergy / maxEnergy);
		energyBar.localScale = new Vector3(theScale, 1f, 1f);
	}
}