using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScroller : MonoBehaviour
{
    [SerializeField] private float topLimit = 0.0f;
    [SerializeField] private float bottomLimit = 206.0f;

    [SerializeField] private float scrollSpeed = 20.0f;

    [SerializeField] private GameObject canvas;

    bool isScrolling;
    float scrollDirection = 0.0f;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isScrolling)
            return;

        Vector3 pos = rectTransform.localPosition;

        pos.y += scrollDirection * Time.deltaTime * scrollSpeed;

        if(pos.y < topLimit)
            pos.y = topLimit;

        if(pos.y > bottomLimit)
            pos.y = bottomLimit;

        rectTransform.localPosition = pos;
    }

    public void ScrollUp()
    {
        scrollDirection = -1.0f;
        isScrolling = true;
    }

    public void ScrollDown()
    {
        scrollDirection = 1.0f;
        isScrolling = true;
    }

    public void StopScrolling()
    {
        scrollDirection = 0.0f;
        isScrolling = false;
    }
}
