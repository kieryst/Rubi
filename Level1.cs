using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour {
	
	public GameObject player;
	public GameObject camObj;
	public GameCamera cam;

	void Start () {
		camObj = GameObject.FindGameObjectWithTag ("MainCamera");
		cam = camObj.GetComponent<GameCamera> ();
		SpawnPlayer ();
	}
	
	private void SpawnPlayer () {
		cam.SetTarget ((Instantiate (player, new Vector3(GameVars.vars.currentX, GameVars.vars.currentY, 0), Quaternion.identity) as GameObject).transform) ;
		cam.transform.position = new Vector3 (GameVars.vars.currentX, GameVars.vars.currentY, -15);
	}
}