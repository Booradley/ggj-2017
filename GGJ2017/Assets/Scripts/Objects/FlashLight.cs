using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class FlashLight : MonoBehaviour 
{
	[SerializeField]
	private Light _light = null;

	[SerializeField]
	private MeshRenderer _lightbulbMeshRenderer = null;

	[SerializeField]
	private Material _lightOffMat = null;

	[SerializeField]
	private Material _lightOnMat = null;

	private Hand _hand = null;

	private void Start()
	{
		SetLight(false);
	}

	public void OnAttachedToHand(Hand hand)
	{
		_hand = hand;
	}

	public void OnDetachedFromHand(Hand hand)
	{
		_hand = null;
	}

	public void SetLight(bool turnOn)
	{
		_light.enabled = turnOn;

		if (turnOn)
		{
			_lightbulbMeshRenderer.material = _lightOnMat;
		}
		else
		{
			_lightbulbMeshRenderer.material = _lightOffMat;
		}
	}

	private void Update()
	{
		if (_hand != null && _hand.controller != null)
		{
			if (_hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
			{
				SetLight(!_light.enabled);
			}
		}

		// For testing
		if (Input.GetKeyDown(KeyCode.L))
		{
			SetLight(!_light.enabled);
		}
	}
}
