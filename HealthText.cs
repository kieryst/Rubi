using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthText : MonoBehaviour {

	Text healthText;
	
	void Start () {
		healthText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		healthText.text = GameVars.vars.currentHealth.ToString();
	}
}