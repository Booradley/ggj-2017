using UnityEngine;
using System.Collections;
using System;

public class PiggyBank : MonoBehaviour
{
    public static event Action onPiggyBankPickedUp;
    public static event Action onPiggyBankBroken;
    
    public void OnPickedUp()
    {
        if (onPiggyBankPickedUp != null)
            onPiggyBankPickedUp();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 8f)
        {
            if (onPiggyBankBroken != null)
                onPiggyBankBroken();
        }
    }
}