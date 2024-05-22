using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; 

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tooltipText;

    private void Start()
    {
        tooltipText.gameObject.SetActive(false); // 确保Tooltip初始时不可见
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        tooltipText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        tooltipText.gameObject.SetActive(false);
    }
}
