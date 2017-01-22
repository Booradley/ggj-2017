using UnityEngine;
using System.Collections;
using System;

public class RadioTrigger : MonoBehaviour
{
    public static event Action onRadioTriggerEntered;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (onRadioTriggerEntered != null)
                onRadioTriggerEntered();

            gameObject.SetActive(false);
        }
    }
}