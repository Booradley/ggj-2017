using UnityEngine;
using System.Collections;
using System;

public class Cup : Breakable 
{
	public static event Action onCupPickedUp;
	public static event Action onCupBankBroken;

	public override void OnPickedUp()
	{
		base.OnPickedUp();

		if (onCupPickedUp != null)
			onCupPickedUp();
	}

	protected override void OnBroken()
	{
		base.OnBroken();

		if (onCupBankBroken != null)
			onCupBankBroken();
	}
}
