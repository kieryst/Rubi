using UnityEngine;
using System.Collections;

public class PermHealthSpawn : MonoBehaviour {

	public GameObject Obj;
	public int ID;

	// ToDo: Destroy spawn object if health pickup is taken. Also check for tag of collider because reasons.
	void Start () {
        if (GameVars.vars.healthPickups[ID] == false) {
			Instantiate (Obj, transform.position, Quaternion.identity);
		}
	}
	void Update () {
		if (Physics2D.OverlapCircle (transform.position, 0.1f) == null) {
			GameVars.vars.healthPickups[ID] = true;
		}
	}
}
