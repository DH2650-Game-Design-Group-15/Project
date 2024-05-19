using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; 

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tooltipText;
    public string messageToShow; // ������Inspector�����ò�ͬ��ť���ı�

    private void Start()
    {
        tooltipText.gameObject.SetActive(false); // ȷ��Tooltip��ʼʱ���ɼ�
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        //tooltipText.text = messageToShow; // ʹ��Inspector�����õ��ı�
        tooltipText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        tooltipText.gameObject.SetActive(false);
    }
}
