using UnityEngine;
using System.Collections;

public class BatSpawn : MonoBehaviour {
	
	public GameObject Bat;
	public float respawnDist = 50f;
	
	void Start () {
		Instantiate (Bat, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Physics2D.OverlapCircle (transform.position, 0.5f) == null) {
			float distance = Vector3.Distance (RubiControllerScript.rubiControl.gameObject.transform.position, transform.position);
			if (distance > respawnDist) {
				Instantiate (Bat, transform.position, Quaternion.identity);
			}
		}
	}
}
