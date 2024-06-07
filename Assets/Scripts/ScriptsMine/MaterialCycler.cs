using UnityEngine;

public class MaterialCycler : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinnedMeshRenderers; // Array to hold references to the skinned mesh renderers
    public Material[] materials; // Array to hold references to the materials

    private int currentIndex = 0; // Current index for the materials

    public GameObject rightPage, leftPage;

    void Start()
    {
        // Initialize the materials for the skinned mesh renderers on start
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            Material[] initialMaterials = new Material[2];
            initialMaterials[0] = materials[(i * 2) % materials.Length];
            initialMaterials[1] = materials[(i * 2 + 1) % materials.Length];
            skinnedMeshRenderers[i].materials = initialMaterials;
        }
    }

    void Update()
    {
        // Check for the "N" key press to cycle materials forward
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnButtonN();
        }

        // Check for the "P" key press to cycle materials backward
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnButtonP();
        }
    }

    public void OnButtonN()
    {
        // Increment the current index by 2 and wrap around if necessary
        currentIndex = (currentIndex + 2) % materials.Length;
        Instantiate(rightPage, gameObject.transform);
        UpdateMaterials();
    }

    public void OnButtonP()
    {
        // Decrement the current index by 2 and wrap around if necessary
        currentIndex = (currentIndex - 2 + materials.Length) % materials.Length;
        Instantiate(leftPage, gameObject.transform);
        UpdateMaterials();
    }

    private void UpdateMaterials()
    {
       /* // Assign new pairs of materials to the skinned mesh renderers based on the current index
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            int materialIndex1 = (currentIndex + i * 2) % materials.Length;
            int materialIndex2 = (currentIndex + i * 2 + 1) % materials.Length;
            Material[] newMaterials = new Material[2];
            newMaterials[0] = materials[materialIndex1];
            newMaterials[1] = materials[materialIndex2];
            skinnedMeshRenderers[i].materials = newMaterials;
        }*/
    }
}
