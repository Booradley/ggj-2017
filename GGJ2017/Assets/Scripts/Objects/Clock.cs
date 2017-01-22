using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private Light _mainLight;

    [SerializeField]
    private Light _fillLight;

    private void Awake()
    {
        _mainLight.enabled = false;
        _fillLight.enabled = false;
    }
}