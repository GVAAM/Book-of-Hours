using System.Collections;
using System.Collections.Generic;
using HandPhysicsToolkit.Helpers.Interfaces;
// using Obi;
//REMOVED OBI 12/10/2023 
using UnityEngine;



public class BookManager : MonoBehaviour
{

    /*//leftpagenum is used by pagesideTextManager for the default page
    public int leftPageNum = 0; // change this to private later
    int simPageNum = 1; // simpage is initially right page 
    public int rightPageNum = 2; // was 2
    int versoInspectorPageNum = 0;
    int rectoInspectorPageNum = 1;
    public bool pairedMode = true;
    private bool isInspectorOn = true;

    [SerializeField] GameObject rightPage;
    [SerializeField] GameObject leftPage;
    [SerializeField] GameObject simPage;
    [SerializeField] GameObject versoInspectorPage;
    [SerializeField] GameObject rectoInspectorPage;
    [SerializeField] private GameObject bookSystemObject;
    [SerializeField] Cloth bookPage;
    //[SerializeField] private RestoreMode restoreMode;

    [SerializeField] public List<Material> pageList = new List<Material>(); // stores the page materials
    //[SerializeField] List<Material> restoredList = new List<Material>(); // stores the restored pages
    private List<Material> currList; // used to refernce pageList or restoredList


    public int getLeftPageNum()
    {
        return leftPageNum;
    }

    public int getRightPageNum()
    {
        return rightPageNum;
    }

    public int getSimPageNum()
    {
        return simPageNum;
    }

    // Sets defaults 
    private void Start()
    {
        currList = pageList;
        rightPage.GetComponent<Renderer>().material = currList[rightPageNum];
        leftPage.GetComponent<Renderer>().material = currList[leftPageNum];
        simPage.GetComponent<Renderer>().material = currList[simPageNum];
        versoInspectorPage.GetComponent<Renderer>().material = currList[versoInspectorPageNum];
        rectoInspectorPage.GetComponent<Renderer>().material = currList[rectoInspectorPageNum];
        //folioMenuText = GameObject.Find("Advance Page Name Text");
    }

    public void Increment(GameObject page,ref int pgn)
    {
        pgn++;
        if(pgn>= currList.Count)
        {
            pgn = 0;
        }
        bookPage.enabled = false;
        bookPage.enabled = true;
        page.GetComponent<Renderer>().material = currList[pgn];
    }

    public void Decrement(GameObject page,ref int pgn)
    {
        pgn--;
        if ( pgn < 0)
        {
            pgn = currList.Count-1;
        }
        bookPage.enabled = false;
        bookPage.enabled = true;
        page.GetComponent<Renderer>().material = currList[pgn];
    }

    public void IncrementAll()
    {
        Increment(rightPage, ref rightPageNum);
        Increment(leftPage, ref leftPageNum);
        Increment(simPage, ref simPageNum);
        if (pairedMode)
        {
            Increment(versoInspectorPage,  ref versoInspectorPageNum);
            Increment(rectoInspectorPage,  ref rectoInspectorPageNum);
        }

    }

    public void decrementAll()
    {
        Decrement(rightPage,ref rightPageNum);
        Decrement(leftPage,ref leftPageNum);
        Decrement(simPage,ref simPageNum);
        if (pairedMode)
        {
            Decrement(versoInspectorPage,  ref versoInspectorPageNum);
            Decrement(rectoInspectorPage,  ref rectoInspectorPageNum);
        }

    }

    //increments only the inspector
    public void IncrementInspector()
    {
        Increment(versoInspectorPage, ref versoInspectorPageNum);
        Increment(rectoInspectorPage, ref rectoInspectorPageNum);
    }
    
    //decrements only the inspector
    public void DecrementInspector()
    {
        Decrement(versoInspectorPage, ref versoInspectorPageNum);
        Decrement(rectoInspectorPage, ref rectoInspectorPageNum);
    }

    //sets inspector pages to book's currently displayed pages, then inverts attachedMode bool
    public void ToggleInspectorMode()
    {
        if (pairedMode == false)
        {
            versoInspectorPageNum = leftPageNum;
            rectoInspectorPageNum = simPageNum;
            versoInspectorPage.GetComponent<Renderer>().material = currList[versoInspectorPageNum];
            rectoInspectorPage.GetComponent<Renderer>().material = currList[rectoInspectorPageNum];
        }
        pairedMode = !pairedMode;
    }

    public void ToggleInspector()
    {
        isInspectorOn = !isInspectorOn;
        versoInspectorPage.GetComponent<MeshRenderer>().enabled = isInspectorOn;
        rectoInspectorPage.GetComponent<MeshRenderer>().enabled = isInspectorOn;
    }

    public void ResetExperienceBM()
    {
        currList = pageList;
        leftPageNum = 0;
        simPageNum = 1;
        rightPageNum = 2; 
        versoInspectorPageNum = 0;
        rectoInspectorPageNum = 1;
        leftPage.GetComponent<Renderer>().material = currList[leftPageNum];
        simPage.GetComponent<Renderer>().material = currList[simPageNum];
        rightPage.GetComponent<Renderer>().material = currList[rightPageNum];
        versoInspectorPage.GetComponent<Renderer>().material = currList[versoInspectorPageNum];
        rectoInspectorPage.GetComponent<Renderer>().material = currList[rectoInspectorPageNum];

        ResetPageParticles(null);
        bookSystemObject.GetComponent<ParticlePositionManager>().LoadParticles("RightSideResting");
    }

    IEnumerator ResetPageRoutine(BaseInteractor interactor)
    {
        //REMOVED OBI 12/10/2023 
        // simPage.GetComponent<ObiTearableCloth>().ResetParticles();
        // simPage.GetComponent<ObiTearableCloth>().RemoveFromSolver();
        // simPage.GetComponent<ObiTearableCloth>().AddToSolver();

        yield return new WaitForEndOfFrame();

        yield return null;
    }

    public void ResetPageParticles(BaseInteractor interactor)
    {
        StartCoroutine(ResetPageRoutine(interactor));
    }
*/
    

    public int leftPageNum = 0;
    int simPageNum = 1;
    public int rightPageNum = 2;
    int versoInspectorPageNum = 0;
    int rectoInspectorPageNum = 1;
    public bool pairedMode = true;
    private bool isInspectorOn = true;

    [SerializeField] GameObject rightPage;
    [SerializeField] GameObject leftPage;
    [SerializeField] GameObject simPage;
    [SerializeField] GameObject versoInspectorPage;
    [SerializeField] GameObject rectoInspectorPage;
    [SerializeField] private GameObject bookSystemObject;
    [SerializeField] private SpringJoint bookPageSpringJoint;

    [SerializeField] public List<Material> pageList = new List<Material>();

    private List<Material> currList;


    public int getLeftPageNum()
    {
        return leftPageNum;
    }

    public int getRightPageNum()
    {
        return rightPageNum;
    }

    public int getSimPageNum()
    {
        return simPageNum;
    }
    private void Start()
    {
        currList = pageList;
        UpdatePageMaterials();
    }

    private void UpdatePageMaterials()
    {
        rightPage.GetComponent<Renderer>().material = currList[rightPageNum];
        leftPage.GetComponent<Renderer>().material = currList[leftPageNum];
        simPage.GetComponent<Renderer>().material = currList[simPageNum];
        versoInspectorPage.GetComponent<Renderer>().material = currList[versoInspectorPageNum];
        rectoInspectorPage.GetComponent<Renderer>().material = currList[rectoInspectorPageNum];
    }

    private void FlipPage(GameObject page, ref int pageNum, int increment)
    {
        pageNum += increment;
        if (pageNum >= currList.Count)
        {
            pageNum = 0;
        }
        else if (pageNum < 0)
        {
            pageNum = currList.Count - 1;
        }
        UpdatePageMaterials();
        // Adjust Spring Joint anchor to simulate page flip
        bookPageSpringJoint.anchor = new Vector3(bookPageSpringJoint.anchor.x, bookPageSpringJoint.anchor.y, -bookPageSpringJoint.anchor.z);
    }

    public void IncrementAll()
    {
        FlipPage(rightPage, ref rightPageNum, 1);
        FlipPage(leftPage, ref leftPageNum, 1);
        FlipPage(simPage, ref simPageNum, 1);
        if (pairedMode)
        {
            FlipPage(versoInspectorPage, ref versoInspectorPageNum, 1);
            FlipPage(rectoInspectorPage, ref rectoInspectorPageNum, 1);
        }
    }

    //increments only the inspector
    public void IncrementInspector()
    {
        Increment(versoInspectorPage, ref versoInspectorPageNum);
        Increment(rectoInspectorPage, ref rectoInspectorPageNum);
    }
    public void Increment(GameObject page, ref int pgn)
    {
        pgn++;
        if (pgn >= currList.Count)
        {
            pgn = 0;
        }
      //  bookPage.enabled = false;
      //  bookPage.enabled = true;
        page.GetComponent<Renderer>().material = currList[pgn];
    }
    public void DecrementAll()
    {
        FlipPage(rightPage, ref rightPageNum, -1);
        FlipPage(leftPage, ref leftPageNum, -1);
        FlipPage(simPage, ref simPageNum, -1);
        if (pairedMode)
        {
            FlipPage(versoInspectorPage, ref versoInspectorPageNum, -1);
            FlipPage(rectoInspectorPage, ref rectoInspectorPageNum, -1);
        }
    }
    public void ToggleInspectorMode()
    {
        if (pairedMode == false)
        {
            versoInspectorPageNum = leftPageNum;
            rectoInspectorPageNum = simPageNum;
            versoInspectorPage.GetComponent<Renderer>().material = currList[versoInspectorPageNum];
            rectoInspectorPage.GetComponent<Renderer>().material = currList[rectoInspectorPageNum];
        }
        pairedMode = !pairedMode;
    }

    public void ToggleInspector()
    {
        isInspectorOn = !isInspectorOn;
        versoInspectorPage.GetComponent<MeshRenderer>().enabled = isInspectorOn;
        rectoInspectorPage.GetComponent<MeshRenderer>().enabled = isInspectorOn;
    }

    public void ResetExperienceBM()
    {
        currList = pageList;
        leftPageNum = 0;
        simPageNum = 1;
        rightPageNum = 2;
        versoInspectorPageNum = 0;
        rectoInspectorPageNum = 1;
        leftPage.GetComponent<Renderer>().material = currList[leftPageNum];
        simPage.GetComponent<Renderer>().material = currList[simPageNum];
        rightPage.GetComponent<Renderer>().material = currList[rightPageNum];
        versoInspectorPage.GetComponent<Renderer>().material = currList[versoInspectorPageNum];
        rectoInspectorPage.GetComponent<Renderer>().material = currList[rectoInspectorPageNum];

        ResetPageParticles(null);
        bookSystemObject.GetComponent<ParticlePositionManager>().LoadParticles("RightSideResting");
    }

    IEnumerator ResetPageRoutine(BaseInteractor interactor)
    {
        //REMOVED OBI 12/10/2023 
        // simPage.GetComponent<ObiTearableCloth>().ResetParticles();
        // simPage.GetComponent<ObiTearableCloth>().RemoveFromSolver();
        // simPage.GetComponent<ObiTearableCloth>().AddToSolver();

        yield return new WaitForEndOfFrame();

        yield return null;
    }

    public void ResetPageParticles(BaseInteractor interactor)
    {
        StartCoroutine(ResetPageRoutine(interactor));
    }

}
