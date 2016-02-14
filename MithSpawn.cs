using UnityEngine;
using System.Collections;

public class MithSpawn : MonoBehaviour {

	public GameObject Obj;

	// ToDo: Destroy spawn object if Mith boss fight already occured?
	void Start () {
		if (GameVars.vars.boss1 == false) {
			Instantiate (Obj, transform.position, Quaternion.identity);
		}
	}
}
