using UnityEngine;
using System.Collections;
using System;
using Valve.VR.InteractionSystem;

public class Breakable : MonoBehaviour 
{
	[SerializeField]
	protected Rigidbody _mainRigidBody = null;

	[SerializeField]
	protected Throwable _mainThrowable = null;

	[SerializeField]
	protected Collider _mainCollider = null;

	[SerializeField]
	protected GameObject _mainMesh = null;

	[SerializeField]
	protected GameObject _brokenMesh = null;

	[SerializeField]
	private AudioClip _breakingSFX = null;

	[SerializeField]
	private AudioSource _audioSource = null;

	[SerializeField]
	protected bool _canBreakOnAwake = false;

	protected bool _canBreak;

	protected void Awake()
	{
		_brokenMesh.SetActive(false);

		SetCanBreak(_canBreakOnAwake);
	}

	public void SetCanBreak(bool value)
	{
		_canBreak = value;
	}

	public virtual void OnPickedUp()
	{
	}

	protected void OnCollisionEnter(Collision collision)
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

			OnBroken();
		}
	}

	protected virtual void OnBroken()
	{
		if (_audioSource != null && _breakingSFX != null)
		{
			_audioSource.clip = _breakingSFX;
			_audioSource.Play();
		}
	}
}
