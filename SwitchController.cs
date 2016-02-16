using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwitchController : MonoBehaviour {

	public GameObject door;
	private Animator doorAnim;
	private Animator anim;
	private DoorController doorScript;
	public bool powered = false;
	public bool triggered = false;

	// Use this for initialization
	void Start () {
		doorAnim = door.GetComponent<Animator> ();
		doorScript = door.GetComponent<DoorController> ();
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (powered && !triggered) {
			triggered = true;
			doorScript.powered = true;
			doorAnim.SetBool ("Powered", true);
			anim.SetBool ("SwitchOn", true);
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			PopupController.pop.DisplayPopup("Press A  to activate");
			RubiControllerScript.rubiControl.switchActive = true;
		}
	}
	void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			PopupController.pop.HidePopup();
			RubiControllerScript.rubiControl.switchActive = false;
		}
	}
}
