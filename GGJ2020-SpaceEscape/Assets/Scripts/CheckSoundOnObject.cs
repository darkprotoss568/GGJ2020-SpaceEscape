using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckSoundOnObject : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            MusicGameplayManager.Instance.StopMusicPlay();
            MusicGameplayManager.Instance.PlayNoise(GetComponent<RepairObjScript>().AnswerClip);
        }
    }

}
