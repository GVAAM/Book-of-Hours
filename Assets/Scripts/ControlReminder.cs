using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlReminder : MonoBehaviour
{
    public GameObject HandDescription;
    public GameObject ControllerDescription;

    [SerializeField] Material triggerMat;
    [SerializeField] private GameObject page;
    private bool handViewed;
    private bool contViewed;

    // Start is called before the first frame update
    void Start()
    {
        handViewed = false;
        contViewed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.Hands))
        {
            HandDescriptionToggle();
            ControllerDescription.SetActive(false);
        }
        else
        {
            ControllerDescriptionToggle();
            HandDescription.SetActive(false);
        }

    }

    public void HandDescriptionToggle()
    {
        string name = page.GetComponent<MeshRenderer>().material.name;
        // Debug.Log(name);
        // Debug.Log(triggerMat.name + " (Instance)");
        if ((name == triggerMat.name + " (Instance)") && !handViewed)
        {
            HandDescription.SetActive(true);
        }
        else
        {
            handViewed = true;
            HandDescription.SetActive(false);
        }
    }

    public void ControllerDescriptionToggle()
    {
        string name = page.GetComponent<MeshRenderer>().material.name;
        // Debug.Log(name);
        // Debug.Log(triggerMat.name + " (Instance)");
        if ((name == triggerMat.name + " (Instance)") && !contViewed)
        {
            ControllerDescription.SetActive(true);
        }
        else
        {
            contViewed = true;
            ControllerDescription.SetActive(false);
        }
    }
}
