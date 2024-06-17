using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectResetController : MonoBehaviour
{
    public static List<GameObject> resettableObjects = new List<GameObject>();

    public static void AddResettableObj(GameObject obj)
    {
        resettableObjects.Add(obj);
    }

    public void ResetObjects()
    {
        foreach(var obj in resettableObjects)
        {
            ResetObject reset = obj.GetComponent<ResetObject>();

            if(reset)
            {
                reset.ResetPositionAndRotation();
            }
        }
    }
}
