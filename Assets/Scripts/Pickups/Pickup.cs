using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
	public Sprite sprite;

	private int charges;

	void Awake()
	{
		Reset ();
	}

	public abstract String GetId ();
	public abstract int GetMaxCharges ();

	public void Reset()
	{
		this.charges = GetMaxCharges();
	}

	public int GetCharges() 
	{
		return charges;
	}

	public void Consume(BallController ball) 
	{
		//TODO decide whether charges should be consumed on an unsuccessful use (such as ball already in air, so no jump applied
		charges--;
		Apply (ball);
	}

	protected abstract void Apply(BallController ball);
}


