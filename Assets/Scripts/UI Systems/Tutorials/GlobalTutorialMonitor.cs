using UnityEngine;
using System.Collections;

public class GlobalTutorialMonitor : MonoBehaviour {

	public TutorialMessage collectableTutorial;
	public TutorialMessage checkpointTutorial;
	
	public void CollectableFound(){
		collectableTutorial.ExternalTriggerCall (TutorialMessage.TriggerBehaviour.Open);
	}
	
	public void CheckpointReached(){
		checkpointTutorial.ExternalTriggerCall (TutorialMessage.TriggerBehaviour.Open);
	}

}
