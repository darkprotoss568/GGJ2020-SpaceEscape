using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingObjects : MonoBehaviour
{

    private bool isDragging = false;
    
    public void setDragging( bool drag)
    {
        isDragging = drag;
        Debug.Log("isDragging: " + drag);
    }

    public bool getIsDragging()
    {
        return isDragging;
    }
}
