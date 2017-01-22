using UnityEngine;
using System.Collections;
using System;

public class PiggyBank : MonoBehaviour
{
    public static event Action onPiggyBankPickedUp;
    public static event Action onPiggyBankBroken;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void OnPickedUp()
    {
        if (onPiggyBankPickedUp != null)
            onPiggyBankPickedUp();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(_rigidBody.velocity.magnitude);
        if (_rigidBody.velocity.magnitude >= 1f)
        {
            if (onPiggyBankBroken != null)
                onPiggyBankBroken();
        }
    }
}