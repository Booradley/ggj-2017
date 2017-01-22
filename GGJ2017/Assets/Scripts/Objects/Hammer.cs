using UnityEngine;
using System.Collections;
using System;

public class Hammer : MonoBehaviour
{
    public static event Action onHammerPickedUp;

    public void OnPickedUp()
    {
        if (onHammerPickedUp != null)
            onHammerPickedUp();
    }
}