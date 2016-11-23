using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldLevels : MonoBehaviour {

	public Text title;
	private LevelSelectManager levelSelectManager;
	private LevelIcon[] levelIcons;

	public Sprite collectedStar;
	public Sprite uncollectedStar;
	public Sprite lockedLevel;
	public Sprite uncompletedLevel;
	public Sprite completedLevel;

	void Awake(){
		//this line can go in start if we don't skip straight to the troposphere world levels screen
		levelIcons = GetComponentsInChildren<LevelIcon> ();
	}

	void Start(){
		GetComponentInChildren<Canvas> ().renderMode = RenderMode.WorldSpace;
		levelSelectManager = GetComponentInParent<LevelSelectManager> ();
	}

	public void DisplayWorldLevels (WorldData world)
	{
		//TODO the following may need to be re-enabled if we ever have multiple worlds with different skyboxes
		//this has a slight issue with the changed skybox prematurely flashing in the background of a level when return to level select is used
		//RenderSettings.skybox = world.skybox;
		title.text = world.worldName;
		for (int i = 0; i < levelIcons.Length; i++) {
			levelIcons [i].DisplayLevelInfo (world.GetXthChildData (i));
		}
	}

	public void LevelSelectedButton(int levelID){
		if (levelSelectManager.IsSafeToPress ()) {
			levelSelectManager.StartLevelInfoDisplay (levelID);
		}
	}
}
