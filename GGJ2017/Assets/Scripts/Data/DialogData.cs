using UnityEngine;
using System.Collections;

public enum DialogType
{
	Required = 0,
	Secondary = 1,
	Interupt = 2,
	InteruptRecovery = 3
}

[CreateAssetMenu]
public class DialogData : ScriptableObject
{
    [SerializeField]
    private AudioClip _audioClip = null;

	[SerializeField]
	private float _delay = 0f;

	[SerializeField]
	private DialogType _dialogType = DialogType.Required;

	public AudioClip dialogClip { get { return _audioClip; } }
	public bool isRequired { get { return _dialogType == DialogType.Required; } }
	public bool isSecondary { get { return _dialogType == DialogType.Secondary; } }
	public bool isInterupt { get { return _dialogType == DialogType.Interupt; } }
	public bool isInteruptRecovery { get { return _dialogType == DialogType.InteruptRecovery; } }
	public bool hasDelay { get { return _delay > 0f; } }
	public float delay { get { return _delay; } }
	public DialogType dialogType { get { return _dialogType; } }
}