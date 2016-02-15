using UnityEngine;
using System.Collections;

public class ParticleLayer : MonoBehaviour {
	public string layerName;
	void Start () {
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = layerName;
	}
}
