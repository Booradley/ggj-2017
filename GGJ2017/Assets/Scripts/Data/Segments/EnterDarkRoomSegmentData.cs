using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Enter Dark Room")]
public class EnterDarkRoomSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DarkRoomTrigger.onDarkRoomTriggerEntered += HandleDarkRoomEntered;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        DarkRoomTrigger.onDarkRoomTriggerEntered -= HandleDarkRoomEntered;
    }

    private void HandleDarkRoomEntered()
    {
        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

        DialogManager.onAllRequiredDialogComplete += HandleDialogComplete;
    }

    private void HandleDialogComplete()
    {
        DialogManager.onAllRequiredDialogComplete -= HandleDialogComplete;
        _isComplete = true;
    }
}