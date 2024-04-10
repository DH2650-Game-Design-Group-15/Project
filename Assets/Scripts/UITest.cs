using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITest : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject dialogueTextObject;
    public string dialogue;
    TextMeshProUGUI dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText = dialogueTextObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        dialogueText.text = dialogue;
    }
}
