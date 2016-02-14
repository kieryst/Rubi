using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Transform healthBar;

	void Update () {
		float maxHealth = GameVars.vars.maxHealth;
		float currHealth = GameVars.vars.currentHealth;
		float theScale = 1 - (currHealth / maxHealth);
		healthBar.localScale = new Vector3(theScale, 1f, 1f);
	}
}