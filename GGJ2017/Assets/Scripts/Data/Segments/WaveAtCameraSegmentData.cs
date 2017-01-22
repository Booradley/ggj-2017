using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

[CreateAssetMenu(menuName = "Segments/Wave At Camera")]
public class WaveAtCameraSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        foreach (Hand hand in Player.instance.hands)
        {
            if (hand.gameObject.activeInHierarchy)
            {
                hand.GetComponent<HandVelocity>().StartSamplingWaves();
            }
        }

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

        HandVelocity.onWaveGestureComplete += HandleWaveAtCameraComplete;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        HandVelocity.onWaveGestureComplete -= HandleWaveAtCameraComplete;

        foreach (Hand hand in Player.instance.hands)
        {
            if (hand.gameObject.activeInHierarchy)
            {
                hand.GetComponent<HandVelocity>().StopSamplingWaves();
            }
        }
    }

    private void HandleWaveAtCameraComplete()
    {
        _isComplete = true;
    }
}