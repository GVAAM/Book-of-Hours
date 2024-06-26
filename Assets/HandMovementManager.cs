using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandMovementManager : MonoBehaviour
{

    Vector3 movementDirection = Vector3.zero;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject CenterEyeAnchor;
    [SerializeField] private GameObject directonPanel;

    [SerializeField] private TextMeshPro OnOffText;
    bool isOn = false;

    private Rigidbody pr;
    private Quaternion lookDirection = Quaternion.identity;
    private Vector3 normalizedLookDirection = Vector3.forward;
    public float playerSpeed = 1;

    private void Start()
    {
        OnOffText.text = "O";
        directonPanel.SetActive(false);

        pr = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //secret speed button for debugging
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            playerSpeed *= 3;
        }
        else if (OVRInput.GetUp(OVRInput.Button.Two))
        {
            playerSpeed /= 3;
        }

        //controls movement relative to the direction the player is facing.
        var centerEyeTransform = CenterEyeAnchor.transform;
        lookDirection = centerEyeTransform.transform.rotation;

        lookDirection.eulerAngles = new Vector3(0, lookDirection.eulerAngles.y, 0);
        normalizedLookDirection = lookDirection * Vector3.forward;
        var lookDirectionRight = Vector3.Cross(Vector3.up, normalizedLookDirection);

        var joystickAxisR = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
        var joystickAxisU = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);

        var fwdMove = movementDirection.z * Time.deltaTime * playerSpeed * (normalizedLookDirection);
        var strafeMove = movementDirection.x * Time.deltaTime * playerSpeed * (lookDirectionRight);
        pr.position += fwdMove + strafeMove;

        pr.position += movementDirection.y * Time.deltaTime * playerSpeed * transform.up;

        float rotDir = -OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger, OVRInput.Controller.LTouch);
        rotDir += OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger, OVRInput.Controller.RTouch);

        pr.transform.RotateAround(pr.transform.position, Vector3.up, joystickAxisU.x * Time.deltaTime * playerSpeed);
    }

    public void UpdatePanel()
    {
        if(isOn)
        {
            OnOffText.text = "O";
            directonPanel.SetActive(false);
        }
        else
        {
            OnOffText.text = "I";
            directonPanel.SetActive(true);
        }
        isOn = !isOn;
    }

    public void SetUpMovement() { movementDirection.y = 1.0f; }
    public void SetDownMovement() { movementDirection.y = -1.0f; }
    public void SetLeftMovement() { movementDirection.x = -1.0f; }
    public void SetRightMovement() { movementDirection.x = 1.0f; }
    public void SetForwardMovement() { movementDirection.z = 1.0f; }
    public void SetBackwardsMovement() { movementDirection.z = -1.0f; }

    public void ClearMovement() {  movementDirection = Vector3.zero; }

}