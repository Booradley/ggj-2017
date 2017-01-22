using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Keypad : MonoBehaviour
{
    public event Action onCodeCorrect;

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

    private List<KeypadButton> _buttons;
    private List<Indicator> _indicators;
    private List<int> _currentInputCode = new List<int>();
    private Coroutine _checkCodeCoroutine;

    private void Awake()
    {
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
        if (_checkCodeCoroutine != null)
            return;

        if (_currentInputCode.Count < 3)
        {
            Debug.LogFormat("ADD INDEX {0}", index);
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
            Debug.LogFormat("{0}, {1}", keyPadButtonIndex, _safeCode[index]);
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
            _codeCorrectIndicator.Display(_correctMaterial);
            Debug.Log("RIGHT");

            if (onCodeCorrect != null)
                onCodeCorrect();
        }
        else
        {
            Debug.Log("WRONG");
            _codeCorrectIndicator.Display(_wrongMaterial);
        }

        yield return new WaitForSeconds(1f);

        _currentInputCode.Clear();
        UpdateCurrentInputCode();

        _checkCodeCoroutine = null;
    }
}