using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Put Hand In Clock")]
public class PutHandInClockSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

        Clock.onClockActivated += HandleClockActivated;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        Clock.onClockActivated -= HandleClockActivated;
    }

    private void HandleClockActivated()
    {
        _isComplete = true;
    }
}