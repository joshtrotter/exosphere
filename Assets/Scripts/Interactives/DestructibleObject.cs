using UnityEngine;
using System.Collections;

public class DestructibleObject : HasLevelState {

	protected const int NORMAL_STATE = 0;
	protected const int DESTROYED_STATE = 1;

	protected virtual void DestroyObject(){
		Debug.Log ("Registering as destroyed");
		RegisterStateChange (DESTROYED_STATE);
		gameObject.SetActive (false);
	}

	public override void ReloadState(int state){
		if (state == DESTROYED_STATE) {
			Debug.Log ("Destroying");
			gameObject.SetActive(false);
		}
	}
}
