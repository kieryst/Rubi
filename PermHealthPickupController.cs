using UnityEngine;
using System.Collections;

public class PermHealthPickupController : MonoBehaviour {

	private int healthDifference;
	public bool consumed;
	public int healthIncrease;
	
	// Use this for initialization
	void Start () {
		consumed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (consumed) {
			Destroy(gameObject);
		}
	}
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			GameVars.vars.maxHealth += healthIncrease;
			GameVars.vars.currentHealth = GameVars.vars.maxHealth;
			consumed = true;
		}
	}
}
