using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private Vector3 startingPos;
    private CanvasGroup canvasGroup;
    private bool frameIsEmpty = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startingPos = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown( PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       // Debug.Log("OnBeginDragHandler");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDragHandler");
        canvasGroup.blocksRaycasts = true;

        if(!frameIsEmpty)rectTransform.anchoredPosition = startingPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
       // Debug.Log("OnDrop");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("isWorking?");

        if (collision.gameObject.CompareTag("Frame"))
        {

            frameIsEmpty = collision.gameObject.GetComponent<ItemDrop>().getIsEmpty();

        }

    }
}
