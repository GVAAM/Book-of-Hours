using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class PagesideTextManager : MonoBehaviour
{
    // controls the transcriptions and translations beside the book

    // take in language text files 
    // store them in a useful way (array of strings)
    // display stored texts
    // change what page we're on (including sim page position)
    // change what language we're on

    [SerializeField] public List<TextAsset> LanguageFileList = new List<TextAsset>();
    //0 will be empty
    [SerializeField] private GameObject leftTextField;
    [SerializeField] private GameObject rightTextField;
    [SerializeField] private GameObject bookManager;
    [SerializeField] private GameObject pageMarker;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject leftQuad;
    [SerializeField] private GameObject rightQuad;
    private GameObject folioMenuText;

    private List<string> LanguageTexts;
    private int pageNum; // represents left page num
    private int totalPages = 3;
    private int langNum = 0;
    private string[][] splitArray = null;

    enum pageRegion { LEFT, RIGHT };

    private pageRegion thisFrame, lastFrame;

    [SerializeField] public UnityEvent SimPageTurnLeft;
    [SerializeField] public UnityEvent SimPageTurnRight;

    public int getPageNum() // accessor function
    {
        return pageNum;
    }

    private void Start()
    {
        langNum = controlPanel.GetComponent<ControlPanel>().CurLangNum;
        pageNum = bookManager.GetComponent<BookManager>().getLeftPageNum();

        splitArray = new string[LanguageFileList.Count][];

        //take in language text file of each language
        //make an array of individual pages
        for (int i = 1; i < LanguageFileList.Count; i++)
        {
            splitArray[i] = null;
            string fullText = LanguageFileList[i].ToString();
            string[] separator = new string[] { "/next/" };
            splitArray[i] = fullText.Split(separator, 9000, StringSplitOptions.None);
        }
        // the length of all the pages should be the same regardless of language,
        // so take the first non-OFF language
        totalPages = splitArray[1].Length;

        displayTexts();

        thisFrame = pageRegion.RIGHT;
        lastFrame = pageRegion.RIGHT;

        folioMenuText = GameObject.Find("Advance Page Name Text");
    }

    // change the language displayed 
    public void IncrementLangNum()
    {
        langNum++;
        if (langNum >= LanguageFileList.Count)
        {
            langNum = 0;
        }
        displayTexts();
    }

    public void DecrementLangNum()
    {
        langNum--;
        if (langNum < 0)
        {
            langNum = LanguageFileList.Count;
        }
        displayTexts();
    }

    // chagne the page displayed
    public void IncrementPageNum()
    {
        // since left and right pages in same file, we will have to increment by 2
        pageNum += 2;

        if (pageNum >= totalPages)
        {
            pageNum = 0;
        }
        displayTexts();
    }

    public void DecrementPageNum()
    {
        // since left and right pages in same file, we will have to increment by 2
        pageNum -= 2;

        if (pageNum < 0)
        {
            pageNum += totalPages;
        }
        displayTexts();
    }

    // update the text if language or page is changed
    void displayTexts()
    {
        if (langNum > 0) // not off
        {
            leftTextField.GetComponent<TextMeshPro>().text = splitArray[langNum][pageNum];
            rightTextField.GetComponent<TextMeshPro>().text = splitArray[langNum][pageNum + 1];
            leftQuad.SetActive(true);
            rightQuad.SetActive(true);
        }
        else
        {
            leftTextField.GetComponent<TextMeshPro>().text = " ";
            rightTextField.GetComponent<TextMeshPro>().text = " ";
            if (leftQuad != null)
            {
                leftQuad.SetActive(false);
            }

            if (rightQuad != null)
            {
                rightQuad.SetActive(false);
            }
           
        }
    }

    // updates the inspector pages if the pages of the book get flipped
    private void Update()
    {
        //two enum variables, one for this frame, other for last frame. The type of mismatch determines when the increment and decrement functions are called
        if (pageMarker.transform.position.x < bookManager.transform.position.x)
        {
            thisFrame = pageRegion.LEFT;
        }
        else
        {
            thisFrame = pageRegion.RIGHT;
        }

        if (lastFrame == pageRegion.LEFT && thisFrame == pageRegion.RIGHT)
        {
            DecrementPageNum();
            // increment the page inspector also if it's paired
            if (bookManager.GetComponent<BookManager>().pairedMode)
            {
                Debug.Log("it's getting turned right");
                SimPageTurnRight.Invoke();
            }

        }
        if (lastFrame == pageRegion.RIGHT && thisFrame == pageRegion.LEFT)
        {
            IncrementPageNum();
            // decrement the page inspector also if it's paired
            if (bookManager.GetComponent<BookManager>().pairedMode)
            {
                Debug.Log("it's getting turned left");
                SimPageTurnLeft.Invoke();
            }

        }

        // update the folio text on the control panel in response to page flip
        if (folioMenuText != null)
            folioMenuText.GetComponent<MenuText>().updateState();
        // folioMenuText.GetComponent<MenuText>().updateState();

        lastFrame = thisFrame;
    }

    public void ResetExperiencePTM()
    {
        // safely move the pagemarker to the other side without triggering an increment
        pageMarker.transform.SetPositionAndRotation(new Vector3(bookManager.transform.position.x + 5, pageMarker.transform.position.y, pageMarker.transform.position.z), Quaternion.identity);
        lastFrame = pageRegion.RIGHT;

        // reset the language and page numbers
        langNum = controlPanel.GetComponent<ControlPanel>().CurLangNum;
        pageNum = bookManager.GetComponent<BookManager>().leftPageNum;
        displayTexts();
    }
}