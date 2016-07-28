using UnityEngine;
using System.Collections;

public class GrindTunnelPiece : TunnelPiece {

	private const float DROP_EXTENSION_DIST = 1.294f;
	private const float STRAIGHT_EXTENSION_DIST = 16f;
	private static Vector3 SWERVE_RIGHT_EXTENSION_DIST = new Vector3 (-1.4f, 0, 22.1f);
	private static Vector3 SWERVE_LEFT_EXTENSION_DIST = new Vector3 (1.4f, 0, 22.1f);

	public int swerveBucketLevel = 4;

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
		//extendDrop (prefs);
		extendBody (prefs);
	}

	public override void tearDown ()
	{
		base.tearDown ();
		//resetDrop ();
		resetBody ();
	}

	public override float length(){
		//tricks the tunnelSpawnController into extending the tunnel afterwards to ensure
		//that there is always something waiting at the end of the grind rails
		return 0f;
	}

	//Not used
	private void extendDrop(TunnelSelectionPreferences prefs) {
		int randExtension = Random.Range (0, 20);
		randExtension = (int) (randExtension * Mathf.Clamp01 (prefs.maxDifficulty));

		if (randExtension > 8) {
			dropExtension1.SetActive(true);
			dropPipes ();
			if (randExtension > 15) {
				dropExtension2.SetActive(true);
				dropPipes ();
				if (randExtension > 18) {
					dropExtension3.SetActive(true);
					dropPipes ();
				}
			}

		}

	}

	private void extendBody(TunnelSelectionPreferences prefs) {
		bodyConfiguration = Random.Range (0, 23);

		if (TunnelSpawnController.INSTANCE.getCurrentClearRun () > 60f) {
			bodyConfiguration = Mathf.Clamp(bodyConfiguration, 5, bodyConfiguration);
		}

		if (prefs.maxBucketLevel <= swerveBucketLevel) {
			bodyConfiguration = Mathf.Clamp(bodyConfiguration, 0, 10);
		}

		if (bodyConfiguration >= 5 && bodyConfiguration <= 10) {
			extendStraight ();						
		} else if (bodyConfiguration == 11) {
			extendSwerveRight();
		} else if (bodyConfiguration <= 13) {
			extendStraight ();
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			extendSwerveRight();
		} else if (bodyConfiguration <= 15) {
			extendSwerveRight();
			extendSwerveRightStraight();
		} else if (bodyConfiguration == 16) {
			extendSwerveLeft();
		} else if (bodyConfiguration <= 18) {
			extendStraight ();
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			extendSwerveLeft();
		} else if (bodyConfiguration <= 20) {
			extendSwerveLeft();
			extendSwerveLeftStraight();
		} else if (bodyConfiguration == 21) {
			extendSwerveLeft();
			extendSwerveLeftStraight();
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + SWERVE_LEFT_EXTENSION_DIST;
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + SWERVE_LEFT_EXTENSION_DIST;
			extendSwerveRight();
			extendSwerveRightStraight();
		} else if (bodyConfiguration == 22) {
			extendSwerveRight();
			extendSwerveRightStraight();
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position + (Vector3.forward * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position + SWERVE_RIGHT_EXTENSION_DIST;
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position + SWERVE_RIGHT_EXTENSION_DIST;
			extendSwerveLeft();
			extendSwerveLeftStraight();
		}
	}

	//Not used
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
		if (bodyConfiguration >= 5 && bodyConfiguration <= 10) {
			extendStraight(true);
		} else if (bodyConfiguration == 11) {
			extendSwerveRight(true);
		} else if (bodyConfiguration <= 13) {
			extendStraight (true);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			extendSwerveRight(true);
		} else if (bodyConfiguration <= 15) {
			extendSwerveRight(true);
			extendSwerveRightStraight(true);
		} else if (bodyConfiguration == 16) {
			extendSwerveLeft(true);
		} else if (bodyConfiguration <= 18) {
			extendStraight (true);
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			extendSwerveLeft(true);
		} else if (bodyConfiguration <= 20) {
			extendSwerveLeft(true);
			extendSwerveLeftStraight(true);
		} else if (bodyConfiguration == 21) {
			extendSwerveLeft(true);
			extendSwerveLeftStraight(true);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveRightExtensionLeft.transform.position = swerveRightExtensionLeft.transform.position - SWERVE_LEFT_EXTENSION_DIST;
			swerveRightExtensionRight.transform.position = swerveRightExtensionRight.transform.position - SWERVE_LEFT_EXTENSION_DIST;
			extendSwerveRight(true);
			extendSwerveRightStraight(true);
		} else if (bodyConfiguration == 22) {
			extendSwerveRight(true);
			extendSwerveRightStraight(true);
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position + (Vector3.back * STRAIGHT_EXTENSION_DIST);
			swerveLeftExtensionLeft.transform.position = swerveLeftExtensionLeft.transform.position - SWERVE_RIGHT_EXTENSION_DIST;
			swerveLeftExtensionRight.transform.position = swerveLeftExtensionRight.transform.position - SWERVE_RIGHT_EXTENSION_DIST;
			extendSwerveLeft(true);
			extendSwerveLeftStraight(true);
		}
	}

	//Not used
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
