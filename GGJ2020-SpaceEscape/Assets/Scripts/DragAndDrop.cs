using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private bool selected = false;
    private Vector2 startingPosition;
    private Vector2 offSetPosition;
    public GameObject draggingManager;
    private Vector2 framePosition;
    private bool isOnFrame = false;
    private bool isPositioned = false;
    private GameObject frameObject;
    float gameTimer;
    float checkingTime = 1.0f;

    private void Start()
    {
        startingPosition = transform.position;
        gameTimer = 0.0f;
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
        gameTimer -= Time.deltaTime;

        if (selected)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x - offSetPosition.x, cursorPos.y - offSetPosition.y);
            GetComponent<SpriteRenderer>().sortingOrder = 500;
        }

        if (Input.GetMouseButtonUp(0) && selected)
        {
            gameTimer = checkingTime;
            selected = false;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            draggingManager.GetComponent<DraggingObjects>().setDragging(false);

            if (frameObject != null)
            {

                if (isOnFrame && !isPositioned && !frameObject.GetComponent<Frames>().getIsObjectInFrame())
                {
                    isPositioned = true;
                    transform.position = framePosition;
                    Debug.Log("Set here");
                    frameObject.GetComponent<Frames>().setObjectInFrame(true);
                }
                else{
                    if (!isOnFrame)
                    {
                        transform.position = startingPosition;
                        Debug.Log("Here2");
                        frameObject.GetComponent<Frames>().setObjectInFrame(false);

                    }

                    if (!isPositioned)
                    {
                        transform.position = startingPosition;
                    }

                }
            }
        }

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Frame"))
        {
            frameObject = collision.gameObject;

            framePosition = collision.GetComponent<Frames>().getPosition();
            isOnFrame = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Frame"))
        {
            isOnFrame = false;
            isPositioned = false;
        }
    }

}
