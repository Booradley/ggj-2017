using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class LightSwitch : MonoBehaviour 
{
	[SerializeField]
	private ToggleLight _toggleLight = null;

	[SerializeField]
	private GameObject _onSwitch = null;

	[SerializeField]
	private GameObject _offSwitch = null;

	[SerializeField]
	private bool _startOn = true;

	public bool isOn { get { return _onSwitch.activeInHierarchy; } }

	private void Start()
	{
		SetOn(_startOn);
	}

	private void HandHoverUpdate(Hand hand)
	{
		if (hand != null && hand.controller != null)
		{
			if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
			{
				SetOn(!isOn);
			}
		}
	}

	public void SetOn(bool turnOn)
	{
		if (turnOn)
		{
			_offSwitch.SetActive(false);
			_onSwitch.SetActive(true);
		}
		else 
		{
			_offSwitch.SetActive(true);
			_onSwitch.SetActive(false);
		}

		_toggleLight.SetLight(turnOn);
	}
}
