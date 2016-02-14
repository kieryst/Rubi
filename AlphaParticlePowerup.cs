using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaParticlePowerup : MonoBehaviour {
	
	private bool collided = false;
	private float displayTime;
	Image image;
	Text textformatting;
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
		
			GameVars.vars.haveAlphaParticle = true;

			Time.timeScale = 0;
			RubiControllerScript.rubiControl.enabled = false;
			displayTime = Time.realtimeSinceStartup + 3f;

			PopupController.pop.popupText.text = "A lpha Particle Obtained!";
			PopupController.pop.DisplayPopup();

			collided = true;
		}
	}
	void Update () {
		if (collided) {
			if (Time.realtimeSinceStartup >= displayTime) {
				Time.timeScale = 1;
				RubiControllerScript.rubiControl.enabled = true;
				PopupController.pop.popupText.text = "";
				PopupController.pop.HidePopup();

				collided = false;

				Destroy (gameObject);
			}
		}
	}
}
