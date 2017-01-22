using UnityEngine;
using System.Collections;
using System;
using Valve.VR.InteractionSystem;

public class Clock : MonoBehaviour
{
    public static event Action onClockActivated;

    [SerializeField]
    private Light _mainLight;

    [SerializeField]
    private Light _fillLight;

    [SerializeField]
    private GameObject _minuteHand;

    [SerializeField]
    private GameObject _hourHand;

    private void Awake()
    {
        _mainLight.enabled = false;
        _fillLight.enabled = false;

        _minuteHand.SetActive(false);
    }

    private void OnTriggerEntered(Collider collider)
    {
        if (collider.tag == "MinuteHand")
        {
            GetComponent<Collider>().enabled = false;
            _minuteHand.SetActive(true);
            collider.GetComponent<MinuteHand>().Remove();
        }
    }
}