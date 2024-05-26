using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    public Image questBG; // Reference to the QuestBG Image
    private Button button; // Reference to the Button component

    void Start()
    {
        // Get the Button component attached to the same GameObject
        button = GetComponent<Button>();

        // Add a listener to the button to call the ChangeColorAndHide method when clicked
        button.onClick.AddListener(ChangeColorAndHide);
    }

    void ChangeColorAndHide()
    {
        // Change the color of the QuestBG Image to green
        if (questBG != null)
        {
            questBG.color = new Color(50, 82, 60, 0.4f);
        }

        // Hide the button by disabling it
        button.gameObject.SetActive(false);
    }
}
