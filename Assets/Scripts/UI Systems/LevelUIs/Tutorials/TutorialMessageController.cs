using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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

	private float leftPanelStartX;
	private float rightPanelStartX;

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

		leftPanelStartX = leftDisplayPanel.transform.localPosition.x;
		rightPanelStartX = rightDisplayPanel.transform.localPosition.x;

		leftDisplayPanel.transform.DOLocalMoveX (leftPanelStartX - (Screen.width / 2), 0.5f).Play ();
		rightDisplayPanel.transform.DOLocalMoveX (rightPanelStartX + (Screen.width / 2), 0.5f).Play ();

		HideMessage ();
	}

	void OnLevelWasLoaded(){
		HideMessage ();
	}

	public void DisplayMessage(TutorialMessage messageObject){
		//ensure that any previous message is overriden
		if (messageObject != currentMessage) { //do not redisplay if already on display
			HideMessage ();
			currentMessage = messageObject;
			StartCoroutine ("SetupMessage");
		}

	}

	private IEnumerator SetupMessage(){
		Debug.Log ("Setting up message for display");
		yield return new WaitForSeconds (0.5f); //wait for previous message to finish hiding
		if (currentMessage.messageIsOnLeft) {
			SetupText (leftMessageText, leftDisplayPanel, leftCaption);
			SetupImage (rightImageHolder, rightDisplayPanel, rightCaption);
		}
		else {
			SetupText (rightMessageText, rightDisplayPanel, rightCaption);
			SetupImage (leftImageHolder, leftDisplayPanel, leftCaption);
		}
		
		leftDisplayPanel.transform.DOLocalMoveX (leftPanelStartX, 0.5f).Play ();
		rightDisplayPanel.transform.DOLocalMoveX (rightPanelStartX, 0.5f).Play ();
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

	//hides the message
	public void HideMessage(TutorialMessage messageToHide = null){
		if (messageToHide == currentMessage) {
			messageToHide = null;
			StopCoroutine("SetupMessage");
			currentMessage = null;
		}
		if (messageToHide == null) {
			leftDisplayPanel.transform.DOLocalMoveX (leftPanelStartX - (Screen.width / 2), 0.5f).Play ();
			rightDisplayPanel.transform.DOLocalMoveX (rightPanelStartX + (Screen.width / 2), 0.5f).Play ().OnComplete(FinishHiding);
		}

	}
	//helper function for hide message called after tweening is finished
	private void FinishHiding(){
		//reset display
		leftMessageText.text = "";
		rightMessageText.text = "";
		Destroy (imageInstance);
		//hide panels
		leftDisplayPanel.SetActive (false);
		rightDisplayPanel.SetActive (false);
		//set current message to null
		//StopCoroutine ("SetupMessage");
		//currentMessage = null;
	}

	//closes the message and registers that it should not be shown again
	public void CloseMessage(){
		if (currentMessage != null) {
			currentMessage.ExternalTriggerCall (TutorialMessage.TriggerBehaviour.Close);
		}
	}

}
