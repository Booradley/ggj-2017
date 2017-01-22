using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;
using System;

public class KeypadButton : MonoBehaviour
{
    public event Action<int> onKeyPressed;

    private int _keyValue;
    public int keyValue { get { return _keyValue; } }

    private bool _enabled;

    public void Setup(int value)
    {
        _keyValue = value;
        _enabled = true;
    }

    public void Disable()
    {
        _enabled = false;
    }

    public Material GetMaterial()
    {
        return GetComponentInChildren<Renderer>().material;
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (!_enabled)
            return;

        if (hand.GetStandardInteractionButtonDown())
        {
            if (onKeyPressed != null)
                onKeyPressed(_keyValue);
        }
    }
}