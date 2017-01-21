using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class SegmentData : ScriptableObject
{
    [SerializeField]
    protected DialogData[] _initialDialog;
    public DialogData[] initialDialog {  get { return _initialDialog; } }

    [SerializeField]
    protected DialogData[] _randomDialog;
    public DialogData[] randomDialog { get { return _randomDialog; } }

    protected bool _isComplete;
    public bool isComplete { get { return _isComplete; } }

    public virtual void Setup()
    {

    }

    public virtual void Cleanup()
    {

    }
}