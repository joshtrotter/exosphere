﻿using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MenuCameraController : MonoBehaviour {

	private Vector3 startPos;
	private Vector3 lastPos;
	private float startTime;
	private bool isAHorizontalSwipe;
	private bool isAVerticalSwipe;
	private float swipeSpeed = 30f;

	public float swipeThreshold = 100f;
	//keep track of whether or not this script should be monitoring swipe input
	public bool shouldMonitorHorizontalSwiping;
	public bool shouldMonitorVerticalSwiping;

	//keep a reference to the LevelInfoManager
	public LevelSelectManager screenManager;

	void OnLevelWasLoaded(){
		if (LevelManager.manager.IsLevelLoader ()) {
			transform.GetChild (0).gameObject.SetActive(true);
		} else {
			transform.GetChild (0).gameObject.SetActive(false);
		}
	}

	void Start(){
		CameraShake ();
	}

	void Update () {
		MonitorSwiping ();
#if UNITY_EDITOR
		if (shouldMonitorHorizontalSwiping) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ArrowKeySwipe(36);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)){
				ArrowKeySwipe (-36);
			}
		}
#endif
	}

	//applies a subtle, constant sway to the camera
	private void CameraShake(){
		GetComponentInChildren<Camera> ().DOShakeRotation (10,1.1f,0).Play ().SetLoops (-1);
	}

	private void MonitorSwiping ()
	{
		//check for screen swiping and update camera accordingly
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			Vector3 target = transform.localRotation.eulerAngles;
			switch (touch.phase) {

			case TouchPhase.Began:
				startPos = touch.position;
				lastPos = touch.position;
				startTime = Time.time;
				break;

			case TouchPhase.Moved:
				//detect whether the finger initially moves far enough to count as a swipe movement
				if (!isAVerticalSwipe && shouldMonitorHorizontalSwiping && Mathf.Abs (touch.position.x - startPos.x) > swipeThreshold) {
					isAHorizontalSwipe = true;
				}

				if (!isAHorizontalSwipe && shouldMonitorVerticalSwiping && Mathf.Abs (touch.position.y - startPos.y) > swipeThreshold){
					isAVerticalSwipe = true;
				}

				if (isAHorizontalSwipe) {
					float changeInX = touch.position.x - lastPos.x;
					target.y -= ((changeInX / Screen.width) * 54);
					DOTween.CompleteAll ();
					transform.DOLocalRotate (target, Time.deltaTime).Play ();
				}

				if (isAVerticalSwipe) {
					float changeInY = touch.position.y - lastPos.y;
					target.x += ((changeInY / Screen.height) * 36);
					DOTween.CompleteAll ();
					transform.DOLocalRotate (target, Time.deltaTime).Play ();
				}

				lastPos = touch.position;
				break;

			case TouchPhase.Ended:
				if (isAHorizontalSwipe) {
					swipeSpeed = (((startPos.x - touch.position.x) / Screen.width) * 36) / (Time.time - startTime);
					swipeSpeed = swipeSpeed > 0 ? Mathf.Max (swipeSpeed, 30) : Mathf.Min (swipeSpeed, -30);

					target.y += (swipeSpeed * 0.3f);

					screenManager.SetClosestScreenAsFocused (Quaternion.Euler (target));
					FocusCameraOnCurrentScreen ();
					isAHorizontalSwipe = false;
				}

				if (isAVerticalSwipe){
					swipeSpeed = (((startPos.y - touch.position.y) / Screen.height) * 36) / (Time.time - startTime);
					swipeSpeed = swipeSpeed > 0 ? Mathf.Max (swipeSpeed, 30) : Mathf.Min (swipeSpeed, -30);

					target.x -= (swipeSpeed * 0.3f);

					screenManager.MoveBetweenVerticalLayers(Quaternion.Euler (target));
					isAVerticalSwipe = false;
				}
				break;
			}
		}
		//check camera position and update infoScreens accordingly
		if (isAHorizontalSwipe) {
			screenManager.SetClosestScreenAsFocused (transform.localRotation);
		}
	}

	public bool IsSwiping(){
		return isAHorizontalSwipe || isAVerticalSwipe;
	}

	public void FocusCameraOnCurrentScreen ()
	{
		Vector3 target = screenManager.GetCurrentScreen().transform.localRotation.eulerAngles;
		float Xdistance = Mathf.Abs(target.x - transform.localEulerAngles.x);
		float Ydistance = Mathf.Abs(target.y - transform.localEulerAngles.y);
		float distance = Mathf.Max(Xdistance,Ydistance);
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
		screenManager.SetClosestScreenAsFocused (Quaternion.Euler (target));
		FocusCameraOnCurrentScreen ();
	}
#endif
}
