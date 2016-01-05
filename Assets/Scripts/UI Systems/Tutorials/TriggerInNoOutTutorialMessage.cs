using UnityEngine;
using System.Collections;

public class TriggerInNoOutTutorialMessage : TutorialMessage {

	void OnTriggerEnter(Collider coll)
	{
		if (coll.CompareTag ("Player")) {
			OpenMessage ();
		}
	}
}
