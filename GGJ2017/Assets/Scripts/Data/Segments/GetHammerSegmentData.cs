﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Get Hammer")]
public class GetHammerSegmentData : SegmentData
{
    public override void Setup()
    {
        base.Setup();

        DialogManager.Instance.AddDialogMulti(_initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

        GameObject.FindGameObjectWithTag("MetalBox").GetComponent<MetalBox>().OpenDoor();

        Hammer.onHammerPickedUp += HandleHammerPickedUp;
    }

    public override void Cleanup()
    {
        base.Cleanup();

        Hammer.onHammerPickedUp -= HandleHammerPickedUp;
    }

    private void HandleHammerPickedUp()
    {
        _isComplete = true;
    }
}