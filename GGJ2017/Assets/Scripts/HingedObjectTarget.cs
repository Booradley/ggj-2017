using UnityEngine;
using System.Collections;
using System;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class HingedObjectTarget : MonoBehaviour
{
    private Action<Hand> _onDetached;
    private Vector3 _initialHingeTargetPosition;

    public void Setup(Action<Hand> onDetached)
    {
        _onDetached = onDetached;
        _initialHingeTargetPosition = transform.position;
    }

    private void OnDetachedFromHand(Hand hand)
    {
        transform.position = _initialHingeTargetPosition;

        if (_onDetached != null)
        {
            _onDetached(hand);
        }
    }

    public void HandAttachedUpdate(Hand hand)
    {
        if (hand.GetStandardInteractionButtonUp() || ((hand.controller != null) && hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip)))
        {
            hand.DetachObject(gameObject);
        }
    }
}