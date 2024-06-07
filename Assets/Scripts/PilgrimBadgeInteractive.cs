using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PilgrimBadgeInteractive : MonoBehaviour
{
    public GameObject line;
    public GameObject description;
    public GameObject highlightSphere;

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
        // Debug.Log(name);
        // Debug.Log(triggerMat.name + " (Instance)");
        if ((name == triggerMat.name + " (Instance)"))
        {
            line.SetActive(true);
            description.SetActive(true);
            highlightSphere.SetActive(true);
        }
        else
        {
            line.SetActive(false);
            description.SetActive(false);
            highlightSphere.SetActive(false);
        }
    }
}
