using UnityEngine;
using System.Collections;

public class GrindTunnelPiece : TunnelPiece {

	private const float DROP_EXTENSION_DIST = 1.294f;
	private const float STRAIGHT_EXTENSION_DIST = 16f;
	private static Vector3 SWERVE_RIGHT_EXTENSION_DIST = new Vector3 (-1.4f, 0, 22.1f);
	private static Vector3 SWERVE_LEFT_EXTENSION_DIST = new Vector3 (1.4f, 0, 22.1f);

	//Size of extra clean run sequence before required to guarantee a grind extension 
	public float autoExtendClearRunDistance = 20f;

	public GameObject dropExtension1;
	public GameObject dropExtension2;
	public GameObject dropExtension3;

	public GameObject leftSide;
	public GameObject rightSide;

	public GameObject straightExtensionLeft1;
	public GameObject straightExtensionRight1;

	public GameObject swerveRightExtensionLeft;
	public GameObject swerveRightExtensionRight;
	public GameObject swerveRightExtensionLeftStraightExtension;
	public GameObject swerveRightExtensionRightStraightExtension;

	public GameObject swerveLeftExtensionLeft;
	public GameObject swerveLeftExtensionRight;
	public GameObject swerveLeftExtensionLeftStraightExtension;
	public GameObject swerveLeftExtensionRightStraightExtension;

	public GameObject exitLeft;
	public GameObject exitRight;

	private int bodyConfiguration;
	
	public override void setup (TunnelSelectionPreferences prefs, TunnelPiece parent)
	{
		base.setup (prefs, parent);
		int minExtension = (int)((TunnelSpawnController.INSTANCE.getCurrentClearRun () - this.minClearSequenceBefore) / autoExtendClearRunDistance);
		extendDrop (minExtension);
		extendBody (minExtension);
	}

	public override void tearDown ()
	{
		base.tearDown ();
		resetDrop ();
		resetBody ();
	}

	private void extendDrop(int minExtension) {
		minExtension = Mathf.Clamp (minExtension - 1, 0, 4);
		int randExtension = Random.Range (minExtension, 4);

		if (randExtension > 0) {
			dropExtension1.SetActive(true);
			dropPipes ();
			if (randExtension > 1) {
				dropExtension2.SetActive(true);
				dropPipes ();
				if (randExtension > 2) {
					dropExtension3.SetActive(true);
					dropPipes ();
				}
			}

		}

	}

	private void extendBody(int minExtension) {
		minExtension = Mathf.Clamp (minExtension, 0, 2);
		bodyConfiguration = Random.Range (minExtension, 24);

		Debug.Log (bodyConfiguration);

		if (bodyConfiguration == 1) {
			extendStraight ();						
		} else if (bodyConfiguration == 2) {
			extendSwerveRight();
		} else if (bodyConfiguration == 3) {
			extendStraight ();
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			extendSwerveRight();
		} else if (bodyConfiguration == 4) {
			extendSwerveRight();
			extendSwerveRightStraight();
		} else if (bodyConfiguration == 5) {
			extendSwerveLeft();
		} else if (bodyConfiguration == 6) {
			extendStraight ();
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			extendSwerveLeft();
		} else if (bodyConfiguration == 7) {
			extendSwerveLeft();
			extendSwerveLeftStraight();
		} else if (bodyConfiguration >= 8) {
			extendSwerveLeft();
			extendSwerveLeftStraight();
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + SWERVE_LEFT_EXTENSION_DIST;
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + SWERVE_LEFT_EXTENSION_DIST;
			extendSwerveRight();
			extendSwerveRightStraight();
		}
	}

	private void resetDrop() {
		if (dropExtension1.activeSelf) {
			dropExtension1.SetActive(false);
			dropPipes (true);
			if (dropExtension2.activeSelf) {
				dropExtension2.SetActive(false);
				dropPipes (true);
				if (dropExtension3.activeSelf) {
					dropExtension3.SetActive(false);
					dropPipes (true);					
				}
			}
		}
	}

	private void resetBody() {
		if (bodyConfiguration == 1) {
			extendStraight(true);
		} else if (bodyConfiguration == 2) {
			extendSwerveRight(true);
		} else if (bodyConfiguration == 3) {
			extendStraight (true);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			extendSwerveRight(true);
		} else if (bodyConfiguration == 4) {
			extendSwerveRight(true);
			extendSwerveRightStraight(true);
		} else if (bodyConfiguration == 5) {
			extendSwerveLeft(true);
		} else if (bodyConfiguration == 6) {
			extendStraight (true);
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			extendSwerveLeft(true);
		} else if (bodyConfiguration == 7) {
			extendSwerveLeft(true);
			extendSwerveLeftStraight(true);
		} else if (bodyConfiguration >= 8) {
			extendSwerveLeft(true);
			extendSwerveLeftStraight(true);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position - SWERVE_LEFT_EXTENSION_DIST;
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position - SWERVE_LEFT_EXTENSION_DIST;
			extendSwerveRight(true);
			extendSwerveRightStraight(true);
		}
	}

	private void dropPipes(bool inverse = false) {
		leftSide.transform.position = leftSide.transform.position + (inverse ? Vector3.up : Vector3.down) * DROP_EXTENSION_DIST;
		rightSide.transform.position = rightSide.transform.position + (inverse ? Vector3.up : Vector3.down) * DROP_EXTENSION_DIST;
		this.endOffset = this.endOffset + (inverse ? Vector3.up : Vector3.down) * DROP_EXTENSION_DIST;
	}

	private void extendStraight(bool inverse = false) {
		straightExtensionLeft1.SetActive(!inverse);
		straightExtensionRight1.SetActive(!inverse);
		exitLeft.transform.position = exitLeft.transform.position + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
		exitRight.transform.position = exitRight.transform.position + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
		this.endOffset = this.endOffset + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
	}

	private void extendSwerveRight(bool inverse = false) {
		swerveRightExtensionLeft.SetActive(!inverse);
		swerveRightExtensionRight.SetActive(!inverse);
		exitLeft.transform.position = exitLeft.transform.position + (SWERVE_RIGHT_EXTENSION_DIST * (inverse ? -1 : 1));
		exitRight.transform.position = exitRight.transform.position + (SWERVE_RIGHT_EXTENSION_DIST * (inverse ? -1 : 1));
		this.endOffset = this.endOffset + (SWERVE_RIGHT_EXTENSION_DIST * (inverse ? -1 : 1));
	}

	private void extendSwerveRightStraight(bool inverse = false) {
		swerveRightExtensionLeftStraightExtension.SetActive(!inverse);
		swerveRightExtensionRightStraightExtension.SetActive(!inverse);
		exitLeft.transform.position = exitLeft.transform.position + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
		exitRight.transform.position = exitRight.transform.position + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
		this.endOffset = this.endOffset + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
	}

	private void extendSwerveLeft(bool inverse = false) {
		swerveLeftExtensionLeft.SetActive(!inverse);
		swerveLeftExtensionRight.SetActive(!inverse);
		exitLeft.transform.position = exitLeft.transform.position + (SWERVE_LEFT_EXTENSION_DIST * (inverse ? -1 : 1));
		exitRight.transform.position = exitRight.transform.position + (SWERVE_LEFT_EXTENSION_DIST * (inverse ? -1 : 1));
		this.endOffset = this.endOffset + (SWERVE_LEFT_EXTENSION_DIST * (inverse ? -1 : 1));
	}

	private void extendSwerveLeftStraight(bool inverse = false) {
		swerveLeftExtensionLeftStraightExtension.SetActive(!inverse);
		swerveLeftExtensionRightStraightExtension.SetActive(!inverse);
		exitLeft.transform.position = exitLeft.transform.position + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
		exitRight.transform.position = exitRight.transform.position + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
		this.endOffset = this.endOffset + (inverse ? Vector3.back : Vector3.forward) * STRAIGHT_EXTENSION_DIST;
	}

}
