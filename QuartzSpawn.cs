using UnityEngine;
using System.Collections;

public class QuartzSpawn : MonoBehaviour {

	public GameObject Obj;

	void Start () {
		if (GameVars.vars.haveQuartz == false) {
			Instantiate (Obj, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
