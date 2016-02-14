using UnityEngine;
using System.Collections;

public class HealthPickupController : MonoBehaviour {

	public int healthValue;
	private bool consumed = false;

	void Update () {
		if (consumed) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			if (GameVars.vars.currentHealth < GameVars.vars.maxHealth) {
				int healthDifference = GameVars.vars.maxHealth - GameVars.vars.currentHealth;
				if (healthDifference < healthValue) {
					GameVars.vars.currentHealth = GameVars.vars.maxHealth;
				} else {
					GameVars.vars.currentHealth += healthValue;
				}
				consumed = true;
			}
		}
	}
}
