using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Find Safe")]
public class FindSafeSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

        Safe.onSafeOpened += HandleSafeOpened;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        Safe.onSafeOpened -= HandleSafeOpened;
    }

    private void HandleSafeOpened()
    {
        _isComplete = true;
    }
}