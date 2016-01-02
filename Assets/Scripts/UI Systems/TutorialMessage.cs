using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialMessage : MonoBehaviour {

	private TutorialMessageController controller;
	public string message;
	public string messageCaption = "Tap to Close";
	public GameObject imagePrefab;
	public string imageCaption;
	//determines whether message is on left or right. Images will be automatically sent to the opposite side to message
	public bool messageIsOnLeft = true;
	[HideInInspector]
	public bool HasBeenClosed;


	void OnLevelWasLoaded()
	{
		controller = GameObject.FindGameObjectWithTag ("TutorialSystem").GetComponent<TutorialMessageController> ();
	}

	public void OpenMessage(){
		if (!HasBeenClosed) {
			controller.DisplayMessage (this);
		}
	}

	public void CloseMessage(){
		controller.HideMessage ();
	}

	void OnTriggerEnter()
	{
		OpenMessage ();
	}

	void OnTriggerExit()
	{
		//CloseMessage ();
	}
}
