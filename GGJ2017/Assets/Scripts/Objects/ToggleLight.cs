using UnityEngine;
using System.Collections;

public class ToggleLight : MonoBehaviour 
{
	[SerializeField]
	private Light _light = null;

	[SerializeField]
	private MeshRenderer _lightbulbMeshRenderer = null;

	[SerializeField]
	private Material _lightOffMat = null;

	[SerializeField]
	private Material _lightOnMat = null;

	[SerializeField]
	private bool _startOn = true;

	public bool isOn { get { return _light.enabled; } }

	private void Start()
	{
		SetLight(_startOn);
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
}
