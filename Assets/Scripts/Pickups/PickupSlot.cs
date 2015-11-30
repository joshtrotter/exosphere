using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickupSlot : MonoBehaviour
{

	public Image slotImage;
	public Image pickupImage;
	
	private Pickup pickup;
	private RectTransform pickupImageTransform;

	//Since the coordinates of our icons will move around during a drag we record the home coordinates so that we can reset the icon to the correct position after the drag
	private Vector3 homeCoords;

	void Awake ()
	{
		UpdateVisibility ();
		pickupImageTransform = pickupImage.rectTransform;
	}

	void Start ()
	{
		homeCoords = slotImage.rectTransform.position;
	}

	public bool IsEquipped ()
	{
		return pickup != null;
	}

	public Pickup GetPickup ()
	{
		return pickup;
	}

	public void EquipPickup (Pickup pickup)
	{
		this.pickup = pickup;
		if (pickup != null) {
			pickupImageTransform.position = homeCoords;
			this.pickupImage.sprite = pickup.sprite;
		}
		UpdateVisibility ();
	}

	public void SwapPickup (PickupSlot pickupSlot)
	{
		Pickup temp = this.pickup;
		this.EquipPickup (pickupSlot.GetPickup ());
		pickupSlot.EquipPickup (temp);
	}

	public void UsePickup(BallController ball)
	{
		if (pickup == null) {
			Debug.Log ("No pickup available for use");
		} else {
			pickup.Consume(ball);
			if (pickup.GetCharges() < 1) {
				RemovePickup();
			}
		}
	}

	public void RemovePickup()
	{
		EquipPickup (null);
	}

	public void Pulse(float pulseTime)
	{
		//Only pulse if not equipped since the pickup will already be visible otherwise
		if (!IsEquipped ()) {
			//TODO implement a real pulse
			slotImage.enabled = true;
			StartCoroutine(TimedPulse(pulseTime));
		}
	}

	private IEnumerator TimedPulse(float pulseTime) 
	{
		yield return new WaitForSeconds (pulseTime);
		UpdateVisibility ();
	}

	public void AddPickupHighlight()
	{
		//TODO temporarily just using green for the highlight - replace with something better
		if (IsEquipped ()) {
			pickupImage.CrossFadeColor (Color.green, 0.2f, true, false);
		}
	}

	public void RemovePickupHighlight()
	{
		//TODO temporarily just using green for the highlight - replace with something better
		if (IsEquipped ()) {
			pickupImage.CrossFadeColor (Color.white, 0.2f, true, false);
		}
	}

	public void AddSlotHighlight()
	{
		//TODO temporarily just using green for the highlight - replace with something better
		slotImage.CrossFadeColor (Color.green, 0.2f, true, false);
	}
	
	public void RemoveSlotHighlight()
	{
		//TODO temporarily just using green for the highlight - replace with something better
		slotImage.CrossFadeColor (Color.white, 0.2f, true, false);
	}

	public RectTransform GetTransform()
	{
		return pickupImageTransform;
	}

	public Vector3 GetHomeCoords()
	{
		return homeCoords;
	}

	public void UpdateVisibility ()
	{
		this.slotImage.enabled = IsEquipped ();
		this.pickupImage.enabled = IsEquipped ();
	}

	public void SetVisible(bool visible) 
	{
		this.slotImage.enabled = visible;
	}
	
}
