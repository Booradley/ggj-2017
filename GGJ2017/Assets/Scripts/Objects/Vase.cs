using UnityEngine;
using System.Collections;
using System;

public class Vase : Breakable 
{
	public static event Action onVasePickedUp;
	public static event Action onVaseBankBroken;

	public override void OnPickedUp()
	{
		base.OnPickedUp();

		if (onVasePickedUp != null)
			onVasePickedUp();
	}

	protected override void OnBroken()
	{
		base.OnBroken();

		if (onVaseBankBroken != null)
			onVaseBankBroken();
	}
}