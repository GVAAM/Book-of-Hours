using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeEventSystem : MonoBehaviour
{
    enum BadgeType
    {
        Wrong1,
        Wrong2,
        Right,
        None
    }


    public GameObject WrongD1;
    public GameObject WrongD2;
    public GameObject RightD;
    public GameObject BIntro;

    SortedSet<BadgeType> activeBadges = new SortedSet<BadgeType>();

    [SerializeField] Material triggerMat;
    [SerializeField] private GameObject page;

    void OnTriggerEnter(Collider col)
    {
        BadgeType nextType = BadgeType.None;

        if (col.gameObject.CompareTag("Badge 3 R"))
            nextType = BadgeType.Right;
        else if (col.gameObject.CompareTag("Badge 1 W"))
            nextType = BadgeType.Wrong1;
        else if (col.gameObject.CompareTag("Badge 2 W"))
            nextType = BadgeType.Wrong2;

        BIntro.SetActive(false);

        switch (nextType)
        {
            case BadgeType.Right:
                RightD.SetActive(true);
                break;
            case BadgeType.Wrong1:
                WrongD1.SetActive(true);
                break;
            case BadgeType.Wrong2:
                WrongD2.SetActive(true);
                break;
            default:
                break;
        }

        foreach(var type in activeBadges)
        {
            switch (type)
            {
                case BadgeType.Right:
                    RightD.SetActive(false);
                    break;
                case BadgeType.Wrong1:
                    WrongD1.SetActive(false);
                    break;
                case BadgeType.Wrong2:
                    WrongD2.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        activeBadges.Add(nextType);
    }

    void OnTriggerExit(Collider col)
    {
        BadgeType nextType = BadgeType.None;

        if (col.gameObject.CompareTag("Badge 3 R"))
            nextType = BadgeType.Right;
        else if (col.gameObject.CompareTag("Badge 1 W"))
            nextType = BadgeType.Wrong1;
        else if (col.gameObject.CompareTag("Badge 2 W"))
            nextType = BadgeType.Wrong2;

        switch (nextType)
        {
            case BadgeType.Right:
                RightD.SetActive(false);
                break;
            case BadgeType.Wrong1:
                WrongD1.SetActive(false);
                break;
            case BadgeType.Wrong2:
                WrongD2.SetActive(false);
                break;
            default:
                break;
        }

        activeBadges.Remove(nextType);

        if(activeBadges.Count == 0)
            BIntro.SetActive(true);
        else
        {
            if(activeBadges.Contains(BadgeType.Right))
                RightD.SetActive(true);
            else if(activeBadges.Contains(BadgeType.Wrong1))
                WrongD1.SetActive(true);
            else if(activeBadges.Contains(BadgeType.Wrong2))
                WrongD2.SetActive(true);
        }
    }
}