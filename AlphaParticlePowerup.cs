using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaParticlePowerup : MonoBehaviour {
	
	private bool collided = false;
	private float displayTime;
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
	
			//Pause the game for a short fanfare
			GameCamera.gameCamera.pseudoPause = true;
			GameVars.vars.haveAlphaParticle = true;
			displayTime = Time.realtimeSinceStartup + 3f;
			PopupController.pop.DisplayPopup("A lpha Particle Obtained!");
			Destroy (gameObject.GetComponent<SpriteRenderer>());
			collided = true;
		}
	}
	void Update () {
		if (collided) {
			if (Time.realtimeSinceStartup >= displayTime) {
				GameCamera.gameCamera.pseudoPause = false;
				PopupController.pop.HidePopup();
				Destroy (gameObject);
			}
		}
	}
}
