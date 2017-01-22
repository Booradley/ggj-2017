using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Find Piggy Bank")]
public class FindPiggyBankSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        PiggyBank.onPiggyBankPickedUp += HandlePiggyBankFound;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        PiggyBank.onPiggyBankPickedUp -= HandlePiggyBankFound;
    }

    private void HandlePiggyBankFound()
    {
        _isComplete = true;
    }
}