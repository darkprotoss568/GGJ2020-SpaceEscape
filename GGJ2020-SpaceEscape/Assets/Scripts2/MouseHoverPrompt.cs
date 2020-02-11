using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseHoverPrompt : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField]
    private CanvasScaler canvasScaler;

    void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToCursorPos();
    }

    public void MoveToCursorPos()
    {
        Vector3 cursorPos = Input.mousePosition;

        rectTransform.anchoredPosition = new Vector2(cursorPos.x * canvasScaler.referenceResolution.x / Screen.width, cursorPos.y * canvasScaler.referenceResolution.y / Screen.height);

        //Debug.Log(rectTransform.anchoredPosition);
        float newX = rectTransform.anchoredPosition.x;
        float newY = rectTransform.anchoredPosition.y;

        newX = Mathf.Clamp(newX, rectTransform.sizeDelta.x / 2, canvasScaler.referenceResolution.x - rectTransform.sizeDelta.x / 2);
        newX = Mathf.Clamp(newY, rectTransform.sizeDelta.y / 2, canvasScaler.referenceResolution.y - rectTransform.sizeDelta.y / 2);

        rectTransform.anchoredPosition = new Vector2(newX, newY);
    }

    public void SetActiveState(bool value)
    {
        if (gameObject.activeInHierarchy != value)
        {
            gameObject.SetActive(value);

            if (value)
                gameObject.transform.SetAsLastSibling();
            else
                gameObject.transform.SetAsFirstSibling();
        }
    }
}
