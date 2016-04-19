using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelIcon : MonoBehaviour {

	public Image star1;
	public Image star2;
	public Image star3;
	public Image status;
	public Image background;
	public Text levelNumber;
	//public Image starSymbol;

	private LevelData currentLevel;
	private WorldLevels worldLevels;

	void Awake(){
		worldLevels = GetComponentInParent<WorldLevels> ();
		//starSymbol.sprite = worldLevels.collectedStar;
	}

	public void DisplayLevelInfo(LevelData level){
		currentLevel = level;
		SetLatestInfo ();
	}

	private void SetLatestInfo(){

		status.gameObject.SetActive(true);
		levelNumber.text = currentLevel.GetLevelID ().ToString();
		levelNumber.gameObject.SetActive(true);

		if (currentLevel.IsUnlocked ()) {
			if (currentLevel.HasBeenCompleted()){
				status.sprite = worldLevels.completedLevel;
			} else {
				status.gameObject.SetActive(false);
			}

			int starsEarned = currentLevel.GetStarsEarned ();
			star1.sprite = starsEarned >= 1 ? worldLevels.collectedStar : worldLevels.uncollectedStar;
			star2.sprite = starsEarned >= 2 ? worldLevels.collectedStar : worldLevels.uncollectedStar;
			star3.sprite = starsEarned >= 3 ? worldLevels.collectedStar : worldLevels.uncollectedStar;
			star1.gameObject.SetActive (true);
			star2.gameObject.SetActive (true);
			star3.gameObject.SetActive (true);

			if (currentLevel.GetPermanentData().levelSprite != null) {
				background.sprite = currentLevel.GetPermanentData().levelSprite;
			}

		} else {
			star1.gameObject.SetActive (false);
			star2.gameObject.SetActive (false);
			star3.gameObject.SetActive (false);

			status.sprite = worldLevels.lockedLevel;
		}
	}

	public void LevelSelectedButton(){
		worldLevels.LevelSelectedButton (currentLevel.GetLevelID ());
	}
}
