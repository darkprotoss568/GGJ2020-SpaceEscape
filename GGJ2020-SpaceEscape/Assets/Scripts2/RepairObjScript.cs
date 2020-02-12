using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RepairObjScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    AudioClip answerClip = null;

    public AudioClip AnswerClip
    {
        get
        {
            return answerClip;
        }

        set
        {
            answerClip = value;
        }
    }
    // Start is called before the first frame update

        
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.SetMouseHoverPromptActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.SetMouseHoverPromptActive(false);
    }
    
}
