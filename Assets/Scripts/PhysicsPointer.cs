using System;
using System.Collections;
using System.Collections.Generic;
using HandPhysicsToolkit.Helpers.Interfaces;
using Oculus.Interaction.Body.Input;
using UnityEngine;

public class PhysicsPointer : MonoBehaviour, BaseInteractor
{
    // on the right hand? why not the left
    public float defaultLength = 3.0f;

    [SerializeField] private GameObject startPointObj;
    [SerializeField] private Interactor sphereInteractor; 

    public bool isLeftHand = true;

    //Private Variables
    private int layerMask = 1 << 3; // only allow layer 3 (LaserUI) for raycasts
    
    // used for caching the startPointObj's position vector so that it can be restored if handtracking is enabled then disabled
    private Vector3 nonHandtrackingStartPoint = Vector3.zero;
    private Vector3 foundEndPoint = Vector3.zero;
    private LineRenderer lineRenderer = null;
    private Interactible foundObject = null;
    private Interactible hitObject = null;
    private bool wasClickPressedLastFrame = false;

    private bool isLROn = false;
    private bool isHandtrackingOn = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        nonHandtrackingStartPoint = startPointObj.transform.localPosition;
    }

    private void Start()
    {
        if (sphereInteractor != null)
        {
            sphereInteractor.onHandTrackingActive.AddListener(OnHandTrackingOn);
            sphereInteractor.onHandTrackingInactive.AddListener(OnHandTrackingOff);
        }
    }

    private void Update()
    {
        // update position of startPointObj if handtracking is enabled
        if (isHandtrackingOn && sphereInteractor)
        {
            var indexDistal = sphereInteractor.GetBonePosition(OVRSkeleton.BoneId.Hand_Index3); // get position of attached hand's index distal bone
            var thumbDistal = sphereInteractor.GetBonePosition(OVRSkeleton.BoneId.Hand_Thumb3); // get position of attached hand's thumb distal bone

            var pointerPose = sphereInteractor.GetPointerPose();
            if (pointerPose)
            {
                startPointObj.transform.rotation = pointerPose.rotation;

                var midOfIndexAndThumb = (indexDistal + thumbDistal) / 2.0f;
            
                startPointObj.transform.position = midOfIndexAndThumb;
            }
        }

        //If the raycast is successful and the interactor is not intersecting anything then we can apply the raycast
        if(TryInteractibleRaycast() && !sphereInteractor.IsIntersectingObject())
        {
            AddInteractible(foundObject);
            UpdateLength();
            lineRenderer.enabled = true;

            CheckPointerClick();
        }
        else
        {
            RemoveInteractible();
            lineRenderer.enabled = false;
        }
    }

    public GameObject GetGameObject() { return gameObject; }
    public bool GetIsLeftHand() { return isLeftHand; }
    public bool GetIsHandTracking() { return isHandtrackingOn; }

    private bool TryInteractibleRaycast()
    {
        RaycastHit hit = CreateForwardRaycast();
        foundEndPoint = DefaultEnd(defaultLength);

        if (hit.collider)
        {
            foundObject = hit.collider.gameObject.GetComponent<Interactible>();
            foundEndPoint = hit.point;

            return true;
        }
        else
            foundObject = null;

        return false;
    }


    private void CheckPointerClick()
    {
        var triggerType = isLeftHand ? OVRInput.Axis1D.PrimaryHandTrigger : OVRInput.Axis1D.SecondaryHandTrigger;
        var controllerHandedness = isLeftHand ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        var isTriggered = false;

        if (isHandtrackingOn)
        {
            isTriggered = sphereInteractor.GetIndexPinch() > 0.8f;
        }
        else
        {
            isTriggered = OVRInput.Get(triggerType, OVRInput.Controller.Touch) > 0.7f ||
                          OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controllerHandedness) > 0.7f;
        }
        
        if (isTriggered)
        {
            if (wasClickPressedLastFrame == false)
            {
                if (hitObject)
                {
                    hitObject.OnPointerClick.Invoke(this);
                }
            }

            wasClickPressedLastFrame = true;
        }
        else
        {
            if (hitObject)
            {
                hitObject.OnPointerUnclick.Invoke(this);
            }
            wasClickPressedLastFrame = false;
        }
    }

    private void AddInteractible(Interactible interactible)
    {
        if (!interactible)
        {
            RemoveInteractible();
        }
        else
        {
            // if there was previously an object that the pointer was over, remove it
            if (hitObject != null)
            {
                RemoveInteractible();
            }
            hitObject = interactible;
            interactible.OnPointerOver.Invoke(this);
        }
    }

    private void RemoveInteractible()
    {
        if (!hitObject) return;
        
        hitObject.OnPointerExit.Invoke(this);
        hitObject = null;
    }

    void UpdateLength()
    {
        lineRenderer.SetPosition(0, startPointObj.transform.position);
        lineRenderer.SetPosition(1, foundEndPoint);
    }

    RaycastHit CreateForwardRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(startPointObj.transform.position, startPointObj.transform.forward);

        Physics.Raycast(ray, out hit, defaultLength, layerMask);
        
        return hit;
    }

    Vector3 DefaultEnd(float length)
    {
        return startPointObj.transform.position + (startPointObj.transform.forward * length);
    }

    //CHECK this when implementing handtracking laser - it only turns off the line renderer, not the actual raycast
    void OnHandTrackingOn()
    {
        isHandtrackingOn = true;
    }
    void OnHandTrackingOff()
    {
        isHandtrackingOn = false;
        
        // reset position/rotation of startPointObj
        startPointObj.transform.localRotation = Quaternion.identity;
        startPointObj.transform.localPosition = nonHandtrackingStartPoint;
    }
    
}
