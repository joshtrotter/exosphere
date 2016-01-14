using UnityEngine;
using System.Collections;

public class GlobalTutorialMonitor : MonoBehaviour {

	public static GlobalTutorialMonitor controller;

	public TutorialMessage collectableTutorial;
	public TutorialMessage checkpointTutorial;

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
	}



}
