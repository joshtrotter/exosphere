using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class TunnelScoreController : MonoBehaviour {

	private int score;
	private float multiplier = 1f;

	//used by external sources to manipulate the multiplier outside the normal speed-based calculation
	private float specialMultiplier = 0f;
	private float specialMultiplierEndTime = 0f;

	//how often the score should update
	public float updateTime = 0.2f;
	//the number of updates should be waited until the multiplier is checked;
	public float multiplierCheckTime = 2.5f;
	public float scorePerDistance = 1.1f;
	//an array of velocity values that are required to maintain/increase speed multiplier
	public float[] multiplierThresholds = new float[10]{0,16,24,36,44,52,60,64,72,80};

	//send a popup message with total distance every time the player travels this many metres
	public float distanceInformIncrements = 500f;
	private float currentWaitToInformDistance;

	//minimum air time
	public float minAirTime = 2f;
	public float airTimePointsPerSecond = 100f;
	private float groundLeftTime;

	//keep track of total distance and time for statistics and potential bonus points
	private float distance;
	private float runTime;
	private float lastCheckTime;

	//we will eventually store/load a highscore value (potentially longest distance/time as well)
	private int highScore;
	private float startTimeForCurrentKm = 0f;
	private float fastestKm;

	//references to pull/push data from
	private Vector3 oldPos;
	public Text scoreText;
	public Text multiplierText;
	public Color lowMultiplierColor;
	public Color highMultiplierColor;

	//can be used to stop scoring, e.g. when recalibrating
	private bool shouldKeepScore = true;
	
	void Start() {
		oldPos = transform.position;
		score = 0;
		distance = 0f;
		runTime = 0f;
		lastCheckTime = 0f;
		groundLeftTime = 10000f;
		currentWaitToInformDistance = distanceInformIncrements;

		StartCoroutine (controlScore ());

	}

	private IEnumerator controlScore() {
		while (true) {
			yield return new WaitForSeconds(updateTime);

			if (shouldKeepScore){
				runTime += updateTime;
			
				float newDistance = Vector3.Distance (transform.position, oldPos);
				oldPos = transform.position;
				distance += newDistance;

				if (distance > currentWaitToInformDistance){
					float kmTime = runTime - startTimeForCurrentKm;
					PopupController.controller.Message (currentWaitToInformDistance + "m");
					fastestKm = Mathf.Max (fastestKm, kmTime);
					startTimeForCurrentKm = runTime;
					currentWaitToInformDistance += distanceInformIncrements;
				}

				if (runTime - lastCheckTime > multiplierCheckTime){
					lastCheckTime = runTime;
					checkMultiplier ();
				}
				updateScore((int)(newDistance * scorePerDistance));
			}

		}
	}

	public void updateScore(int amount, bool shouldUseMultiplier = true) {
		float effectiveMultiplier = shouldUseMultiplier ? multiplier + specialMultiplier : 1;

		score += (int)(effectiveMultiplier * amount);
		scoreText.text = "" + score;
	}

	public void checkMultiplier(bool canDecrease = true){

		if (runTime > specialMultiplierEndTime) {
			specialMultiplier = 0;
		}

		float speed = GetComponent<Rigidbody>().velocity.magnitude;
		if (speed > multiplierThresholds [Mathf.Min (multiplierThresholds.Length - 1, (int)multiplier)]) {
			multiplier++;
			multiplierText.rectTransform.DOShakeAnchorPos (2f, 5f, (int)Mathf.Lerp (10,20, multiplier / multiplierThresholds.Length), 45f).Play ();
			ChangeMultiplierText ();
		} else if (canDecrease){
			bool changed = false;
			while (speed < multiplierThresholds [Mathf.Min (multiplierThresholds.Length, (int)multiplier) - 1]) {
				multiplier--;
				changed = true;
			}
			if (changed) ChangeMultiplierText ();
		}
	}

	private void ChangeMultiplierText ()
	{
		float combinedMultiplier = multiplier + specialMultiplier;

		if (combinedMultiplier > 1) {
			multiplierText.rectTransform.eulerAngles = new Vector3(0,0,15f) * ((int)combinedMultiplier % 2 == 0 ? 1 : -1);
			multiplierText.fontSize = (int)Mathf.Lerp (30, 46, combinedMultiplier / multiplierThresholds.Length);
			multiplierText.color = Color.Lerp (lowMultiplierColor, highMultiplierColor, combinedMultiplier / multiplierThresholds.Length);
			multiplierText.text = "x" + combinedMultiplier;
		} else {
			multiplierText.DOFade (0f, 0.25f).Play();
		}
	}

	//Collisions used to determine ball airtime and award additional points
	void OnCollisionStay(){
		CheckForAirtime ();
	}

	private void CheckForAirtime () {
		if (runTime - groundLeftTime >= minAirTime) {
			int airScore = (int)((runTime - groundLeftTime) * (airTimePointsPerSecond / 10)) * 10;
			if (runTime - groundLeftTime >= 2 * minAirTime) {
				//good luck
				airScore *= 2;
				if (runTime - groundLeftTime >= 3 * minAirTime) {
					//that's a landing and a half, that is
					airScore += 500;
					PopupController.controller.Message ("Unbelievable Airtime! +" + airScore);
				} else {
					PopupController.controller.Message ("Massive Airtime! +" + airScore);
				}
			} else {
				PopupController.controller.Message ("Airtime! +" + airScore);
			}
			updateScore (airScore, false);
		}
		groundLeftTime = runTime;
	}

	public void AddSpecialMultiplier (float amount, float duration) {
		specialMultiplier += amount;
		specialMultiplierEndTime = Mathf.Max (specialMultiplierEndTime, runTime + duration);
		ChangeMultiplierText ();
	}

	public int GetScore(){
		return score;
	}

	public float GetDistance(){
		return distance;
	}

	public float GetRunTime(){
		return runTime;
	}

	public float GetFastestKmTime(){
		return fastestKm;
	}

	public void HaltScoring(){
		shouldKeepScore = false;
	}

	public void ResumeScoring(){
		shouldKeepScore = true;
	}
}
