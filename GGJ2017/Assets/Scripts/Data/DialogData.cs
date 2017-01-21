using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class DialogData : ScriptableObject
{
    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField]
    private bool _interrupt;
}