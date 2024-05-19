using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; 

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tooltipText;
    public string messageToShow; // 用于在Inspector中设置不同按钮的文本

    private void Start()
    {
        tooltipText.gameObject.SetActive(false); // 确保Tooltip初始时不可见
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        //tooltipText.text = messageToShow; // 使用Inspector中设置的文本
        tooltipText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        tooltipText.gameObject.SetActive(false);
    }
}
