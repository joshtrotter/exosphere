using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class TunnelScoreController : MonoBehaviour {

	private int score;
	private float multiplier = 1f;

	//how often the score should update
	public float updateTime = 0.2f;
	//the number of updates should be waited until the multiplier is checked;
	public float multiplierCheckTime = 2.5f;
	public float scorePerDistance = 1.1f;
	//an array of velocity values that are required to maintain/increase speed multiplier
	public float[] multiplierThresholds = new float[10]{0,16,16,24,24,36,36,48,48,60};

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

	//references to pull/push data from
	private Vector3 oldPos;
	public Text scoreText;
	public Text multiplierText;
	public Color lowMultiplierColor;
	public Color highMultiplierColor;
	//private Outline multiplierOutline;
	
	void Start() {
		oldPos = transform.position;
		score = 0;
		distance = 0f;
		runTime = 0f;
		lastCheckTime = 0f;
		groundLeftTime = 10000f;
		currentWaitToInformDistance = distanceInformIncrements;
		//multiplierOutline = multiplierText.GetComponent<Outline> ();

		StartCoroutine (controlScore ());

	}

	private IEnumerator controlScore() {
		while (true) {
			yield return new WaitForSeconds(updateTime);

			runTime += updateTime;
		
			float newDistance = Vector3.Distance (transform.position, oldPos);
			oldPos = transform.position;
			distance += newDistance;

			if (distance > currentWaitToInformDistance){
				PopupController.controller.Message (currentWaitToInformDistance + "m");
				currentWaitToInformDistance += distanceInformIncrements;
			}

			if (runTime - lastCheckTime > multiplierCheckTime){
				lastCheckTime = runTime;
				checkMultiplier ();
			}
			updateScore((int)(newDistance * scorePerDistance));


		}
	}

	public void updateScore(int amount, bool shouldUseMultiplier = true) {
		score += (int)(((shouldUseMultiplier ? 1 : 1 / multiplier) * multiplier) * amount);
		scoreText.text = "" + score;
	}

	public void checkMultiplier(){
		float speed = GetComponent<Rigidbody>().velocity.magnitude;
		if (speed > multiplierThresholds [Mathf.Min (multiplierThresholds.Length - 1, (int)multiplier)]) {
			multiplier++;
			multiplierText.rectTransform.DOShakeAnchorPos (2f, 5f, (int)Mathf.Lerp (10,20, multiplier / multiplierThresholds.Length), 45f).Play ();
			ChangeMultiplierText ();
		} else {
			bool changed = false;
			while (speed < multiplierThresholds [Mathf.Min (multiplierThresholds.Length, (int)multiplier) - 1]) {
				multiplier--;
				changed = true;
			}
			if (changed) ChangeMultiplierText ();
		}
	}

	void ChangeMultiplierText ()
	{
		if (multiplier > 1) {
			//multiplierText.rectTransform.eulerAngles = Vector3.zero;
			//multiplierText.rectTransform.DORotate (Vector3.Lerp (Vector3.zero, new Vector3 (0f, 0f, 30f), multiplier / multiplierThresholds.Length) * ((int)multiplier % 2 == 0 ? 1 : -1), 1f).Play ();
			//multiplierText.rectTransform.DORotate (new Vector3(0,0,15f) * ((int)multiplier % 2 == 0 ? 1 : -1), 1f).Play ();
			multiplierText.rectTransform.eulerAngles = new Vector3(0,0,15f) * ((int)multiplier % 2 == 0 ? 1 : -1);
			multiplierText.fontSize = (int)Mathf.Lerp (30, 46, multiplier / multiplierThresholds.Length);
			multiplierText.color = Color.Lerp (lowMultiplierColor, highMultiplierColor, multiplier / multiplierThresholds.Length);
			multiplierText.text = "x" + multiplier;
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
}
