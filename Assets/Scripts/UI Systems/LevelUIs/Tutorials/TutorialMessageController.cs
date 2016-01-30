using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialMessageController : MonoBehaviour {

	public static TutorialMessageController controller;

	public Text leftMessageText;
	public Text rightMessageText;
	public GameObject leftImageHolder;
	public GameObject rightImageHolder;
	public Text leftCaption;
	public Text rightCaption;

	//find references to parent panels to display/hide
	private GameObject leftDisplayPanel;
	private GameObject rightDisplayPanel;

	//store information about the currentMessage
	private TutorialMessage currentMessage;
	private Text caption;
	private GameObject imageInstance;

	void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
		} else if (controller != this) {
			Destroy(gameObject);
		}
		leftDisplayPanel = leftMessageText.transform.parent.gameObject;
		rightDisplayPanel = rightMessageText.transform.parent.gameObject;

		HideMessage ();
	}

	public void DisplayMessage(TutorialMessage messageObject){
		//ensure that any previous message is overriden
		HideMessage ();
		currentMessage = messageObject;

		if (currentMessage.messageIsOnLeft) {
			SetupText (leftMessageText, leftDisplayPanel, leftCaption);
			SetupImage (rightImageHolder, rightDisplayPanel, rightCaption);
		}
		else {
			SetupText (rightMessageText, rightDisplayPanel, rightCaption);
			SetupImage (leftImageHolder, leftDisplayPanel, leftCaption);
		}
	}

	private void SetupText (Text messageText, GameObject displayPanel, Text caption)
	{
		messageText.text = currentMessage.message;
		caption.text = currentMessage.messageCaption;
		displayPanel.SetActive (true);
	}

	private void SetupImage(GameObject imageHolder, GameObject displayPanel, Text caption){
		if (currentMessage.imagePrefab != null){
			imageInstance = Instantiate(currentMessage.imagePrefab);
			imageInstance.transform.SetParent(imageHolder.transform, false);
			//setup caption
			caption.text = currentMessage.imageCaption;
			//display the panel
			displayPanel.SetActive (true);
		}
	}

	//closes the message
	public void HideMessage(TutorialMessage messageToHide = null){
		if (messageToHide == currentMessage || messageToHide == null) {
			//reset display
			leftMessageText.text = "";
			rightMessageText.text = "";
			Destroy (imageInstance);
			//hide panels
			leftDisplayPanel.SetActive (false);
			rightDisplayPanel.SetActive (false);
		}

	}

	//closes the message and registers that it should not be shown again
	public void CloseMessage(){
		currentMessage.ExternalTriggerCall (TutorialMessage.TriggerBehaviour.Close);
	}

}
