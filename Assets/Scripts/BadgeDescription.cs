using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeDescription : MonoBehaviour
{

    public GameObject description;

    // Start is called before the first frame update
    void Start()
    {
        description.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DescOn()
    {
        description.SetActive(true);
    }

    public void DescOff()
    {
        description.SetActive(false);
    }
}
