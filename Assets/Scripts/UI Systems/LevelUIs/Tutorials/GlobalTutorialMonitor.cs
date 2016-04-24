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
		StartCoroutine(OpenTutorial (collectableTutorial));
	}
	
	public void CheckpointReached(){
		StartCoroutine (OpenTutorial(checkpointTutorial));
	}

	private IEnumerator OpenTutorial(TutorialMessage tutorial){
		if (PlayerPrefs.GetString (tutorial.uniqueId) != "closed"){
			tutorial.ExternalTriggerCall (TutorialMessage.TriggerBehaviour.Open);
			yield return new WaitForSeconds (tutorial.timeOut);
			PlayerPrefs.SetString (tutorial.uniqueId, "closed");
			PlayerPrefs.Save();
		}
	} 

	public void ClearGlobalTutorialData(){
		foreach (TutorialMessage tut in GetComponentsInChildren<TutorialMessage>()){
			PlayerPrefs.DeleteKey(tut.uniqueId);
		}
		PlayerPrefs.Save ();
	}

}
