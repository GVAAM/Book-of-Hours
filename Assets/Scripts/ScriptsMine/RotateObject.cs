using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 5f;

    void Update()
    {
        // Check for input to rotate object
        if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateObj(Vector3.right);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateObj(-Vector3.right);
        }
        else
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            HingeJoint hingeJoint = rb.GetComponent<HingeJoint>();
            if (hingeJoint != null)
            {
                JointSpring spring = hingeJoint.spring;
                spring.spring = 5000f;
                hingeJoint.spring = spring;
            }
        }
    }

    void RotateObj(Vector3 axis)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        HingeJoint hingeJoint = rb.GetComponent<HingeJoint>();
        if (hingeJoint != null)
        {
            JointSpring spring = hingeJoint.spring;
            spring.spring = 0f;
            hingeJoint.spring = spring;
        }
        // Calculate rotation amount based on speed and time
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Rotate the object
        transform.Rotate(axis, rotationAmount);
    }
    /* public float rotationSpeed = 5f;
     public GameObject sphere; // Reference to the sphere object
     public bool isGrabbing = false;

     void Update()
     {
         // Check if the sphere object exists and the primary button is pressed
         if (sphere != null && isGrabbing)
         {
             // Get the direction of the sphere's movement
             Vector3 sphereMovementDirection = sphere.GetComponent<Rigidbody>().velocity.normalized;

             // Debug.Log("sphereMovementDirection");
             // Check the direction of movement and rotate the object accordingly
             if (sphereMovementDirection == Vector3.right)
             {
                 // RotateObj(Vector3.right);
                 Debug.Log("Right---");
             }
             else if (sphereMovementDirection == Vector3.left)
             {
                 // RotateObj(Vector3.left);
                 Debug.Log("left---");
             }
             else if (sphereMovementDirection == Vector3.up)
             {
                 Debug.Log("up---");
             }
             else if (sphereMovementDirection == Vector3.down)
             {
                 Debug.Log("down---");
             }
         }


         if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && !isGrabbing)
         {
             isGrabbing = true;
         }
         else if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && isGrabbing)
         {
             isGrabbing = false;
             sphere = null;
         }
     }
     void RotateObj(Vector3 axis)
     {
         // Calculate rotation amount based on speed and time
         float rotationAmount = rotationSpeed * Time.deltaTime;

         // Rotate the object
         transform.Rotate(axis, rotationAmount);
     }

     // Trigger enter event
     void OnTriggerEnter(Collider other)
     {
         if (other.CompareTag("Sphere")) // Assuming the sphere has a tag "Sphere"
         {
             sphere = other.gameObject; // Assign the sphere object reference
         }
     }*/
}
