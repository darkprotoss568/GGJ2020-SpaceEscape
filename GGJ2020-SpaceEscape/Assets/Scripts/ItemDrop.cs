using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrop : MonoBehaviour, IDropHandler
{
    private bool ImEmpty = true;
    private GameObject objectDropped;
    private bool isMouseReleased = true;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && ImEmpty)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            ImEmpty = false;
			gameObject.GetComponent<RepairSlots>().CurrentAnswer = eventData.pointerDrag.GetComponent<RepairObjScript>().AnswerClip;
            objectDropped = eventData.pointerDrag;
            GameManager.Instance.SwitchMode(true);
        }
    }

    public bool getIsEmpty()
    {
        return ImEmpty;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        

    }


    public void OnTriggerStay2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Objects") && ImEmpty && isMouseReleased)
        {
            objectDropped = collision.gameObject;
            objectDropped.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            ImEmpty = false;

        }

        if (collision.gameObject == objectDropped && isMouseReleased)
        {
            objectDropped.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }


    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == objectDropped)
        {
            ImEmpty = true;
            gameObject.GetComponent<RepairSlots>().CurrentAnswer = null;

            GameManager.Instance.SwitchMode(false);
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0)) {
            isMouseReleased = true;
        }
        else
        {
            isMouseReleased = false;
        }

    }
}
