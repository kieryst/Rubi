using UnityEngine;
using System.Collections;

public class PreMithSpawn1 : MonoBehaviour {

	public GameObject Obj;
	
	void Start () {
		if (GameVars.vars.mith1 == false) {
			Instantiate (Obj, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
