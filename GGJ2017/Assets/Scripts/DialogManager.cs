using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
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

	private List<DialogData> _dialogQueue = null;
	private List<DialogData> _secondaryQueue = null;
	private DialogData _currentDialog = null;
	private bool _interupted = false;

	private Coroutine _cancelDialogCoroutine = null;
	private bool _cancelling = true;

    private void Start()
    {
		_dialogQueue = new List<DialogData>();
		_secondaryQueue = new List<DialogData>();
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

		}
	}

	/// <summary>
	///  Clears the secondary dialog queue and adds the new list to the queue. Only plays if it runs out of clips from the dialog queue.
	/// </summary>
	/// <param name="dialog">Dialog List.</param>
	public void AddSecondaryDialogMulti(DialogData[] dialog)
	{
		_secondaryQueue.Clear();

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
					
			});

		}
	}

	/// <summary>
	/// Cancels the current dialog and returns it to the front of the queue.
	/// </summary>
	/// <param name="onComplete">Called when cancel is complete.</param>
	public void CancelCurrentDialog(System.Action onComplete)
	{
		if (_cancelDialogCoroutine != null)
		{
			StopCoroutine(_cancelDialogCoroutine);
		}

		_cancelDialogCoroutine = StartCoroutine(CancelCurrentDialogCoroutine(onComplete));
	}

	private IEnumerator CancelCurrentDialogCoroutine(System.Action onComplete)
	{
		// Wait for next frame incase we receive multiple interupts in one frame.
		yield return null;
		/*_cancelling = true;

		if (_dialogAudioSource.isPlaying)
		{
			_dialogQueue.Insert(0, _currentDialog);
			_dialogAudioSource.Stop();
		}

		_cancelling = false;

		if (onComplete != null)
		{
			onComplete();
		}*/
	}
}