using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandPhysicsToolkit.Helpers.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;



public class Interactor : MonoBehaviour, BaseInteractor
{

    public bool isIntersecting = false;
    private List<Interactible> intersectedObjects = new List<Interactible>();
    private List<Interactible> _grabbedObjects = new List<Interactible>();
    public bool isLeftHand = true;

    public Color _highlightOnTouch = Color.cyan;
    public Color _highlightOnGrab = Color.green;

    public OVRHand ovrHand; // the left or right hand prefab respectively
    private OVRSkeleton handSkeleton;
    private float _dampenedPinchValue = 0.0f;
    private float _lastValidGripValue = 0.0f;
    private float _pinchVelocity;
    [SerializeField] private float _smoothTime = 0.2f;

    private bool IsHandTracking = false;
    private bool wasHandTracking = false; // used to detect hand tracking toggle

    [SerializeField] public UnityEvent onHandTrackingActive;
    [SerializeField] public UnityEvent onHandTrackingInactive;
    private bool isGrabbing = false;
    private bool wasPinchingOrGripping = false;
    private HandGrabType currentHandTrackingGesture = HandGrabType.Pinch;

    private Vector3 _initialScale;
    private Vector3 _initialPosition = Vector3.zero;
    private Quaternion _initialRotation;
  //  private OutlineBehaviour _outlineBehaviour;
    private MaterialSwitcher _materialSwitcher;

    private bool isGrabbingOther = false;
    private bool grabbingPage = false; // whether or not currently grabbing a page

    public HandGrabType GetHandGrabType()
    {
        return currentHandTrackingGesture;
    }

    private void Awake()
    {
        _initialScale = this.transform.localScale;
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
     //   _outlineBehaviour = GetComponent<OutlineBehaviour>();
        _materialSwitcher = GetComponent<MaterialSwitcher>();

        onHandTrackingActive = new UnityEvent();
        onHandTrackingInactive = new UnityEvent();
    }

    private void Start()
    {
        StartCoroutine(DelayedHandTrackingEventInvoke());
    }

    IEnumerator DelayedHandTrackingEventInvoke()
    {
        yield return new WaitForSecondsRealtime(3);

        IsHandTracking = OVRInput.GetActiveController() == OVRInput.Controller.Hands;
        HandTrackingChange(IsHandTracking);
        wasHandTracking = IsHandTracking;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateHandTrackingStatus();

        if (IsHandTracking)
        {
            SetInteractorTransform();
            DoHandTrackingUpdate();
        }
        else DoControllerUpdate();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool GetIsLeftHand()
    {
        return isLeftHand;
    }

    public bool GetIsHandTracking()
    {
        return IsHandTracking;
    }

    private void SetInteractorTransform()
    {
        if (currentHandTrackingGesture == HandGrabType.Pinch)
        {
            transform.position = GetBonePosition(OVRSkeleton.BoneId.Hand_IndexTip);
        }
        else
        {
            var middleBonePos = GetBonePosition(OVRSkeleton.BoneId.Hand_Middle1);
            var wristBonePos = GetBonePosition(OVRSkeleton.BoneId.Hand_WristRoot);

            var transform1 = transform;
            var position = (middleBonePos + wristBonePos) / 2;
            if (isLeftHand)
            {
                position += (transform1.up * 2);
            }
            else
            {
                position += (transform1.up * -2);
            }


            transform1.position = position;
        }
    }

    public Vector3 GetBonePosition(OVRSkeleton.BoneId boneId)
    {
        if (!handSkeleton) handSkeleton = ovrHand.GetComponentInChildren<OVRSkeleton>();
        if (!handSkeleton) return Vector3.zero;
        if (handSkeleton.Bones == null) return Vector3.zero;

        return handSkeleton.Bones.Count == 0 ? Vector3.zero : handSkeleton.Bones[(int)boneId].Transform.position;
    }

    public Transform GetPointerPose()
    {
        if (!ovrHand) return null;
        return ovrHand.IsPointerPoseValid ? ovrHand.PointerPose : null;
    }

    private void DoControllerUpdate()
    {
        var triggerType = isLeftHand ? OVRInput.Axis1D.PrimaryHandTrigger : OVRInput.Axis1D.SecondaryHandTrigger;

        var controllerHandedness = isLeftHand ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;

        if (OVRInput.Get(triggerType, OVRInput.Controller.Touch) > 0.7f || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controllerHandedness) > 0.7f)
        {
            foreach (var obj in intersectedObjects)
            {
                if (!isGrabbingOther || objIsPage(obj) == grabbingPage)
                {
                    DoGrab(obj, HandGrabType.Controller);
                    if (objIsPage(obj)) grabbingPage = true;
                    isGrabbingOther = true;
                }
            }
            isGrabbingOther = true; 
        }
        else
        {
            var cachedList = new List<Interactible>(_grabbedObjects);
            foreach (var obj in cachedList)
            {
                ungrabPage(obj);
                DoUngrab(obj, HandGrabType.Controller);
            }
            isGrabbingOther = false;
        }
    }

    private bool DoGrab(Interactible objtoGrab, HandGrabType handGrabType)
    {
        if (_grabbedObjects.Contains(objtoGrab))
        {
            return false;
        }

        if (!objtoGrab.TryGrab(this, handGrabType))
        {
            return false;
        }

        isGrabbing = true;
        _grabbedObjects.Add(objtoGrab);

        /*
        if (!objIsPage(objtoGrab))
        {
            // Set the interactor as the parent of the grabbed object
            objtoGrab.transform.parent = transform;
        }
        */

        GetComponent<MaterialSwitcher>().TurnOnHighlight(_highlightOnGrab);

        return true;
    }

    private bool DoUngrab(Interactible objtoGrab, HandGrabType handGrabType)
    {
        if (!_grabbedObjects.Contains(objtoGrab))
        {
            return false;
        }

        if (!objtoGrab.TryUngrab(this, handGrabType))
        {
            return false;
        }

        _grabbedObjects.Remove(objtoGrab);
        isGrabbing = false;
        /*
        if (objtoGrab != null && objtoGrab.transform.parent == transform)
        {
            // Set the object's parent to null to detach it from the interactor
            objtoGrab.transform.parent = null;
        }
        */
        GetComponent<MaterialSwitcher>().TurnOffHighlight();

        return true;
    }

    private void DoHandTrackingUpdate()
    {
        var pinchValue = GetIndexPinch(); // the strength of the pinch (values in range [0,1])
        var handGripValue = GetHandGripStrength(); // thhe strength of grip

        var isPinching = (pinchValue > handGripValue); 
        currentHandTrackingGesture = isPinching ? HandGrabType.Pinch : HandGrabType.Grip;


        var isSuccessfulPinch = pinchValue > 0.7f; // changed: was 0.8
        var isSuccessfulGrip = handGripValue > 0.7f; // changed: was 0.8

        if (isSuccessfulGrip || isSuccessfulPinch) 
        {
            foreach (var obj in intersectedObjects)
            {
                if (DoGrab(obj, currentHandTrackingGesture))
                {
                    wasPinchingOrGripping = true;
                }
                  
            }
            isGrabbingOther = true;
        }
        else if (wasPinchingOrGripping)
        {
            var cachedList = new List<Interactible>(_grabbedObjects);

            foreach (var obj in cachedList)
            {
                ungrabPage(obj);
                if (DoUngrab(obj, HandGrabType.Pinch)) // check if it was pinching
                {
                    wasPinchingOrGripping = false;
                }
                if (DoUngrab(obj, HandGrabType.Grip)) // check if it was gripping
                {
                    wasPinchingOrGripping = false;
                }
            }
            isGrabbingOther = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactible = other.gameObject.GetComponent<Interactible>();
        if (!interactible) return;
        if (isGrabbing /*&& other != simPageCollider*/) return;
        //if (isIntersecting) return; // trying to prevent from grabbing another thing 

        isIntersecting = true;
        intersectedObjects.Add(interactible);

        interactible.OnTouch.Invoke(this);

        //change interactor sphere's material
        if (!isGrabbing)
            GetComponent<MaterialSwitcher>().TurnOnHighlight(_highlightOnTouch);
    }

    private void OnTriggerExit(Collider other)
    {
        var interactible = other.gameObject.GetComponent<Interactible>();
        if (!interactible) return;

        isIntersecting = false;
        intersectedObjects.Remove(interactible);

        interactible.OffTouch.Invoke(this);

        //change interactor sphere's material
        if (!isGrabbing)
            GetComponent<MaterialSwitcher>().TurnOffHighlight();
    }


    private bool objIsPage(Interactible objtoGrab)
    {
        /* //if (isIntersecting) return false; // trying to prevent from grabbing another thing 
         var leftPage = leftCollider.gameObject.GetComponent<Interactible>();
         var rightPage = rightCollider.gameObject.GetComponent<Interactible>();
         var simPage = simPageCollider.gameObject.GetComponent<Interactible>();

         if (objtoGrab == leftPage || objtoGrab == rightPage || objtoGrab == simPage)
         {
             return true;

         }

         return false;*/
        return false;
    }

    private void ungrabPage(Interactible objtoGrab)
    {
       /* var leftPage = leftCollider.gameObject.GetComponent<Interactible>();
        var rightPage = rightCollider.gameObject.GetComponent<Interactible>();
        var simPage = simPageCollider.gameObject.GetComponent<Interactible>();

        if (objtoGrab == leftPage || objtoGrab == rightPage || objtoGrab == simPage)
        {
            grabbingPage = false;
        }*/
    }


    #region Hand Tracking Code

    private void UpdateHandTrackingStatus()
    {
        IsHandTracking = OVRInput.GetActiveController() == OVRInput.Controller.Hands;

        if (IsHandTracking != wasHandTracking)
        {
            HandTrackingChange(IsHandTracking);
        }

        wasHandTracking = IsHandTracking;
    }

    void HandTrackingChange(bool isHandTracking)
    {
        if (isHandTracking)
        {
            // make interactor sphere larger
            transform.localScale = _initialScale * 2;
            _materialSwitcher.enabled = false;

            

            // turn outline off on interactor sphere
            gameObject.layer = LayerMask.NameToLayer("Default");

            onHandTrackingActive.Invoke();
        }
        else
        {
            _materialSwitcher.enabled = true;

            // turn outline on on interactor sphere
           // gameObject.layer = LayerMask.NameToLayer("OutlineLayer");

            transform.localScale = _initialScale;
            transform.localPosition = _initialPosition;
            transform.localRotation = _initialRotation;
            onHandTrackingInactive.Invoke();
        }
    }

    // Returns values [0,1]
    public float GetIndexPinch()
    {
        if (!ovrHand)
        {
            return 0.0f;
        }
        if (!handSkeleton) handSkeleton = ovrHand.GetComponentInChildren<OVRSkeleton>();

        // check hand tracking status one more time before attempting to get strength, just to make sure
        UpdateHandTrackingStatus();

        if (!IsHandTracking)
        {
            return 0.0f;
        }

        var handConfidence = ovrHand.GetFingerConfidence(OVRHand.HandFinger.Index); // gets tracking confidence level of the system
        var isHandBeingTracked = ovrHand.IsTracked;

        if (handConfidence == OVRHand.TrackingConfidence.Low || isHandBeingTracked == false)
        {
            return 0.0f;
        }

        float indexFingerPinchStrength = ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Index); // [0, 1] where 0 is not pinching

        return indexFingerPinchStrength;
    }

    // Returns values [0,1]
    public float GetHandGripStrength()
    {
        // if no hand tracking hand exists, return 0 strength
        if (!ovrHand)
        {
            return 0.0f;
        }
        // get the skeleton from the hand
        if (!handSkeleton) handSkeleton = ovrHand.GetComponentInChildren<OVRSkeleton>();

        // check hand tracking status one more time before attempting to get strength, just to make sure
        UpdateHandTrackingStatus();

        // if we aren't hand tracking, return 0 strength
        if (!IsHandTracking)
        {
            return 0.0f;
        }

        var handConfidence = ovrHand.HandConfidence;
        var isHandBeingTracked = ovrHand.IsTracked;

        Transform Hand_Root = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform;

        var tipTransforms = new Transform[4];

        tipTransforms[0] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform;  // index tip position
        tipTransforms[1] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform; // middle tip position
        tipTransforms[2] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform;   // ring tip position
        tipTransforms[3] = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_PinkyTip].Transform;  // pinky tip position

        // Calculate the sum of all squared distances between each finger tip and the wrist
        // TODO (Note): Do we need to consider the different sizes of hands when taking in this calculation?
        //              If so, how can we normalize this?
        var sumSquares = 0.0f;
        for (var i = 0; i < tipTransforms.Length; ++i)
        {
            var square = Vector3.Distance(tipTransforms[i].position, Hand_Root.position);
            square *= square;

            sumSquares += square;
        }

        // Remap the sumSquare values into a 0...1 range so that it can be used to trigger actions
        // TODO (Note): Values were observed based off my personal hand, again, do we need to consider different hand sizes?
        float normal = Mathf.InverseLerp(180f, 1100f, sumSquares);
        float bValue = Mathf.Lerp(0, 1, normal);
        bValue = 1 - bValue;

        if (handConfidence == OVRHand.TrackingConfidence.Low || isHandBeingTracked == false)
        {
            _dampenedPinchValue = Mathf.SmoothDamp(_lastValidGripValue, bValue, ref _pinchVelocity, _smoothTime);
        }
        else
        {
            _dampenedPinchValue = Mathf.SmoothDamp(_dampenedPinchValue, bValue, ref _pinchVelocity, _smoothTime);
        }

        _lastValidGripValue = _dampenedPinchValue;
        return _dampenedPinchValue;
    }

    #endregion
}
