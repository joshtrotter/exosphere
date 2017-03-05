using UnityEngine;
using System.Collections;

public class GameObjectDisabler : SwitchableObject {

	public GameObject targetObject;

	public override void Activate ()
	{
		Debug.Log ("Activated");
		if (targetObject.activeSelf) {
			Debug.Log ("Setting object " + targetObject.name + " inactive");
			targetObject.SetActive (false);
		} else {
			Debug.Log ("Setting object " + targetObject.name + " active");
			targetObject.SetActive(true);
		}
	}
}
