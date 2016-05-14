using UnityEngine;
using System.Collections;

public class UpdateTunnelRunnerMultiplier : MonoBehaviour {

	public int maxIncrease = 2;
	public bool updateOnEnter = false;

	private TunnelScoreController scorer;

	void Start(){
		scorer = GameObject.FindObjectOfType<TunnelScoreController>();
	}

	void OnTriggerExit(Collider coll){
		if(coll.CompareTag("Player") && !updateOnEnter){
			for (int i = 0; i < maxIncrease; i++){
				scorer.checkMultiplier();
				Debug.Log("Initiating additional multiplier check");
			}
		}
	}

	void OnTriggerEnter(Collider coll){
		if(coll.CompareTag("Player") && updateOnEnter){
			for (int i = 0; i < maxIncrease; i++){
				scorer.checkMultiplier();
				Debug.Log("Initiating additional multiplier check");
			}
		}
	}

	void OnCollisionExit(Collision coll){
		if(coll.gameObject.CompareTag("Player") && !updateOnEnter){
			for (int i = 0; i < maxIncrease; i++){
				scorer.checkMultiplier();
				Debug.Log("Initiating additional multiplier check");
			}
		}
	}
	
	void OnCollisionEnter(Collision coll){
		if(coll.gameObject.CompareTag("Player") && updateOnEnter){
			for (int i = 0; i < maxIncrease; i++){
				scorer.checkMultiplier();
				Debug.Log("Initiating additional multiplier check");
			}
		}
	}
}
