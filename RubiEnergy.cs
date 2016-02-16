using UnityEngine;
using System.Collections;

public class RubiEnergy : MonoBehaviour {
	
	public float currentEnergy;
	public float regenAmount = 1f;

	public void Start() {
		DontDestroyOnLoad (gameObject);
		StartCoroutine (RegenEnergy ());
	}
	
	public void SpendEnergy (int amount) {
		currentEnergy -= amount;
	}

	private IEnumerator RegenEnergy() {
		while (true) {
			yield return new WaitForSeconds(regenAmount);
			if (currentEnergy < GameVars.vars.maxEnergy) {
				++currentEnergy;
			}
		}
	}
}
