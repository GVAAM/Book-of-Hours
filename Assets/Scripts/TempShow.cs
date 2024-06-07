using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShow : MonoBehaviour
{

    [SerializeField] GameObject text;
    private bool status;

    // Start is called before the first frame update
    public void Start()
    {
        text.SetActive(false);
        status = false;
    }

    // Update is called once per frame
    public void show()
    {
        if (status)
        {
            text.SetActive(false);
            status = false;
        }
        else
        {
            text.SetActive(true);
            status = true;
        }
    }

    public void hide()
    {
        text.SetActive(false);
    }
}
