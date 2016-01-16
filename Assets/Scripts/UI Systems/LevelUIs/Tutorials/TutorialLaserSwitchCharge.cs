using UnityEngine;
using System.Collections;

public class TutorialLaserSwitchCharge : LaserSwitchCharge {

	//the tutorials that the switch interacts with
	public TutorialMessage.TutorialSwitchTuple[] tutorials;

	public override void TurnOn(){
		base.TurnOn ();
		foreach (TutorialMessage.TutorialSwitchTuple tutorial in tutorials){
			tutorial.tut.ExternalTriggerCall(tutorial.method);
		}
	}
}

