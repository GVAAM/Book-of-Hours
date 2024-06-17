using System.Collections;
using System.Collections.Generic;
using System.Data;
using HandPhysicsToolkit.Helpers.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
public class NewFollowObject : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    private GameObject targetObjectRealParent; 

    private BaseInteractor currentInteractor = null;
    private bool grabbing = false;
    private bool wasGrabbing = false;

    private void Start()
    {
        if(targetObject)
        {
            targetObjectRealParent = targetObject.transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject)
        {
            Vector3 newPos = targetObject.transform.position;

            // Define a speed for your interpolation
            float moveSpeed = .5f; // You can adjust this as needed
            float rotationSpeed = .5f; // You can adjust this as needed
            float positionChangeMagnitude = Vector3.Distance(transform.position, newPos);
            float rotationChangeMagnitude = Quaternion.Angle(transform.rotation, targetObject.transform.rotation);


            Vector3 smoothedPos = Vector3.Lerp(transform.position, newPos, moveSpeed);
            Quaternion smoothedRot = Quaternion.Slerp(transform.rotation, targetObject.transform.rotation, rotationSpeed);

            transform.position = smoothedPos;
            transform.rotation = smoothedRot;
        }
    }

    public void RegisterTarget(BaseInteractor interactor)
    {
        targetObject.transform.SetParent(interactor.GetGameObject().transform, true);
        grabbing = true;
        wasGrabbing = false;
        currentInteractor = interactor;
    }

    public void UnregisterTarget()
    {
        grabbing = false;
        wasGrabbing = true;
        currentInteractor = null;

        targetObject.transform.SetParent(targetObjectRealParent.transform);
        targetObject.transform.localPosition = Vector3.zero;
        targetObject.transform.localRotation = Quaternion.identity;
    }
}
