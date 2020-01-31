using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private bool selected = false;
    private Vector2 startingPosition;
    private Vector2 offSetPosition;
    public GameObject draggingManager;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && !draggingManager.GetComponent<DraggingObjects>().getIsDragging())
        {
            draggingManager.GetComponent<DraggingObjects>().setDragging(true);
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selected = true;
            offSetPosition = new Vector2(cursorPos.x - transform.localPosition.x, cursorPos.y - transform.localPosition.y);
        }
    }

    private void Update()
    {
        if (selected)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x - offSetPosition.x, cursorPos.y - offSetPosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            selected = false;
            draggingManager.GetComponent<DraggingObjects>().setDragging(false);
        }

    }
 



}
