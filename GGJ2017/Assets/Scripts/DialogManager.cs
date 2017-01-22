using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
	public static event Action onAllRequiredDialogComplete;

    private static DialogManager _instance;
    public static DialogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DialogManager>();
            }

            return _instance;
        }
    }

	[SerializeField]
	private AudioSource _dialogAudioSource = null;

	[SerializeField]
	private DialogData[] _interuptRecoveryDialog = null;

	private List<DialogData> _dialogQueue = null;
	private DialogData _currentDialog = null;
	private bool _interupted = false;

	private List<DialogData> _secondaryQueue = null;
	private int _secondaryQueuePlayCount = 0;
	private bool _allClipsPlayedOnce = false;
	private int _secondaryDialogLastIndex = -1;

	private Coroutine _updateDialogCoroutine = null;
	private bool _updateRunning = false;

	private Coroutine _playDialogCoroutine = null;
	private bool _playingDialog = false;

	private Coroutine _cancelCurrentDialogCoroutine = null;
	private bool _cancellingDialog = false;

    private void Start()
    {
		_dialogQueue = new List<DialogData>();
		_secondaryQueue = new List<DialogData>();

		StartUpdateDialogLoop();
	}

	/// <summary>
	/// Clears the dialog queue and adds the new list to the queue.
	/// </summary>
	/// <param name="dialog">Dialog List.</param>
	public void AddDialogMulti(DialogData[] dialog)
	{
		_dialogQueue.Clear();

		for (int i = 0; i < dialog.Length; ++i)
		{
			AddDialog(dialog[i]);
		}
	}

	/// <summary>
	/// Adds the dialog to the end of the queue. If interupt, it will stop the current playing dialog.
	/// </summary>
	/// <param name="dialog">Dialog.</param>
	public void AddDialog(DialogData dialog)
	{
		if (dialog.isInterupt)
		{
			InteruptDialog(dialog);
		}
		else
		{
			_dialogQueue.Add(dialog);
		}
	}

	/// <summary>
	///  Clears the secondary dialog queue and adds the new list to the queue. Only plays if it runs out of clips from the dialog queue.
	/// </summary>
	/// <param name="dialog">Dialog List.</param>
	public void AddSecondaryDialogMulti(DialogData[] dialog)
	{
		_secondaryQueue.Clear();
		_allClipsPlayedOnce = false;
		_secondaryQueuePlayCount = 0;
		_secondaryDialogLastIndex = -1;

		for (int i = 0; i < dialog.Length; ++i)
		{
			AddSecondaryDialog(dialog[i]);
		}
	}

	/// <summary>
	/// Adds dialog to the secondary dialog queue.
	/// </summary>
	/// <param name="dialog">Dialog.</param>
	public void AddSecondaryDialog(DialogData dialog)
	{
		_secondaryQueue.Add(dialog);
	}

	/// <summary>
	/// Interupts the current dialog.
	/// </summary>
	/// <param name="dialog">Dialog.</param>
	public void InteruptDialog(DialogData dialog)
	{
		if (_currentDialog != null)
		{
			CancelCurrentDialog(() => {
				PlayDialog(dialog);
			});
		}
		else
		{
			PlayDialog(dialog);
		}
	}

	/// <summary>
	/// Resets the dialog manager. Cancels all active coroutines and emptys queues. 
	/// </summary>
	public void Reset()
	{
		_dialogQueue.Clear();
		_currentDialog = null;
		_secondaryQueue.Clear();

		StopPlayDialogCoroutine();
		StopCancelCurrentDialogCoroutine();
	}
		
	//---------------------------------------------------------------------
	// Update
	//---------------------------------------------------------------------

	private void StartUpdateDialogLoop()
	{
		_updateDialogCoroutine = StartCoroutine(UpdateDialogCoroutine());
	}

	private IEnumerator UpdateDialogCoroutine()
	{
		_updateRunning = true;
		while (_updateRunning)
		{
			if (_playingDialog || _cancellingDialog)
			{
				yield return null;
				continue;
			}

			DialogData newDialog = null;
			if (_interupted)
			{
				int randomIndex = UnityEngine.Random.Range(0, _interuptRecoveryDialog.Length);
				newDialog = _interuptRecoveryDialog[randomIndex];
				_interupted = false;
			}
			else if (_dialogQueue.Count > 0)
			{
				newDialog = _dialogQueue[0];
				_dialogQueue.RemoveAt(0);
			}
			else if (_secondaryQueue.Count > 0)
			{
				if (_allClipsPlayedOnce)
				{
					// Pick a random secondary dialog to play
					if (_secondaryQueue.Count > 1 && _secondaryDialogLastIndex != -1)
					{
						int randomIndex = UnityEngine.Random.Range(0, _secondaryQueue.Count);
						if (randomIndex == _secondaryDialogLastIndex)
						{
							if (randomIndex == _secondaryQueue.Count - 1)
							{
								randomIndex -= 1;
							}
							else
							{
								randomIndex += 1;
							}
						}
						newDialog = _secondaryQueue[randomIndex];
					}
					else
					{
						int randomIndex = UnityEngine.Random.Range(0, _secondaryQueue.Count);
						newDialog = _secondaryQueue[randomIndex];
					}
				}
				else
				{
					newDialog = _secondaryQueue[0];

					// Move dialog to end of queue
					_secondaryQueue.RemoveAt(0);
					_secondaryQueue.Add(newDialog);

					_secondaryDialogLastIndex = _secondaryQueue.Count - 1;

					// Check if we have now played all clips
					_secondaryQueuePlayCount++;
					if (_secondaryQueuePlayCount >= _secondaryQueue.Count)
					{
						_allClipsPlayedOnce = true;
					}
				}
			}
				
			if (newDialog != null)
			{
				// We found a clip :D
				PlayDialog(newDialog);
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
		}

		_updateDialogCoroutine = null;
	}

	private void StopUpdateDialogCoroutine()
	{
		_updateRunning = false;
	}
		
	//---------------------------------------------------------------------
	// Play
	//---------------------------------------------------------------------

	private void PlayDialog(DialogData dialog, System.Action onComplete = null)
	{
		StopPlayDialogCoroutine();
		_playDialogCoroutine = StartCoroutine(PlayDialogCoroutine(dialog, onComplete));
	}

	private IEnumerator PlayDialogCoroutine(DialogData dialog, System.Action onComplete)
	{
		_playingDialog = true;
		DialogData prevDialog = _currentDialog;
		_currentDialog = dialog;

		_dialogAudioSource.clip = _currentDialog.dialogClip;
		_dialogAudioSource.loop = false;

		if (!_currentDialog.hasDelay || (prevDialog != null && prevDialog.isInteruptRecovery))
		{
			_dialogAudioSource.Play();
			yield return null;
		}
		else
		{
			yield return new WaitForSeconds(_currentDialog.delay);
			_dialogAudioSource.Play();
			yield return null;
		}

		while(_dialogAudioSource.isPlaying)
		{
			yield return null;
		}

		_playingDialog = false;

		if (_currentDialog.isRequired && _dialogQueue.Count == 0 && onAllRequiredDialogComplete != null)
		{
			onAllRequiredDialogComplete();
		}

		if (onComplete != null)
		{
			onComplete();
		}
	}

	private void StopPlayDialogCoroutine()
	{
		if (_playDialogCoroutine != null)
		{
			StopCoroutine(_playDialogCoroutine);
			_playDialogCoroutine = null;
		}

		_playingDialog = false;
	}

	//---------------------------------------------------------------------
	// Cancel
	//---------------------------------------------------------------------

	/// <summary>
	/// Cancels the current dialog and returns it to the front of the queue.
	/// </summary>
	/// <param name="onComplete">Called when cancel is complete.</param>
	public void CancelCurrentDialog(System.Action onComplete = null)
	{
		StopCancelCurrentDialogCoroutine();
		_cancelCurrentDialogCoroutine = StartCoroutine(CancelCurrentDialogCoroutine(onComplete));
	}

	private IEnumerator CancelCurrentDialogCoroutine(System.Action onComplete)
	{
		// Wait for next frame incase we receive multiple interupts in one frame.
		yield return null;
		_cancellingDialog = true;

		if (_currentDialog != null)
		{
			if (_dialogAudioSource.isPlaying)
			{
				_interupted = true;
			}

			if (_currentDialog.isRequired || _currentDialog.isSecondary)
			{
				_dialogQueue.Insert(0, _currentDialog);
				_dialogAudioSource.Stop();
			}
		}

		StopPlayDialogCoroutine();

		_cancellingDialog = false;

		if (onComplete != null)
		{
			onComplete();
		}
	}

	private void StopCancelCurrentDialogCoroutine()
	{
		if (_cancelCurrentDialogCoroutine != null)
		{
			StopCoroutine(_cancelCurrentDialogCoroutine);
			_cancelCurrentDialogCoroutine = null;
		}

		_cancellingDialog = false;
	}
}