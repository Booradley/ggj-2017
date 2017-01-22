using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Radio Trigger")]
public class RadioTriggerSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        RadioTrigger.onRadioTriggerEntered += HandleRadioTriggerEntered;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        RadioTrigger.onRadioTriggerEntered -= HandleRadioTriggerEntered;
    }

    private void HandleRadioTriggerEntered()
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