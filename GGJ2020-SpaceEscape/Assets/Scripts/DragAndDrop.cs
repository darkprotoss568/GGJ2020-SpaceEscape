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
    private bool isOverTheFrame = false;
    private bool isPositioned = false;
    private GameObject frameObject;
    private GameObject frameObjectPositioned;
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

        Debug.Log("isOver");
    }

    private void OnMouseUp()
    {
        Debug.Log("up");
        draggingManager.GetComponent<DraggingObjects>().setDragging(false);
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
            draggingManager.GetComponent<DraggingObjects>().setDragging(false);
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            

            if (frameObject != null)
            {

                if (isOverTheFrame && !isPositioned && !frameObject.GetComponent<Frames>().getIsObjectInFrame())
                {
                    isPositioned = true;
                    //Saving the object in we are positioned
                    frameObjectPositioned = frameObject;
                    //framePosition = frameObjectPositioned.GetComponent<Frames>().getPosition();

                    //Debug.Log(framePosition);
                    transform.position = frameObjectPositioned.GetComponent<Frames>().getPosition();
                    //Debug.Log("Set here");
                    frameObjectPositioned.GetComponent<Frames>().setObjectInFrame(true);
                }
                
                else{

                    //Debug.Log("isOverTheFrame = " + isOverTheFrame);
                    //Debug.Log("isPositioned = " + isPositioned);
                    //Debug.Log("frame has Object = " + frameObject.GetComponent<Frames>().getIsObjectInFrame());
                    if (!isOverTheFrame)
                    {
                        transform.position = startingPosition;
                        //Debug.Log("Here2");

                    }

                    if (!isPositioned)
                    {
                        transform.position = startingPosition;
                        if (frameObjectPositioned != null)
                        {
                            if (frameObjectPositioned.GetComponent<Frames>().getIsObjectInFrame())
                            {
                                frameObjectPositioned.GetComponent<Frames>().setObjectInFrame(false);
                                frameObjectPositioned = null;
                            }
                        }
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

            

            isOverTheFrame = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Frame"))
        {
            isOverTheFrame = false;
            isPositioned = false;
            draggingManager.GetComponent<DraggingObjects>().setDragging(false);
            if (frameObjectPositioned != null && frameObjectPositioned.GetComponent<Frames>().getIsObjectInFrame())
            {
                frameObjectPositioned.GetComponent<Frames>().setObjectInFrame(false);
            }
        }
    }

}
