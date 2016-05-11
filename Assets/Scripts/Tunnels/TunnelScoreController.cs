using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	
	void Start() {
		oldPos = transform.position;
		score = 0;
		distance = 0f;
		runTime = 0f;
		lastCheckTime = 0f;
		groundLeftTime = 10000f;
		StartCoroutine (controlScore ());
	}

	private IEnumerator controlScore() {
		while (true) {
			yield return new WaitForSeconds(updateTime);

			runTime += updateTime;
		
			float newDistance = Vector3.Distance (transform.position, oldPos);
			oldPos = transform.position;
			distance += newDistance;

			if (runTime - lastCheckTime > multiplierCheckTime){
				lastCheckTime = runTime;
				checkMultiplier ();
			}
			updateScore((int)(newDistance * scorePerDistance));


		}
	}

	public void updateScore(int amount, bool shouldUseMultiplier = true) {
		score += (int)(((shouldUseMultiplier ? 1 : 1 / multiplier) * multiplier) * amount);
		scoreText.text = "" + score + "\nx" + multiplier;
	}

	public void checkMultiplier(){
		float speed = GetComponent<Rigidbody>().velocity.magnitude;
		//Debug.Log (speed);
		if (speed > multiplierThresholds [(int)multiplier - 1]) {
			if ((int)multiplier < multiplierThresholds.Length) {
				multiplier++;
				//PopupController.controller.Message ("Muliplier Increased (x" + multiplier + ")");
			} 
		} else {
			while (speed < multiplierThresholds [(int)multiplier - 1]) {
				multiplier--;
			}
		}
	}

	//Collisions used to determine ball airtime and award additional points
	/*void OnCollisionEnter(Collision coll){
		if (runTime - groundLeftTime >= minAirTime) {
			int airScore = (int)((runTime - groundLeftTime) * (airTimePointsPerSecond / 10)) * 10;
			if (runTime - groundLeftTime >= 2 * minAirTime) { //good luck
				airScore *= 2;
				PopupController.controller.Message ("Massive Airtime! +" + airScore);
			} else {
				PopupController.controller.Message ("Airtime! +" + airScore);
			}
			updateScore(airScore, false);
			groundLeftTime = runTime; //prevent double-dipping
		}
	}

	void OnCollisionExit(Collision coll){
		groundLeftTime = runTime;
	}*/
	void OnCollisionStay(){
		if (runTime - groundLeftTime >= minAirTime) {
			int airScore = (int)((runTime - groundLeftTime) * (airTimePointsPerSecond / 10)) * 10;
			if (runTime - groundLeftTime >= 2 * minAirTime) { //good luck
				airScore *= 2;
				PopupController.controller.Message ("Massive Airtime! +" + airScore);
			} else {
				PopupController.controller.Message ("Airtime! +" + airScore);
			}
			updateScore(airScore, false);
			groundLeftTime = runTime; //prevent double-dipping
		}
		groundLeftTime = runTime;
	}
}
