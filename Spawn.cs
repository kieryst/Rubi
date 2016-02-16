using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public GameObject Obj;
	public float respawnDist;
	public bool collided;
	public float delay = 1f;
	
	void Start () {
		Instantiate (Obj, transform.position, Quaternion.identity);
		StartCoroutine (SpawnThings ());
	}

	void OnTriggerStay2D (Collider2D collider) {
		if (collider.gameObject.tag == "Enemy") {
			collided = true;
		}
	}
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Enemy") {
			collided = true;
		}
	}
	void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.tag == "Enemy") {
			collided = false;
		}
	}

	private IEnumerator SpawnThings() {
		while (true) {
			yield return new WaitForSeconds(delay);
			if (!collided) {
				float distance = Vector3.Distance (RubiControllerScript.rubiControl.gameObject.transform.position, transform.position);
				if (distance > respawnDist) {
					Instantiate (Obj, transform.position, Quaternion.identity);
				}
			}
		}
	}
}
