using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class RightPageTrigger : MonoBehaviour
{
    public GameObject rightPage;
    private GameObject parentObj;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Find the parent object by tag
        parentObj = GameObject.FindWithTag("ParentObj");

        if (parentObj == null)
        {
            Debug.LogError("Parent object not found with tag 'ParentObj'");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("AAA---" + collision);
        if (collision.gameObject.CompareTag("Page"))
        {
            // Trigger the button action on the MaterialCycler component of the parent object
            parentObj.GetComponent<MaterialCycler>().OnButtonN();

            // Instantiate the 'rightPage' GameObject as a child of the 'parentObj'
           // Instantiate(rightPage, parentObj.transform);

            // Destroy the grandparent object of the current object
            Destroy(transform.parent.parent.parent.parent.gameObject);
        }
    }
}
