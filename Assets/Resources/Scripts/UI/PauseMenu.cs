using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public Inputs inputs;
    public GameObject pauseMenu;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        ResumeGame();
    }

    public void OnPressPause(InputAction.CallbackContext context){
        if (context.started){
            if (isPaused){
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        inputs.ReturnToActionMap();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Pause");
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        inputs.ChangeActionMap("Pause");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Resume");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If we are running in a build
        Application.Quit();
#endif
    }
}
