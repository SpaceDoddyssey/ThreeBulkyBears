using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverColor, defaultColor;
    public TextMeshProUGUI text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defaultColor;
    }
}