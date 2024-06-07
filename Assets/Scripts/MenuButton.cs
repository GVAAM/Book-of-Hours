using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    public GameObject menu;
    public MenuMovement eyeTracking;

    private bool hidden;

    public OVRInput.Controller controller = OVRInput.Controller.All;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartUp());
        hidden = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hidden == true && OVRInput.Get(OVRInput.Button.Start, controller))
        {
            //menu.SetActive(true);
            eyeTracking.TrackEye();
            //hidden = false;
            StartCoroutine(OnDelay());
        }

        if (hidden == false && OVRInput.Get(OVRInput.Button.Start, controller))
        {
            //menu.SetActive(false);

            //hidden = true;

            StartCoroutine(OffDelay());
        }
    }

    IEnumerator OffDelay()
    {
        menu.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        hidden = true;
    }

    IEnumerator OnDelay()
    {
        menu.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        hidden = false;
    }

    IEnumerator StartUp()
    {
        yield return new WaitForSeconds(0.1f);

        menu.SetActive(false);
    }
}
