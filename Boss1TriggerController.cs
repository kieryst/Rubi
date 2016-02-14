using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boss1TriggerController : MonoBehaviour {

	public bool boss1Triggered = false;
	private bool boss1Init = false;
	private bool dialogFinished = false;

	private bool cameraPanned = false;

	public PopupController pop;

	DialogItems[] items;

	int currDialog = 0;

	void Start () {
		items = new DialogItems[5];
		items [0] = new DialogItems ();
		items [0].dialog = "...";
		items [0].portrait = "rubi_portrait";
		items [1] = new DialogItems ();
		items [1].dialog = "... Hello?";
		items [1].portrait = "rubi_portrait";
		items [2] = new DialogItems ();
		items [2].dialog = "... Hey! Hey what's going on here?";
		items [2].portrait = "rubi_portrait";
		items [3] = new DialogItems ();
		items [3].dialog = "I'm not going home, Rubi.";
		items [3].portrait = "mith_portrait";
		items [4] = new DialogItems ();
		items [4].dialog = "... A nd neither are you!!";
		items [4].portrait = "mith_portrait";
	}
	
	void FixedUpdate () {
		// Begin conversation before boss fight
		if (MithController.mithControl != null) {
			if (boss1Triggered && !boss1Init) {
				//First trigger. Fix the camera and begin dialog when Rubi is not grounded. (Don't want to start if Rubi is mid-jump)
				if (RubiControllerScript.rubiControl.grounded) {
					GameCamera.gameCamera.isFixed = true;
					GameCamera.gameCamera.fixed_x = -36f;
					GameCamera.gameCamera.fixed_y = -1f;
					GameCamera.gameCamera.disabledAllowCamera = true;
					boss1Init = true;
					GameCamera.gameCamera.enabled = false;
					PopupController.pop.DisplayDialog (items [currDialog].dialog, items[currDialog].portrait);
				}
			}
			// Cycle dialog
			if (boss1Triggered && boss1Init && !dialogFinished) {
				if (Input.GetButtonDown ("Fire1") && currDialog < items.Length - 1) {
					currDialog += 1;
					PopupController.pop.CycleDialog (items [currDialog].dialog, items[currDialog].portrait);
				} else if (Input.GetButtonDown ("Fire1") && currDialog == items.Length - 1) {
					// Last dialog. Enable Mith boss fight.
					GameCamera.gameCamera.enabled = true;
					dialogFinished = true;
					MithController.mithControl.bossEnabled = true;
					MithController.mithControl.StartMoving ();
					PopupController.pop.HideDialog ();
				}
			}

			// Mith defeated. Unlock camera.
			if (GameVars.vars.boss1 == true && GameCamera.gameCamera.isFixed == true) {
				GameCamera.gameCamera.isFixed = false;
			}
		}

		// Mith does not exist (Probably defeated). Destroy trigger zone.
		if (MithController.mithControl == null) {
			Destroy (gameObject);
		}
	}

	// Player entered trigger zone. Set flag.
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player" && !boss1Triggered) {
			boss1Triggered = true;
		}
	}
}
