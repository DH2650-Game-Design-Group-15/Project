using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject dialogueTemplate;
    public GameObject dialogueTextObject;

    TextMeshProUGUI dialogueText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowDialogue(string dialogue)
    {
        dialogueText = dialogueTextObject.GetComponent<TextMeshProUGUI>();
        dialogueText.text = dialogue;
        dialogueTemplate.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogueTemplate.SetActive(false);
    }
}
