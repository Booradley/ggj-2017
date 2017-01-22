using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Put Hand In Clock")]
public class PutHandInClockSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        Clock.onClockActivated += HandleClockActivated;

        if (!Clock.isActivated)
        {
            DialogManager.Instance.AddDialogMulti(_initialDialog);
            DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);
        }
        else
        {
            _isComplete = true;
        }
    }

    public override void Cleanup()
    {
        base.Cleanup();

        Clock.onClockActivated -= HandleClockActivated;
    }

    private void HandleClockActivated()
    {
        DialogManager.Instance.Reset();
        _isComplete = true;
    }
}