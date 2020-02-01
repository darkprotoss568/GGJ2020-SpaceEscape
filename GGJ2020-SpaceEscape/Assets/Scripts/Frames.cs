using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frames : MonoBehaviour
{

    private Vector2 startingPosition;
    private bool isObjectInFrame = false;


    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    public void setObjectInFrame(bool frameBool)
    {
       
        isObjectInFrame = frameBool;
        //Debug.Log(frameBool);
    }

    public bool getIsObjectInFrame()
    {
        return isObjectInFrame;
    }

    public Vector2 getPosition()
    {
        return startingPosition;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (!isObjectInFrame)
        {
            Debug.Log("out");

        }
        //isObjectInFrame = false;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isObjectInFrame)
        {
            Debug.Log("in");

        }

    }
}
