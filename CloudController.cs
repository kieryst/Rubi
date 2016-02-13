using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

	// Clouds travel Right to Left. If a background cloud goes too far off the left, respawn it on the Right hand side of the level.

	public float speed;
	public GameObject cloud;
	public GameObject respawnPoint;

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "CloudDestruction") {
			Instantiate (cloud, new Vector3(respawnPoint.transform.position.x, cloud.transform.position.y, cloud.transform.position.z), Quaternion.identity);
			Destroy (gameObject);
		}
	}
	
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = new Vector3 (-speed, 0, 0);
	}
}
