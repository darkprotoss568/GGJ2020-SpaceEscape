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
    private bool frameIsUsable = false;
    private bool ImNotColliding = true;
    private GameObject previousFrame;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ResetStartingPos();
        canvasGroup = GetComponent<CanvasGroup>();
    }
	
	void Start()
	{
		canvas = GameObject.FindGameObjectsWithTag("Canvas")[0].GetComponent<Canvas>();
	}

    public void OnPointerDown( PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

	public void ResetStartingPos()
	{
		startingPos = rectTransform.anchoredPosition;
	}
	
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        if(eventData.button == PointerEventData.InputButton.Left) {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       // Debug.Log("OnBeginDragHandler");
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetAsLastSibling();
        MusicGameplayManager.Instance.StopMusicPlay();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!frameIsUsable) {
            rectTransform.anchoredPosition = startingPos;
        }

        if (ImNotColliding)
        {
            rectTransform.anchoredPosition = startingPos;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
       // Debug.Log("OnDrop");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Frame"))
        {
            frameIsUsable = collision.gameObject.GetComponent<ItemDrop>().getIsEmpty();
            ImNotColliding = !frameIsUsable;
            previousFrame = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Frame") && collision.gameObject == previousFrame)
        {
            ImNotColliding = true;
        }
    }
}
