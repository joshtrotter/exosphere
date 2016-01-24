using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MenuCameraController : MonoBehaviour {

	private Vector3 startPos;
	private Vector3 lastPos;
	private float startTime;
	private bool isASwipe;
	private float swipeSpeed = 30f;

	public float swipeThreshold = 100f;
	//keep track of whether or not this script should be monitoring swipe input
	public bool shouldMonitorSwiping;

	//keep a reference to the LevelInfoManager
	private LevelSelectManager screenManager;

	void Start(){
		screenManager = GetComponentInParent<LevelSelectManager> ();
		CameraShake ();
	}

	void Update () {
		if (shouldMonitorSwiping) {
			MonitorSwiping ();
#if UNITY_EDITOR
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ArrowKeySwipe(36);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)){
				ArrowKeySwipe (-36);
			}
#endif
		}
	}

	private void CameraShake(){
		GetComponentInChildren<Camera> ().DOShakeRotation (10,1.1f,0).Play ().SetLoops (-1);
	}

	private void MonitorSwiping ()
	{
		//check for screen swiping and update camera accordingly
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			switch (touch.phase) {

			case TouchPhase.Began:
				startPos = touch.position;
				lastPos = touch.position;
				startTime = Time.time;
				break;

			case TouchPhase.Moved:
				//detect whether the finger initially moves far enough to count as a swipe movement
				if (Mathf.Abs (touch.position.x - startPos.x) > swipeThreshold) {
					isASwipe = true;
				}
				float changeInX = touch.position.x - lastPos.x;
				if (isASwipe) {
					Vector3 target = transform.localRotation.eulerAngles;
					target.y -= ((changeInX / Screen.width) * 36);
					DOTween.CompleteAll ();
					transform.DOLocalRotate (target, Time.deltaTime).Play ();
				}
				lastPos.x = touch.position.x;
				break;

			case TouchPhase.Ended:
				if (isASwipe) {
					swipeSpeed = (((startPos.x - touch.position.x) / Screen.width) * 36) / (Time.time - startTime);
					swipeSpeed = swipeSpeed > 0 ? Mathf.Max (swipeSpeed, 30) : Mathf.Min (swipeSpeed, -30);
					//swipeSpeed = Mathf.Max (Mathf.Abs (swipeSpeed),30);
					Vector3 target = transform.localRotation.eulerAngles;
					target.y += (swipeSpeed * 0.3f);
					screenManager.SetClosestScreenAsFocused (Quaternion.Euler (target));
					FocusCameraOnCurrentScreen ();
					isASwipe = false;
				}
				break;
			}
		}
		//check camera position and update infoScreens accordingly
		if (isASwipe) {
			screenManager.SetClosestScreenAsFocused (transform.localRotation);
		}
	}

	public bool IsSwiping(){
		return isASwipe;
	}

	public void FocusCameraOnCurrentScreen ()
	{
		Vector3 target = screenManager.GetCurrentScreen().transform.localRotation.eulerAngles;
		float distance = Mathf.Abs(target.y - transform.localEulerAngles.y);
		distance = distance > 180 ? 360 - distance : distance;
		float time = (distance / Mathf.Abs (swipeSpeed));
		FocusCamera (target, time);

	}

	public void FocusCamera (Vector3 target, float time)
	{
		DOTween.CompleteAll ();
		transform.DOLocalRotate (target, time).Play ();
	}

#if UNITY_EDITOR
	private void ArrowKeySwipe (float num)
	{
		Vector3 target = transform.localEulerAngles;
		target.y += num;
		if (target.y < 360 && target.y > 0) {
			screenManager.SetClosestScreenAsFocused (Quaternion.Euler (target));
			FocusCameraOnCurrentScreen ();
		}
	}
#endif
}
