using UnityEngine;
using System.Collections;

public class NeutrinoPhaseSpawn : MonoBehaviour {

	public GameObject Obj;

	void Start () {
		if (GameVars.vars.haveNeutrinoPhase == false) {
			Instantiate (Obj, transform.position, Quaternion.identity);
		}
	}
}
