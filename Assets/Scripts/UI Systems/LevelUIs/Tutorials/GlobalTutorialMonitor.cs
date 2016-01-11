using UnityEngine;
using System.Collections;

public class GlobalTutorialMonitor : MonoBehaviour {

	public static GlobalTutorialMonitor controller;

	public TutorialMessage collectableTutorial;
	public TutorialMessage checkpointTutorial;
	public float displayTime = 8;

	void Awake(){
		controller = this;
	}
	
	public void CollectableFound(){
		OpenTutorial (collectableTutorial);
	}
	
	public void CheckpointReached(){
		OpenTutorial (checkpointTutorial);
	}

	private void OpenTutorial(TutorialMessage tutorial){
		tutorial.ExternalTriggerCall (TutorialMessage.TriggerBehaviour.Open);
		StartCoroutine (CloseAfterTimeOut (tutorial));
	}

	private IEnumerator CloseAfterTimeOut(TutorialMessage tutorial){
		float timeOpen = 0;
		while (timeOpen < displayTime) {
			yield return new WaitForEndOfFrame();
			timeOpen += Time.deltaTime;
		}
		tutorial.ExternalTriggerCall (TutorialMessage.TriggerBehaviour.Close);
	}

}
