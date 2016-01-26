using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldInfo : MonoBehaviour {

	public WorldData currentWorld;
	private WorldSelectManager worldSelectManager;

	public Text title;

	void Start(){
		worldSelectManager = GetComponentInParent<WorldSelectManager> ();
	}

	public void DisplayWorldInfo(WorldData newWorld){
		currentWorld = newWorld;
		SetLatestInfo ();
	}

	public void EnterWorld(){
		worldSelectManager.EnterWorld (currentWorld);
	}

	private void SetLatestInfo(){
		title.text = currentWorld.worldName;
	}
}
