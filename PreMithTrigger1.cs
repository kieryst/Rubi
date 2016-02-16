using UnityEngine;
using System.Collections;

public class PreMithTrigger1 : MonoBehaviour {

	public bool mith1Triggered = false;
	private bool mith1Init = false;

	DialogItems[] items;
	int currDialog = 0;
	
	void Start () {
		items = new DialogItems[3];
		items [0] = new DialogItems ();
		items [0].dialog = "Mith!!! Sister!";
		items [0].portrait = "rubi_portrait";
		items [1] = new DialogItems ();
		items [1].dialog = "I don't know what's happened. I don't know where we are!";
		items [1].portrait = "rubi_portrait";
		items [2] = new DialogItems ();
		items [2].dialog = "... Hmm. It seems you're here too Rubi?";
		items [2].portrait = "mith_portrait";
	}

	void Update () {
		if (MithController.mithControl != null) {
			if (mith1Triggered && !mith1Init) {
				if (RubiControllerScript.rubiControl.grounded) {
					GameCamera.gameCamera.isFixed = true;
					GameCamera.gameCamera.fixed_x = -20f;
					GameCamera.gameCamera.fixed_y = -20f;
					GameCamera.gameCamera.pseudoPause = true;
					mith1Init = true;
					PopupController.pop.DisplayDialog (items [currDialog].dialog, items[currDialog].portrait, false);
				}
			}
			if (mith1Init) {
				if (Input.GetButtonDown ("Fire1") && currDialog < items.Length - 1) {
					currDialog = currDialog + 1;
					PopupController.pop.DisplayDialog (items [currDialog].dialog, items[currDialog].portrait, true);
				} else if (Input.GetButtonDown ("Fire1") && currDialog == items.Length - 1) {
					GameCamera.gameCamera.pseudoPause = false;
					GameVars.vars.mith1 = true;
					PopupController.pop.HideDialog ();
				}
			}
			if (GameVars.vars.mith1 == true && GameCamera.gameCamera.isFixed == true) {
				GameCamera.gameCamera.isFixed = false;
			}
		}
		if (GameVars.vars.mith1 == true) {
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player" && !mith1Triggered) {
			mith1Triggered = true;
		}
	}
}
