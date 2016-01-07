using UnityEngine;
using System.Collections;

public class PauseMenuButton : StateMachineBehaviour {

	private PauseMenu pauseMenu;

	void Awake(){
		pauseMenu = GameObject.FindGameObjectWithTag ("LevelManager").GetComponentInChildren<PauseMenu> ();
	}
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Debug.Log (stateInfo.length);
		pauseMenu.Unpause ();
	}

}
