using UnityEngine;
using System.Collections;

public class SlimeBehaviour : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") 
			RubiHealth.rubiHealth.TakeDamage(1);
	}
	void OnTriggerStay2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") 
			RubiHealth.rubiHealth.TakeDamage(1);
	}
}
