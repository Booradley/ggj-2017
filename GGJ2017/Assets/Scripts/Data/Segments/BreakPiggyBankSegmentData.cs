using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Break Piggy Bank")]
public class BreakPiggyBankSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        PiggyBank.onPiggyBankBroken += HandlePiggyBankBroken;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        PiggyBank.onPiggyBankBroken -= HandlePiggyBankBroken;
    }

    private void HandlePiggyBankBroken()
    {
        _isComplete = true;
    }
}