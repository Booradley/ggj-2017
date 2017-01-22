using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Look For Manual")]
public class LookForManualSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);
        
        TreeTrigger.onTreeTriggerEntered += HandleTreeEntered;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        TreeTrigger.onTreeTriggerEntered -= HandleTreeEntered;
    }

    private void HandleTreeEntered()
    {
        _isComplete = true;
    }
}