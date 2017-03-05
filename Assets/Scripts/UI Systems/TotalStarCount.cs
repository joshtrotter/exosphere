using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TotalStarCount : MonoBehaviour {

	public Text numStars;

	void Start(){
		OnLevelWasLoaded ();
	}

	void OnLevelWasLoaded(){
		numStars.text = LevelDataManager.manager.CountAllStarsEarned ().ToString ();
	}
}
