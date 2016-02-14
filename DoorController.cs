using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public bool opened = false;
	public bool powered = false;
	private Animator anim;
	private BoxCollider2D doorCollider;
	
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		doorCollider = gameObject.GetComponent<BoxCollider2D> ();
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "AlphaParticle" && !opened && powered) {
			opened = true;
			anim.SetBool ("Open", true);
		}
	}
	
	public void Open () {
		doorCollider.enabled = false;
	}
}
