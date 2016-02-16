using UnityEngine;
using System.Collections;

public class QuartzController : MonoBehaviour {
	
	public bool consumed;
	private float anchor;

	void Start () {
		anchor = gameObject.transform.position.y;
		consumed = false;
	}
	
	void Update () {
		if (consumed) {
			Destroy(gameObject);
		}
		transform.position = new Vector3(transform.position.x, anchor + (Mathf.Sin(Time.realtimeSinceStartup))* 0.1f, transform.position.z);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			GameVars.vars.haveQuartz = true;
			GameVars.vars.maxEnergy += 1;
			consumed = true;
		}
	}
}
