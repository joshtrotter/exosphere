using UnityEngine;
using System.Collections;

public class TriggerInOutTutorialMessage : TutorialMessage {
	
	private float startTime;
	public float minDisplayTime = 2f;

	void OnTriggerEnter(Collider coll)
	{
		if (coll.CompareTag ("Player")) {
			startTime = Time.realtimeSinceStartup;
			OpenMessage ();
		}
	}

	void OnTriggerExit()
	{
		StartCoroutine (StopFlashClose ());
	}

	//ensures the message has been up for at least 2 seconds to stop it flashing on and off
	private IEnumerator StopFlashClose(){ 
		while (Time.realtimeSinceStartup - startTime < minDisplayTime) {
			yield return new WaitForEndOfFrame();
		}
		CloseMessage ();
	}

}
