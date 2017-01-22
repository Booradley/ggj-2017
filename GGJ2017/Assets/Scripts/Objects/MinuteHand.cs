using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class MinuteHand : MonoBehaviour
{
    private Hand _hand;

    private void OnAttachedToHand(Hand hand)
    {
        _hand = hand;
    }

    private void OnDetachedFromHand(Hand hand)
    {
        _hand = null;
    }

    public void Remove()
    {
        if (_hand != null)
        {
            _hand.DetachObject(gameObject);
            _hand = null;
        }

        Destroy(gameObject);
    }
}