using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class DialogData : ScriptableObject
{
    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField]
    private bool _interrupt;

	[SerializeField]
	private float _delay;

	public bool isInterupt { get { return _interrupt; } }
	public bool hasDelay { get { return _delay > 0; } }
	public float delay { get { return _delay; } }
}