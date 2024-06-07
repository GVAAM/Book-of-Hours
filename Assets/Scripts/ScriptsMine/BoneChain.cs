using Oculus.Interaction.Input;
using UnityEngine;

public class BoneChain : MonoBehaviour
{
    public Rigidbody[] bones; // Array to hold the bones
    public Rigidbody triggerHand; // The trigger hand
    public Collider triggerCollider; // Collider attached to the trigger hand
    public Collider leftTrigger, rightTrigger;

    private SpringJoint[] springJoints; // Array to hold the spring joints
    public bool isGrabbing=false;
    public SpringJoint handJoint;
    private void Start()
    {
        // Initialize the spring joints
        springJoints = new SpringJoint[bones.Length ];
        for (int i = 0; i < springJoints.Length; i++)
        {
            springJoints[i] = bones[i].gameObject.AddComponent<SpringJoint>();
            springJoints[i].connectedBody = bones[i + 1];
            springJoints[i].spring = 1000f;
            // Adjust spring settings as needed
            /*springJoints[i].spring = 100f; // Adjust stiffness of the spring
            springJoints[i].damper = 5f; // Adjust damping of the spring*/
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger hand entered the collider
        if (other == triggerCollider && isGrabbing)
        {
            // Disable spring joints
            foreach (var joint in springJoints)
            {
                joint.enableCollision = false;
            }
            //if(handJoint.GetComponent<SpringJoint>() != null) return;
            // Connect trigger hand to the first bone
            if (triggerHand.gameObject.GetComponent<SpringJoint>() == null)
                handJoint = triggerHand.gameObject.AddComponent<SpringJoint>();
            else
            {
                handJoint = triggerHand.gameObject.GetComponent<SpringJoint>();
            }
          handJoint.connectedBody = bones[0];
            // Adjust spring settings as needed
            handJoint.spring = 100f; // Adjust stiffness of the spring
            handJoint.damper = 5f; // Adjust damping of the spring
        }
       
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && !isGrabbing)
        {
            isGrabbing = true;
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && isGrabbing) isGrabbing = false;

        if (!isGrabbing)
        {
            // Disable spring joints
            foreach (var joint in springJoints)
            {
                joint.enableCollision = false;
            }
            // Connect trigger hand to the first bone
            //  SpringJoint handJoint = triggerHand.gameObject.AddComponent<SpringJoint>();
            
            if (handJoint != null) handJoint.connectedBody = null;
            // Adjust spring settings as needed
         //   if (triggerHand.gameObject.GetComponent<SpringJoint>() != null) triggerHand.gameObject.GetComponent<SpringJoint>().connectedBody = null;
        }
    }   
}
