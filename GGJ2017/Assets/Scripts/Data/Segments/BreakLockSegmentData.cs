using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Break Lock")]
public class BreakLockSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

        LockedDoor.onLockBroken += HandleLockBroken;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        LockedDoor.onLockBroken -= HandleLockBroken;
    }

    private void HandleLockBroken()
    {
        _isComplete = true;
    }
}