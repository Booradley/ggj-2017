using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class SegmentData : ScriptableObject
{
    [SerializeField]
    private DialogData[] _initialDialog;

    [SerializeField]
    private DialogData[] _randomDialog;
}