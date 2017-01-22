using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Find Piggy Bank")]
public class FindPiggyBankSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);
        
        PiggyBank.onPiggyBankPickedUp += HandlePiggyBankFound;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        PiggyBank.onPiggyBankPickedUp -= HandlePiggyBankFound;
    }

    private void HandlePiggyBankFound()
    {
        DialogManager.Instance.Reset();
        _isComplete = true;
    }
}