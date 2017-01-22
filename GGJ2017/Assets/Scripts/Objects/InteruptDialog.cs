using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class InteruptDialog : MonoBehaviour 
{
	[SerializeField]
	private DialogData dialog = null;

	private int _amountOfTimesPickedUp = 0;

	private void OnAttachedToHand(Hand hand)
	{
		++_amountOfTimesPickedUp;

		if (_amountOfTimesPickedUp == 1)
		{
			DialogManager.Instance.InteruptDialog(dialog);
		}
	}
}
