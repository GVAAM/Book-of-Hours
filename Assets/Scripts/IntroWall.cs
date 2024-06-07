using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class IntroWall : MonoBehaviour
{
    //controls the wall of introductory text and images
    
    [SerializeField] private GameObject textWall;
    [SerializeField] private GameObject introWallParent;
    [SerializeField] private GameObject spriteWall;

    [SerializeField, TextArea] List<string> textList = new List<string>();
    [SerializeField] List<Sprite> spriteList = new List<Sprite>();

    private int currentIndex = 0;
    
    void Start()
    {
        UpdateTextWall();
    }
    
    void Update()
    {
        if ( OVRInput.GetDown(OVRInput.Button.One))
        {
            if (currentIndex < textList.Count - 1)
            {
                currentIndex++;
                UpdateTextWall();
            }
            else
            {
                introWallParent.SetActive(false);
                LoadNextScene();
            }
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two)) // turn off intro wall when press B
        {
            introWallParent.SetActive(false);
            LoadNextScene();
        }
    }

    void UpdateTextWall()
    {
        textWall.GetComponent<TextMeshPro>().text = textList[currentIndex].Replace("@n", Environment.NewLine);
        spriteWall.GetComponent<SpriteRenderer>().sprite = spriteList[currentIndex];
        StartCoroutine(FadeInText(3f, textWall.GetComponent<TextMeshPro>()));
        if (currentIndex == 0 || spriteWall.GetComponent<SpriteRenderer>().sprite != spriteList[currentIndex-1] )
        {
            StartCoroutine(FadeInSprite(3f, spriteWall.GetComponent<SpriteRenderer>()));
        }
        
    }

    public void Restart()
    {
        currentIndex = 0;
        introWallParent.SetActive(true);
        UpdateTextWall();
    }


    public IEnumerator FadeInText(float t, TextMeshPro i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeInSprite(float t, SpriteRenderer i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("TemplateScene");
    }
}
