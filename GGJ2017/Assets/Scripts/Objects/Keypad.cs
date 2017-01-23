using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Keypad : MonoBehaviour
{
	public event Action onCodeCorrect;
	public event Action<int[]> onCodeEntered;

    [SerializeField]
    private KeypadButton _button1;

    [SerializeField]
    private KeypadButton _button2;

    [SerializeField]
    private KeypadButton _button3;

    [SerializeField]
    private KeypadButton _button4;

    [SerializeField]
    private KeypadButton _button5;

    [SerializeField]
    private KeypadButton _button6;

    [SerializeField]
    private Indicator _indicator1;

    [SerializeField]
    private Indicator _indicator2;

    [SerializeField]
    private Indicator _indicator3;

    [SerializeField]
    private Indicator _codeCorrectIndicator;

    [SerializeField]
    private Material _correctMaterial;

    [SerializeField]
    private Material _wrongMaterial;

    [SerializeField]
    private int[] _safeCode;

    [SerializeField]
    private AudioClip _keyPressSound;

    [SerializeField]
    private AudioClip _codeCorrectSound;

    [SerializeField]
    private AudioClip _codeWrongSound;

    private AudioSource _audioSource;
    private List<KeypadButton> _buttons;
    private List<Indicator> _indicators;
    private List<int> _currentInputCode = new List<int>();
    private Coroutine _checkCodeCoroutine;

	public int[] correctCode { get { return _safeCode; } }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _buttons = new List<KeypadButton>() { _button1, _button2, _button3, _button4, _button5, _button6 };
        _indicators = new List<Indicator>() { _indicator1, _indicator2, _indicator3 };

        int index = 0;
        foreach(KeypadButton button in _buttons)
        {
            button.Setup(index);
            button.onKeyPressed += HandleButtonPressed;
            index++;
        }
    }

    private void Start()
    {
        UpdateCurrentInputCode();
    }

    public void Disable()
    {
        foreach (KeypadButton button in _buttons)
        {
            button.Disable();
            button.onKeyPressed -= HandleButtonPressed;
        }
    }

    private void UpdateCurrentInputCode()
    {
        foreach (Indicator indicator in _indicators)
        {
            indicator.Clear();
        }

        _codeCorrectIndicator.Clear();

        int index = 0;
        foreach(int keyPadButtonIndex in _currentInputCode)
        {
            _indicators[index].Display(_buttons[keyPadButtonIndex].GetMaterial());
            index++;
        }
    }

    private void HandleButtonPressed(int index)
    {
        _audioSource.PlayOneShot(_keyPressSound);

        if (_checkCodeCoroutine != null)
            return;

        if (_currentInputCode.Count < 3)
        {
            _currentInputCode.Add(index);
            UpdateCurrentInputCode();
        }

        if (_currentInputCode.Count == 3)
        {
            _checkCodeCoroutine = StartCoroutine(CheckCode());
        }
    }

    private IEnumerator CheckCode()
    {
        int index = 0;
        bool codeIsCorrect = true;
        foreach (int keyPadButtonIndex in _currentInputCode)
        {
            if (keyPadButtonIndex != _safeCode[index])
            {
                codeIsCorrect = false;
                break;
            }
            index++;
        }

        yield return new WaitForSeconds(1f);

        if (codeIsCorrect)
        {
            _audioSource.PlayOneShot(_codeCorrectSound);
            _codeCorrectIndicator.Display(_correctMaterial);

            if (onCodeCorrect != null)
                onCodeCorrect();
        }
        else
        {
            _audioSource.PlayOneShot(_codeWrongSound);
            _codeCorrectIndicator.Display(_wrongMaterial);
        }

		if (onCodeEntered != null)
			onCodeEntered(_currentInputCode.ToArray());

        yield return new WaitForSeconds(1f);

        _currentInputCode.Clear();
        UpdateCurrentInputCode();

        _checkCodeCoroutine = null;
    }
}