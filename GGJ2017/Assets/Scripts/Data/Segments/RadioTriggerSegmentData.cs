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
        _isComplete = true;
    }
}