using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Segments/Use Pass Code On Door")]
public class UsePasscodeOnDoorSegment : SegmentData
{
	[System.NonSerialized]
	private Keypad _doorKeypad;

	[System.NonSerialized]
	private Keypad _safeKeypad;

	public override void Setup()
	{
		base.Setup();

		DialogManager.Instance.AddDialogMulti(new DialogData[0]);
		DialogManager.Instance.AddSecondaryDialogMulti(_randomDialog);

		_doorKeypad = GameObject.FindGameObjectWithTag("DoorLock").GetComponent<Keypad>();
		_doorKeypad.onCodeEntered += HandleCodeEntered;

		_safeKeypad = GameObject.FindGameObjectWithTag("SafeLock").GetComponent<Keypad>();
		_safeKeypad.onCodeCorrect += HandleSafeOpened;
	}

	public override void Cleanup()
	{
		base.Cleanup();

		if (_doorKeypad != null)
		{
			_doorKeypad.onCodeEntered -= HandleCodeEntered;
		}

		if (_safeKeypad != null)
		{
			_safeKeypad.onCodeCorrect -= HandleSafeOpened;
		}

		DialogManager.onAllRequiredDialogComplete -= HandleDialogComplete;
	}

	private void HandleCodeEntered(int[] currentInputCode)
	{
		int index = 0;
		bool codeIsCorrect = true;
		foreach (int keyPadButtonIndex in currentInputCode)
		{
			if (keyPadButtonIndex != _safeKeypad.correctCode[index])
			{
				codeIsCorrect = false;
				break;
			}
			index++;
		}

		if (codeIsCorrect)
		{
			_doorKeypad.onCodeEntered -= HandleCodeEntered;

			DialogManager.Instance.Reset();
			DialogManager.Instance.AddDialogMulti(_initialDialog);
			DialogManager.Instance.AddSecondaryDialogMulti(new DialogData[0]);

			DialogManager.onAllRequiredDialogComplete += HandleDialogComplete;
		}
	}

	private void HandleSafeOpened()
	{
		_safeKeypad.onCodeCorrect -= HandleSafeOpened;
		_doorKeypad.onCodeEntered -= HandleCodeEntered;

		_isComplete = true;
	}

	private void HandleDialogComplete()
	{
		DialogManager.onAllRequiredDialogComplete -= HandleDialogComplete;

		_isComplete = true;
	}
}