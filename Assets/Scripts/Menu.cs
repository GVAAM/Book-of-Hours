using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public bool startHidden = true;
    public List<GameObject> otherMenus = new List<GameObject>();

    private bool hidden;

    // Start is called before the first frame update
    void Start()
    {
        hidden = startHidden;
        menu.SetActive(!hidden);
        if(!hidden)
            foreach (var m in otherMenus)
                m.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onAndOff()
    {
        if (hidden)
            StartCoroutine(OnDelay());
        else
            StartCoroutine(OffDelay());
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

        foreach(var m in otherMenus)
            m.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        hidden = false;
    }
}
