using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; 

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tooltipText;

    private void Start()
    {
        tooltipText.gameObject.SetActive(false); // ȷ��Tooltip��ʼʱ���ɼ�
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
