using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class WorldSelectManager : MonoBehaviour {

	public CanvasRenderer movingPanel;
	public Image background;

	//a panel over the buttons of the main menu which can be enabled to prevent animation/colour glitches
	public GameObject blockingPanel;

	private CanvasGroup canvas;
	private LevelSelectManager levelSelectManager;
	private WorldInfo worldInfo;
	private RectTransform rectTransform;
	private int worldNumber = 0;

	private bool isShown;

	//the distance between the different layers
	public float worldGap = 716;

	private Vector3 startPos;
	private Vector3 lastPos;
	private float startTime;
	private bool isAVerticalSwipe;
	private float swipeSpeed = 30f;
	
	public float swipeThreshold = 100f;


	void Awake(){
		DontDestroyOnLoad (this);
		isShown = true;

		canvas = GetComponentInChildren<CanvasGroup> ();
		levelSelectManager = GetComponent<LevelSelectManager> ();
		worldInfo = GetComponentInChildren<WorldInfo> ();
		rectTransform = worldInfo.GetComponent<RectTransform> ();

		worldGap = Screen.height * 3;
		background.rectTransform.sizeDelta = new Vector2 (background.rectTransform.sizeDelta.x, worldGap * 6);
		rectTransform.offsetMax = new Vector2 (rectTransform.offsetMax.x, worldGap);
		rectTransform.offsetMin = new Vector2 (rectTransform.offsetMin.x, worldGap);
	}

	/* TODO This comment prevents swiping on the main menu (which would allow player to move into the world select screens)
	void Update(){
		UpdateWorldInfo ();
		MonitorSwiping ();
	}
	*/

	//changes position and display of the world info screen to display correct data
	private void UpdateWorldInfo ()
	{
		if (worldNumber > 0) {
			rectTransform.offsetMax = new Vector2 (rectTransform.offsetMax.x, (worldNumber * worldGap));
			rectTransform.offsetMin = new Vector2 (rectTransform.offsetMin.x, (worldNumber * worldGap));
			worldInfo.DisplayWorldInfo (LevelDataManager.manager.GetWorldData (worldNumber));
		}
	}

	public void MoveScreenToWorld (int worldChange = 0)
	{
		worldNumber += worldChange;
		float time = Mathf.Abs((((worldNumber * worldGap) - Mathf.Abs (movingPanel.transform.localPosition.y)))) / worldGap;
		movingPanel.transform.DOLocalMoveY ((-1 * (worldNumber * worldGap)), time).Play ().OnUpdate(CheckForClosestWorldToCurrentPosition);
	}

	public void EnterWorld(WorldData world){
		if (!isAVerticalSwipe){
			//prevent weird animation glitches on buttons by covering them up
			blockingPanel.SetActive(true);
			isShown = false;
			levelSelectManager.StartWorldLevelsDisplay (world);
			canvas.DOFade (0, 1).Play ().OnComplete(Disable);
		}
	}

	public void InitiateWorldPurchaseOptions(WorldData world){
		if (!isAVerticalSwipe){
			Debug.Log ("Send them somewhere they can give us the moolah!");
		}
	}

	public void ReturnToOpeningScreen(){
		//remove blocking panel so that buttons can be pressed
		blockingPanel.SetActive(false);
		Debug.Log (worldNumber * worldGap);
		movingPanel.transform.DOLocalMoveY (0, 0).Play ();
		worldNumber = 0;
		canvas.alpha = 1f;
		canvas.gameObject.SetActive (true);
	}

	public void ExitWorld(){
		//remove blocking panel so that buttons can be pressed
		blockingPanel.SetActive(false);
		isShown = true;
		canvas.gameObject.SetActive (true);
		canvas.DOFade (1, 1).Play ();
	}

	private void Disable(){
		canvas.gameObject.SetActive (false);
	}

	private void MonitorSwiping ()
	{
		//check for screen swiping and update camera accordingly
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			
			Vector3 target = movingPanel.transform.localPosition;
			switch (touch.phase) {
				
			case TouchPhase.Began:
				startPos = touch.position;
				lastPos = touch.position;
				startTime = Time.time;
				break;
				
			case TouchPhase.Moved:
				//detect whether the finger initially moves far enough to count as a swipe movement
				if ( Mathf.Abs (touch.position.y - startPos.y) > swipeThreshold){
					isAVerticalSwipe = true;
				}
				
				if (isAVerticalSwipe) {
					float changeInY = touch.position.y - lastPos.y;
					target.y += ((changeInY / Screen.height) * (worldGap * 1.5f));
					if (target.y <= 0 && target.y >= -1 * (worldGap * LevelDataManager.manager.GetNumberOfWorlds())){
						DOTween.CompleteAll ();
						movingPanel.transform.DOLocalMove(target, Time.deltaTime).Play ();
					}
				}
				
				lastPos = touch.position;
				break;
				
			case TouchPhase.Ended:
				if (isAVerticalSwipe){
					swipeSpeed = (((startPos.y - touch.position.y) / Screen.height) * (worldGap)) / (Time.time - startTime);
					target.y -= (swipeSpeed * 0.3f);
					//Debug.Log (background.transform.localPosition + ", " + target);
					//background.transform.DOLocalMove(target, 0.3f).Play ().OnKill (MoveScreenToWorld);
					if (target.y <= 0 && target.y >= -1 * (worldGap * LevelDataManager.manager.GetNumberOfWorlds())){
						CheckForClosestWorld(target.y);
					}
					MoveScreenToWorld(0);
					isAVerticalSwipe = false;
				}
				break;
			}

			if (isAVerticalSwipe) {
				CheckForClosestWorldToCurrentPosition();
			}
		}
	}

	private void CheckForClosestWorldToCurrentPosition(){
		CheckForClosestWorld (movingPanel.transform.localPosition.y);
	}

	private void CheckForClosestWorld(float target_y){
		if ((-1 * target_y) < ((worldNumber * worldGap) - (worldGap * 0.3))) {
			worldNumber -= 1;
		} else if ((-1 * target_y) > (worldNumber * worldGap) + (worldGap * 0.3)) {
			worldNumber += 1;
		}
	}
	
	//the main menu controller will call this function when the back button is pressed
	public void BackButton(){
		if (isShown) ReturnToOpeningScreen ();
	}

	public void InitiateTunnelRunnerLaunch(){
		blockingPanel.SetActive (true);
		movingPanel.transform.DOLocalMoveY ((-2 * Screen.height), 1f).Play ().OnComplete (LaunchTunnelRunner);
	}

	public void OpenSettingsMenu(){
		blockingPanel.SetActive (true);
		movingPanel.transform.DOLocalMoveY ((2 * Screen.height), 1f).Play ().OnComplete (SettingsMenu.controller.RequestToBeShown);
	}

	private void LaunchTunnelRunner(){
		LevelManager.manager.LoadTunnelRunner ();	
	}

}
