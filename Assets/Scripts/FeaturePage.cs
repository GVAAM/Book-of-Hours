using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeaturePage : MonoBehaviour
{
    [SerializeField] private Material page;
    [SerializeField] private bool onLeft;


    public Material getPage()
    {
        return page;
    }

    public bool isOnLeft()
    {
        return onLeft;
    }

}
