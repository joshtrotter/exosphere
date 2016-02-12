using UnityEngine;
using System.Collections;

public class DeadBallCollider : SwitchableObject {

	private LevelManager levelManager;
	private bool shouldReload = true;

	void OnTriggerEnter(Collider coll) 
	{
		if (coll.CompareTag("Player") && shouldReload) {
			LevelManager.manager.ReloadLevel();
		}
	}

	public override void Activate ()
	{
		shouldReload = !shouldReload;
	}
}
