using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSystemNew : MonoBehaviour
{
    public int leftPageNum = 0;
    int simPageNum = 1;
    public int rightPageNum = 2;
    [SerializeField] GameObject leftPage, rightPage, simPage;
    [SerializeField] Collider leftCollider, rightCollider;

    [SerializeField] public List<Material> pageList = new List<Material>();
    private List<Material> currList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
