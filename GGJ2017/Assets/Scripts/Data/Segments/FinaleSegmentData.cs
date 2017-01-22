using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Finale")]
public class FinaleSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

        DialogManager.onAllRequiredDialogComplete += HandleDialogComplete;
    }

    private void HandleDialogComplete()
    {
        DialogManager.onAllRequiredDialogComplete -= HandleDialogComplete;

        _isComplete = true;
    }

    public override void Cleanup()
    {
        base.Cleanup();
    }
}