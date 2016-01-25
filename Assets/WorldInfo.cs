using UnityEngine;
using System.Collections;

public class WorldInfo : MonoBehaviour {

	public WorldData currentWorld;
	private WorldSelectManager worldSelectManager;

	void Start(){
		worldSelectManager = GetComponentInParent<WorldSelectManager> ();
	}

	public void EnterWorld(){
		worldSelectManager.EnterWorld (currentWorld);
	}
}
