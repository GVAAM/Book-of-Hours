using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OverheadLightingToggle : MonoBehaviour
{
    [SerializeField]
    GameObject light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleLight()
    {
        if (light != null)
        {
            if(light.activeSelf)
                light.SetActive(false);
            else 
                light.SetActive(true);
        }
    }
}
