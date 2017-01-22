using UnityEngine;
using System.Collections;
using System;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(VelocityEstimator))]
public class HingedObject : MonoBehaviour
{
    [SerializeField]
    private Transform _hingeRoot;

    [SerializeField]
    private HingedObjectTarget _hingeTarget;

    [SerializeField]
    private float _angleOffset = 0f;

    [SerializeField]
    private Vector3 _hingeVector;
    
    public bool detachOthers = false;
    public float catchSpeedThreshold = 0.0f;
    public float wasDetachedSecondsInterval = 0.1f;
    public float distanceUntilDetach = 0.2f;

    private VelocityEstimator _velocityEstimator;
    private HingeJoint _hingeJoint;
    private float _detachTime;
    private bool _isAiming = false;
    private JointLimits _initialJointLimits;
    private float _grabDistanceFromHinge;

    private Hand _hand;
    public Hand hand
    {
        get { return _hand; }
        private set { }
    }

    private bool _canAttach = true;
    public bool canAttach
    {
        get { return _canAttach; }
    }

    public bool isAttached
    {
        get { return _hand != null; }
    }

    public bool wasRecentlyDetached
    {
        get { return !isAttached && Time.fixedTime - _detachTime <= wasDetachedSecondsInterval; }
    }

    public void Awake()
    {
        _velocityEstimator = GetComponent<VelocityEstimator>();
        _hingeJoint = GetComponent<HingeJoint>();
        _hingeTarget.Setup(OnDetachedFromHandRedirect);
    }

    public void Enable()
    {
        _canAttach = true;
    }

    public void Disable()
    {
        _canAttach = false;
    }

    public void OnHandHoverBegin(Hand hand)
    {
        if (_canAttach == true && !isAttached)
        {
            if (hand.GetStandardInteractionButton() || ((hand.controller != null) && hand.controller.GetPress(Valve.VR.EVRButtonId.k_EButton_Grip)))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb.velocity.magnitude >= catchSpeedThreshold)
                {
                    _hand = hand;
                    _hand.HoverLock(GetComponent<Interactable>());
                    _velocityEstimator.BeginEstimatingVelocity();
                    _hingeTarget.transform.position = _hand.transform.position;
                    _grabDistanceFromHinge = Vector3.Distance(_hingeRoot.transform.position, _hand.transform.position);
                    _hand.AttachObject(_hingeTarget.gameObject, Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand);
                    _isAiming = true;

                    FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = _hingeTarget.GetComponent<Rigidbody>();
                    fixedJoint.enablePreprocessing = true;
                }
            }
        }
    }

    public void HandHoverUpdate(Hand hand)
    {
        if (_canAttach == true && (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))))
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            _hand = hand;
            _hand.HoverLock(GetComponent<Interactable>());
            _velocityEstimator.BeginEstimatingVelocity();
            _hingeTarget.transform.position = _hand.transform.position;
            _grabDistanceFromHinge = Vector3.Distance(_hingeRoot.transform.position, _hand.transform.position);
            _hand.AttachObject(_hingeTarget.gameObject, Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand);
            _isAiming = true;

            FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = _hingeTarget.GetComponent<Rigidbody>();
            fixedJoint.enablePreprocessing = true;
        }
        else if (_hand != null && _canAttach == true && (hand.GetStandardInteractionButtonUp() || ((hand.controller != null) && hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))))
        {
            _hand.DetachObject(_hingeTarget.gameObject);
        }
        else if (_hand != null)
        {
            // Check for distance detach
            if (Vector3.Distance(_hand.transform.position, _hingeRoot.transform.position) - _grabDistanceFromHinge >= distanceUntilDetach)
            {
                Debug.Log("Distance Detach");
                _hand.DetachObject(_hingeTarget.gameObject);
            }
        }
    }

    public void OnDetachedFromHandRedirect(Hand hand)
    {
        _hand = null;
        _detachTime = Time.fixedTime;
        _isAiming = false;
        _grabDistanceFromHinge = 0f;

        Destroy(gameObject.GetComponent<FixedJoint>());

        hand.HoverUnlock(GetComponent<Interactable>());

        Rigidbody rb = GetComponent<Rigidbody>();
        _velocityEstimator.FinishEstimatingVelocity();
        rb.velocity = _velocityEstimator.GetVelocityEstimate();
        rb.angularVelocity = _velocityEstimator.GetAngularVelocityEstimate();
    }
}