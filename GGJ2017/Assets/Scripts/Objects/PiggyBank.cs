using UnityEngine;
using System.Collections;
using System;

public class PiggyBank : Breakable
{
    public static event Action onPiggyBankPickedUp;
    public static event Action onPiggyBankBroken;

    public override void OnPickedUp()
    {
		base.OnPickedUp();

        if (onPiggyBankPickedUp != null)
            onPiggyBankPickedUp();
    }

	protected override void OnBroken()
    {
		base.OnBroken();

        if (onPiggyBankBroken != null)
            onPiggyBankBroken();
    }
}