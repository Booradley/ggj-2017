using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu]
public class SegmentData : ScriptableObject
{
    [SerializeField]
    protected DialogData[] _initialDialog;
    public DialogData[] initialDialog {  get { return _initialDialog; } }

    [SerializeField]
    protected DialogData[] _randomDialog;
    public DialogData[] randomDialog { get { return _randomDialog; } }

    [NonSerialized]
    protected bool _isComplete = false;
    public bool isComplete { get { return _isComplete; } }

    public virtual void Setup()
    {

    }

    public virtual void Cleanup()
    {

    }
}