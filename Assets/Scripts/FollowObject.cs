using System.Collections;
using System.Collections.Generic;
using System.Data;
using HandPhysicsToolkit.Helpers.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowObject : MonoBehaviour
{
    private Transform targetObject;
    private bool _isHandTracking = false;
    private bool _isLeftHand = false;
    private BaseInteractor currentInteractor = null;
    private bool grabbing = false;
    private bool wasGrabbing = false;

    [Header("Controller Offset")]
    [FormerlySerializedAs("pinchPosOffset")] [SerializeField] private Vector3 controllerPosOffset;
    [FormerlySerializedAs("pinchRotOffset")] [SerializeField] private Vector3 controllerRotOffset;
    
    
    [Header("Handtracking Offset")]
    [FormerlySerializedAs("gripPosOffset")] [SerializeField] private Vector3 handtrackingPosOffset;
    [FormerlySerializedAs("gripRotOffset")] [SerializeField] private Vector3 handtrackingRotOffset;
    

    // Update is called once per frame
    void Update()
    {
        if (targetObject)
        {
            Vector3 newPos = targetObject.transform.position;
            _isHandTracking = currentInteractor.GetIsHandTracking();


            var posOffset = (!_isHandTracking) ? controllerPosOffset : handtrackingPosOffset;
            var rotOffset = (!_isHandTracking) ? controllerRotOffset : handtrackingRotOffset;

            // Define a speed for your interpolation
            float moveSpeed = .5f; // You can adjust this as needed
            float rotationSpeed = .5f; // You can adjust this as needed
            float positionChangeMagnitude = Vector3.Distance(transform.position, newPos);
            float rotationChangeMagnitude = Quaternion.Angle(transform.rotation, targetObject.rotation);


            Vector3 smoothedPos = Vector3.Lerp(transform.position, newPos, moveSpeed);
            Quaternion smoothedRot = Quaternion.Slerp(transform.rotation, targetObject.rotation * Quaternion.Euler(0, 180f, 0) * Quaternion.Euler(rotOffset), rotationSpeed);

            transform.position = smoothedPos + posOffset;
            transform.rotation = smoothedRot; //  * Quaternion.Euler(rotOffset);

            
            // transform.Rotate(rotOffset);

            
            if (positionChangeMagnitude >= 3f)
            {
                // Lerp the position
                //transform.position = Vector3.Lerp(transform.position, newPos, moveSpeed);
                // Apply the position offset
                //transform.position += posOffset;
                transform.position = smoothedPos + posOffset;
            }


            if (rotationChangeMagnitude >= 50f)
            {
                transform.rotation = smoothedRot;
                // Slerp the rotation
                /*
                transform.rotation = Quaternion.Slerp(transform.rotation, targetObject.rotation, rotationSpeed);

                if (!_isLeftHand && _isHandTracking)
                {
                    rotOffset.x -= 180f;
                    rotOffset.z -= 180f;
                }
                // Rotate by the offset
                transform.Rotate(rotOffset);
                */
            }
            

        }
    }

    public void attach()
    {
        transform.SetParent(targetObject);
    }

    public void dettach()
    {
        transform.SetParent(null);
    }

    public void RegisterTarget(BaseInteractor interactor)
    {
        if (targetObject != null) return;

        targetObject = interactor.GetGameObject().transform;
        grabbing = true;
        wasGrabbing = false;
        // Attach the magnifying glass to the hand and adjust position
        //transform.SetParent(interactor.GetGameObject().transform);
        currentInteractor = interactor;

        _isLeftHand = interactor.GetIsLeftHand();
        //attach();
    }
    
    public void UnregisterTarget()
    {
        if (targetObject == null) return;

        grabbing = false;
        wasGrabbing = true;
        // Release the magnifying glass to the hand and adjust position
        //transform.SetParent(null);
        targetObject = null;
        currentInteractor = null;
        //dettach();
    }
}
