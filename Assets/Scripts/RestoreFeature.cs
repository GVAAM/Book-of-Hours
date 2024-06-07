using System.Collections;
using System.Collections.Generic;
using HandPhysicsToolkit.Helpers.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
// using Obi;


public class RestoreFeature : MonoBehaviour
{
    [SerializeField] private GameObject simPage; // simpage
    [SerializeField] private GameObject sidePage; // non-simpage side page
    [SerializeField] private Material triggerMat;

    [SerializeField] private bool onLeft;
    private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    private Quaternion defaultSimRotation;
    [SerializeField] private GameObject _spineCenter;
    [SerializeField] private GameObject _gp;
    private GraspingPoint _graspingPoint;
    private float speed = 1f;
    private float t;

    // private ObiParticleGroup _particleGroup;
    // private ObiActor actor;
    [SerializeField] private string particleGroupName;
    [SerializeField] private int particleIdx;
  
    private GraspingPoint.SimPageSide side;
    [SerializeField] GameObject mesh;

    [SerializeField] Material restoredMaterial;
    private bool isRestored;
    private Vector3 staticLoc;

    private Material originalMat;
    [SerializeField] private Material highlightMat;
    public float fadeDuration = 1f;

    private GameObject currPage;
    [SerializeField] private RestoreMode restoreMode;
    [SerializeField] private PageEdgeGrabTrigger pegt;
    private float minDist;

    [SerializeField] private GameObject text;

    
    // Start is called before the first frame update
    private void Awake()
    {
        // actor = simPage.GetComponent<ObiActor>();
        _graspingPoint = _gp.GetComponent<GraspingPoint>();
        //mesh.layer = LayerMask.NameToLayer("OutlineLayer");
        side = onLeft ? GraspingPoint.SimPageSide.LeftSide : GraspingPoint.SimPageSide.RightSide;
        isRestored = false;
        originalMat = gameObject.GetComponent<MeshRenderer>().material;
        defaultRotation = mesh.transform.rotation;
        defaultPosition = mesh.transform.position;
        defaultSimRotation = Quaternion.Euler(0.2f, 0f, 0f);
        t = 0f;
        currPage = sidePage;
        // foreach (var @group in actor.blueprint.groups.Where(@group => @group.name == particleGroupName))
        // {
        //     _particleGroup = @group;
        //
        // }
        // particleIdx = _particleGroup.particleIndices[particleIdx];
        // particleIdx = _particleGroup.particleIndices[0];
        // var particlePos = actor.GetParticlePosition(particleIdx);
        // minDist = Vector3.Distance(particlePos, defaultPosition);
        text.SetActive(false);
    }


    // Update is called once per frame
    // private void Update() 
    // {
    //     currPage = _graspingPoint.GetSimPageSide() == side ?  simPage : sidePage; // see which side the sim page is on
    //     
    //
    //     if (currPage == simPage)
    //     {
    //         Vector3 loc = actor.GetParticlePosition(particleIdx);
    //
    //         if (restoreMode.getHandStatus())
    //         {
    //             Vector3 lineVector = _gp.transform.position - _spineCenter.transform.position;
    //             lineVector.z = 0;
    //             lineVector.y = Mathf.Abs(lineVector.y);
    //             float angle = Vector3.Angle(lineVector, Vector3.left);
    //             Quaternion rotationQuaternion = Quaternion.Euler(-1f, 0f, -1*angle);
    //             Vector3 perpendicularVector = Vector3.Cross(lineVector, Vector3.forward);
    //             perpendicularVector.Normalize();
    //             perpendicularVector.y = perpendicularVector.y >= 0 ? perpendicularVector.y + 1 : Mathf.Abs(perpendicularVector.y - 1); 
    //             mesh.transform.SetPositionAndRotation(loc + perpendicularVector * 1.2f, rotationQuaternion);
    //         }
    //         else if (mesh.transform.rotation != defaultRotation)
    //         {
    //             Vector3 lineVector = _gp.transform.position - _spineCenter.transform.position;
    //             lineVector.z = 0;
    //             float angle = onLeft? Vector3.Angle(lineVector, Vector3.left) : Vector3.Angle(lineVector, -1*Vector3.left);
    //             Quaternion rotationQuaternion = Quaternion.Lerp(Quaternion.Euler(-1f, 0f, -1 * angle), defaultRotation, Mathf.Min(t*speed, 1f));
    //             Vector3 perpendicularVector = Vector3.Cross(lineVector, Vector3.forward);
    //             perpendicularVector.Normalize();
    //             perpendicularVector.y = Mathf.Abs(perpendicularVector.y);
    //             mesh.transform.SetPositionAndRotation(loc + perpendicularVector * 1.2f, rotationQuaternion);
    //             t += Time.deltaTime;
    //         }
    //         else
    //         {
    //             t = 0f;
    //         }
    //
    //         var closestPoint = sidePage.GetComponent<Collider>().ClosestPoint(loc);
    //         staticLoc = closestPoint + new Vector3(0f, 0.5f, 0f);
    //     }
    //     else
    //     {
    //         mesh.transform.position = staticLoc;
    //         mesh.transform.rotation = defaultRotation; 
    //     }
    //     
    // }

    public void toggleRestored()
    {
        if (!isRestored)
        {
            isRestored = true;
            text.SetActive(true);
            StartCoroutine(fadeOut(currPage.GetComponent<MeshRenderer>().material, restoredMaterial, currPage)); // fade effect
            // currPage.GetComponent<MeshRenderer>().material = restoredMaterial;
            // currPage.GetComponent<MaterialSwitcher>().setHighlightMat(restoredMaterial);
            // currPage.GetComponent<MaterialSwitcher>().setOriginalMat(restoredMaterial);
            
        }
        else
        {
            isRestored = false;
            text.SetActive(false);
            StartCoroutine(fadeOut(restoredMaterial, triggerMat, currPage));
            // currPage.GetComponent<MeshRenderer>().material = triggerMat;
            // currPage.GetComponent<MaterialSwitcher>().setHighlightMat(highlightMat);
            // currPage.GetComponent<MaterialSwitcher>().setOriginalMat(triggerMat);
           
        }
    }

    // public IEnumerator particleLocator()
    // {
    //     foreach (var pIdx in _particleGroup.particleIndices)
    //     {
    //         var particlePos = actor.GetParticlePosition(pIdx);
    //         if (Vector3.Distance(particlePos, defaultPosition) < minDist)
    //         {
    //             particleIdx = pIdx;
    //             minDist = Vector3.Distance(particlePos, defaultPosition);
    //         }
    //     }
    //     yield return null;
    // }

    public IEnumerator fadeOut(Material startMat, Material endMat, GameObject mesh)
    {
        float elapsedTime = 0f;
        Material newMaterial = new Material(startMat);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            // interpolate the material properties
            // renderer.material.color.a = Mathf.Lerp(alphaStart,alphaEnd,lep);
            newMaterial.color = Color.Lerp(startMat.color, endMat.color, t);
            newMaterial.mainTextureOffset = Vector2.Lerp(startMat.mainTextureOffset, endMat.mainTextureOffset, t);

            mesh.GetComponent<MeshRenderer>().material = newMaterial;

            yield return null;
        }

        mesh.GetComponent<MeshRenderer>().material = endMat;
    }

}

