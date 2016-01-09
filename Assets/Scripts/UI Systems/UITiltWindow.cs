using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class UITiltWindow : MonoBehaviour {

	public Vector2 range = new Vector2(5f, 3f);
	
	Transform mTrans;
	Quaternion mStart;
	Vector2 mRot = Vector2.zero;
	
	void Start ()
	{
		mTrans = transform;
		mStart = mTrans.localRotation;
	}
	
	void Update ()
	{
		float y = CrossPlatformInputManager.GetAxis("Vertical");
		float x = CrossPlatformInputManager.GetAxis("Horizontal");
		
		float halfWidth = Screen.width * 0.5f;
		float halfHeight = Screen.height * 0.5f;
		x = Mathf.Clamp((x - halfWidth) / halfWidth, -1f, 1f);
		y = Mathf.Clamp((y - halfHeight) / halfHeight, -1f, 1f);
		mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 5f);
		
		mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);
	}
}
