using UnityEngine;
using System.Collections;
using System;
using Valve.VR.InteractionSystem;

public class PiggyBank : MonoBehaviour
{
    public static event Action onPiggyBankPickedUp;
    public static event Action onPiggyBankBroken;

    [SerializeField]
    private Rigidbody _mainRigidBody = null;

    [SerializeField]
    private Throwable _mainThrowable = null;

    [SerializeField]
    private Collider _mainCollider = null;

    [SerializeField]
    private GameObject _mainMesh = null;

    [SerializeField]
    private GameObject _brokenMesh = null;

    private bool _canBreak;

    private void Awake()
    {
        _brokenMesh.SetActive(false);
    }

    public void SetCanBreak(bool value)
    {
        _canBreak = value;
    }
    
    public void OnPickedUp()
    {
        if (onPiggyBankPickedUp != null)
            onPiggyBankPickedUp();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_canBreak)
            return;

        if (collision.relativeVelocity.magnitude > 5f)
        {
            _mainMesh.SetActive(false);
            _mainCollider.enabled = false;
            _mainRigidBody.isKinematic = true;

            _brokenMesh.SetActive(true);

            Rigidbody[] rigidBodies = _brokenMesh.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidBodies)
            {
                rb.AddExplosionForce(collision.relativeVelocity.magnitude * 2f, collision.contacts[0].point, 0.5f);
            }

            _canBreak = false;

            if (onPiggyBankBroken != null)
                onPiggyBankBroken();
        }
    }
}