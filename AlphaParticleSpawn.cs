using UnityEngine;
using System.Collections;

public class AlphaParticleSpawn : MonoBehaviour {

	public GameObject Obj;
	
	void Start () {
		if (GameVars.vars.haveAlphaParticle == false) {
			Instantiate (Obj, transform.position, Quaternion.identity);
		}
	}
}
