using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Break Piggy Bank")]
public class BreakPiggyBankSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();
    }

    public override void Cleanup()
    {
        base.Cleanup();
    }

    private void HandlePiggyBankFound()
    {
        _isComplete = true;
    }
}