using UnityEngine;
using System.Collections;
using System;

public class PiggyBank : MonoBehaviour
{
    public static event Action onPiggyBankPickedUp;

    public void OnPickedUp()
    {
        if (onPiggyBankPickedUp != null)
            onPiggyBankPickedUp();
    }
}