using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeDescription : MonoBehaviour
{
    public GameObject description;

    [SerializeField] Material triggerMat;
    [SerializeField] private GameObject page;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        string name = page.GetComponent<MeshRenderer>().material.name;
        Debug.Log(name);
        Debug.Log(triggerMat.name + " (Instance)");
        if ((name == triggerMat.name + " (Instance)"))
        {
            description.SetActive(true);
        }
        else
        {
            description.SetActive(false);
        }
    }
}
