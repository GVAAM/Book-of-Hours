using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.LookDev;

public class StaticDebugText : MonoBehaviour
{
    static TextMeshPro mText;
    static List<string> mLines = new List<string>();
    static List<string> mPermanentLines = new List<string>();

    private void Awake()
    {
        mText = GetComponent<TextMeshPro>();
    }

    private void LateUpdate()
    {
        string text = "";

        foreach(string line in mPermanentLines)
        {
            text += line;
            text += "\n";
        }

        text += "\n";

        foreach (string line in mLines)
        {
            text += line;
            text += "\n";
        }

        mText.text = text;

        mLines.Clear();
    }

    public static void AddDebugMessageForFrame(string line)
    {
        mLines.Add(line);
    }

    public static void AddPermanentDebugMessage(string line)
    {
        mPermanentLines.Add(line);
    }
}
