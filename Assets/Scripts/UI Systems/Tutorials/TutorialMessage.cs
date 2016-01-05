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

	void Awake(){
		message = message.Replace ("\\n", "\n");
		controller = GameObject.FindGameObjectWithTag ("TutorialSystem").GetComponent<TutorialMessageController> ();
	}

	protected void OpenMessage(){
		controller.DisplayMessage (this);
	}

	protected void HideMessage(){
		controller.HideMessage ();
	}

	protected void CloseMessage(){
		controller.HideMessage ();
		this.gameObject.SetActive (false);
	}

}
