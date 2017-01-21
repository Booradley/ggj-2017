using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Wave At Camera")]
public class WaveAtCameraSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        HandVelocity.onWaveGestureComplete += HandleWaveAtCameraComplete;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        HandVelocity.onWaveGestureComplete -= HandleWaveAtCameraComplete;
    }

    private void HandleWaveAtCameraComplete()
    {
        _isComplete = true;
    }
}