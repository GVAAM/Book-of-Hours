using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeEventSystem : MonoBehaviour
{

    public GameObject WrongD1;
    public GameObject WrongD2;
    public GameObject RightD;
    public GameObject BIntro;

    [SerializeField] Material triggerMat;
    [SerializeField] private GameObject page;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        string name = page.GetComponent<MeshRenderer>().material.name;
        if ((name == triggerMat.name + " (Instance)"))
        {

            if (col.gameObject.CompareTag("Badge 3 R"))
            {
                RightD.SetActive(true);
                BIntro.SetActive(false);
            }

            if (col.gameObject.CompareTag("Badge 1 W"))
            {
                WrongD1.SetActive(true);
                BIntro.SetActive(false);
            }

            if (col.gameObject.CompareTag("Badge 2 W"))
            {
                WrongD2.SetActive(true);
                BIntro.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Badge 3 R"))
        {
            RightD.SetActive(false);
        }

        if (col.gameObject.CompareTag("Badge 1 W"))
        {
            WrongD1.SetActive(false);
        }

        if (col.gameObject.CompareTag("Badge 2 W"))
        {
            WrongD2.SetActive(false);
        }
    }
}