using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Find Piggy Bank")]
public class FindPiggyBankSegmentData : SegmentData
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