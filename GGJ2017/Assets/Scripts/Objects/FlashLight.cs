using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class FlashLight : MonoBehaviour 
{
	[SerializeField]
	private ToggleLight _toggleLight = null;

	private Hand _hand = null;

	public void OnAttachedToHand(Hand hand)
	{
		_hand = hand;
	}

	public void OnDetachedFromHand(Hand hand)
	{
		_hand = null;
	}

	private void Update()
	{
		if (_hand != null && _hand.controller != null)
		{
			if (_hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
			{
				_toggleLight.SetLight(!_toggleLight.isOn);
			}
		}

		// For testing
		if (Input.GetKeyDown(KeyCode.L))
		{
			_toggleLight.SetLight(!_toggleLight.isOn);
		}
	}
}
