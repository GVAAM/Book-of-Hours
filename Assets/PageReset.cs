using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageReset : MonoBehaviour
{
    [SerializeField]
    List<GameObject> pages = new List<GameObject>();

    List<List<Vector3>> pagePositions = new List<List<Vector3>>();
    List<List<Quaternion>> pageRotations = new List<List<Quaternion>>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < pages.Count; i++)
        {
            Transform bone = pages[i].GetComponent<SkinnedMeshRenderer>().rootBone;
            pagePositions.Add(new List<Vector3>());
            pageRotations.Add(new List<Quaternion>());

            while(bone != null)
            {
                pagePositions[i].Add(bone.position);
                pageRotations[i].Add(bone.rotation);

                bone = bone.GetChild(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPages()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            Transform bone = pages[i].GetComponent<SkinnedMeshRenderer>().rootBone;
            int j = 0;
            while (bone != null)
            {
                bone.position = pagePositions[i][j];
                bone.rotation = pageRotations[i][j];
                j++;

                bone = bone.GetChild(0);
            }
        }
    }
}
