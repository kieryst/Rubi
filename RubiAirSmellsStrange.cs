using UnityEngine;
using System.Collections;

public class RubiAirSmellsStrange : MonoBehaviour {

	public bool rubiAirStrangeTriggered = false;
	private bool rubiAirStrangeInit = false;
	
	DialogItems[] items;
	
	int currDialog = 0;
	
	void Start () {
		items = new DialogItems[2];
		items [0] = new DialogItems ();
		items [0].dialog = "The air smells strange up here...";
		items [0].portrait = "rubi_hmm_portrait";
		items [1] = new DialogItems ();
		items [1].dialog = "... It's heavy.";
		items [1].portrait = "rubi_hmm_portrait";
	}

	void Update () {
		if (rubiAirStrangeTriggered && !rubiAirStrangeInit) {
			if (RubiControllerScript.rubiControl.grounded) {
				GameCamera.gameCamera.pseudoPause = true;
				PopupController.pop.DisplayDialog (items [currDialog].dialog, items[currDialog].portrait, false);
				rubiAirStrangeInit = true;
			}
		}

		if (rubiAirStrangeInit) {
			if (Input.GetButtonDown ("Fire1") && currDialog < items.Length - 1) {
				currDialog += 1;
				PopupController.pop.DisplayDialog (items [currDialog].dialog, items[currDialog].portrait, true);
			} else if (Input.GetButtonDown ("Fire1") && currDialog == items.Length - 1) {
				GameCamera.gameCamera.pseudoPause = false;
				GameVars.vars.rubiAirStrange = true;
				PopupController.pop.HideDialog ();
			}
		}
		if (GameVars.vars.rubiAirStrange == true) {
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player" && !rubiAirStrangeTriggered) {
			rubiAirStrangeTriggered = true;
		}
	}
}
