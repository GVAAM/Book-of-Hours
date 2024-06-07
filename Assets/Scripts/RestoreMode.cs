using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Restore mode feature
 */
public class RestoreMode : MonoBehaviour
{

    // in-scene dependents: Collider L, Collider R, Restore Button 
    private bool _isActive = false;
    private bool _lastState = false;
    private Dictionary<Material, KeyValuePair<GameObject, bool>> _restoredFeatures; // name of material to list of gameobjects
    [SerializeField] private GraspingPoint _graspingPoint;
    [SerializeField] private BookManager _bookManager;
    private List<Material> pageList;
    [SerializeField] private List<GameObject> _features;
    private int lastRight;
    private int lastLeft;
    private bool isGripping;

    [SerializeField] GameObject UI;
 

    public void updateHand()
    {
        isGripping = !isGripping;
    }

    public bool getHandStatus()
    {
        return isGripping;
    }

    private void Start()
    {

        _isActive = false;
        _lastState = false;

        // identify all the restore features and assign them to the correct page
        pageList = _bookManager.pageList;

        _restoredFeatures = new Dictionary<Material, KeyValuePair<GameObject, bool>>();


        foreach (GameObject page in _features)
        {
            page.SetActive(false);
            // TO-DO: should be (feature, onleft) as the key
            _restoredFeatures[page.GetComponent<FeaturePage>().getPage()] = new KeyValuePair<GameObject, bool>(page, page.GetComponent<FeaturePage>().isOnLeft());
        }

        lastRight = -1;
        lastLeft = -1;
        isGripping = false;
        UI.SetActive(false);
    }

    public void Update()
    {
        if (_isActive && _isActive != _lastState) // initial switch to on
        {
            UI.SetActive(true);
            int currLeft = _graspingPoint.GetSimPageSide() == GraspingPoint.SimPageSide.RightSide ? _bookManager.getLeftPageNum() : _bookManager.getSimPageNum(); // see which side the sim page is on 
            int currRight = currLeft == _bookManager.getLeftPageNum() ? _bookManager.getSimPageNum() : _bookManager.getRightPageNum();

            Material leftPage = pageList[currLeft];
            Material rightPage = pageList[currRight];

            if (_restoredFeatures.ContainsKey(leftPage) && _restoredFeatures[leftPage].Value)
            {
                _restoredFeatures[leftPage].Key.SetActive(true);

            }

            if (_restoredFeatures.ContainsKey(rightPage) && !_restoredFeatures[rightPage].Value)
            {
                _restoredFeatures[rightPage].Key.SetActive(true);

            }

            lastLeft = currLeft;
            lastRight = currRight;
            _lastState = _isActive; // update lastState to on
        }
        else if (_isActive) // subsequent updates when page flips
        {
            int currLeft = _graspingPoint.GetSimPageSide() == GraspingPoint.SimPageSide.RightSide ? _bookManager.getLeftPageNum() : _bookManager.getSimPageNum(); // see which side the sim page is on 
            int currRight = currLeft == _bookManager.getLeftPageNum() ? _bookManager.getSimPageNum() : _bookManager.getRightPageNum();
            if (currLeft != lastLeft && currRight != lastRight) // works for increment and decrement -- need to make it work for if the page is just open
                // what if this was removed? 
            {
                if (lastLeft > -1)
                {
                    Material lastLeftPage = pageList[lastLeft];
                    if (_restoredFeatures.ContainsKey(lastLeftPage))
                    {
                        _restoredFeatures[lastLeftPage].Key.SetActive(false);
                    }
                }
                if (lastRight > -1)
                {
                    var lastRightPage = pageList[lastRight];
                    if (_restoredFeatures.ContainsKey(lastRightPage))
                    {
                        _restoredFeatures[lastRightPage].Key.SetActive(false);
                    }
                }

                Material leftPage = pageList[currLeft];
                Material rightPage = pageList[currRight];

                if (_restoredFeatures.ContainsKey(leftPage) && _restoredFeatures[leftPage].Value)
                {
                    _restoredFeatures[leftPage].Key.SetActive(true);

                }
                if (_restoredFeatures.ContainsKey(rightPage) && !_restoredFeatures[rightPage].Value)
                {
                    _restoredFeatures[rightPage].Key.SetActive(true);

                }
            }


            lastLeft = currLeft;
            lastRight = currRight;

        }
        else if (_isActive != _lastState) // not on 
        {
            UI.SetActive(false);
            foreach (Material page in _restoredFeatures.Keys)
            {
                _restoredFeatures[page].Key.SetActive(false);
            }
            _lastState = _isActive;
        }
    }

    public bool restoreModeOn()
    {
        return _isActive;
    }

    public void ToggleRestore()
    {
        _isActive = !_isActive;
        Update();
    }

    public void ResetExperienceRestore()
    {
        _isActive = false;
        Update();
    }

}
