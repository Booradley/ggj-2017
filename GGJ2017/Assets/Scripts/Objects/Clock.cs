using UnityEngine;
using System.Collections;
using System;
using Valve.VR.InteractionSystem;

public class Clock : MonoBehaviour
{
    public static event Action onClockActivated;

    public static bool isActivated = false;

    [SerializeField]
    private ToggleLight _toggleLight;

    [SerializeField]
    private GameObject _minuteHand;

    [SerializeField]
    private GameObject _hourHand;

    [SerializeField]
    private WallPanels _wallPanels;

    private void Awake()
    {
        _minuteHand.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "MinuteHand")
        {
            GetComponent<Collider>().enabled = false;
            _minuteHand.SetActive(true);
            collider.GetComponentInParent<MinuteHand>().Remove();

            _toggleLight.SetLight(true);

            GetComponent<Animator>().SetTrigger("Activate");
            _wallPanels.PlayWallAnimation();

            isActivated = true;

            if (onClockActivated != null)
                onClockActivated();
        }
    }
}