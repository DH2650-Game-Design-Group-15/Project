using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public Inputs inputs;
    public GameObject pauseMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        ResumeGame();
    }

    public void OnPauseGame(InputAction.CallbackContext context){
        if (context.started){
            PauseGame();
        }
    }

    public void OnResumeGame(InputAction.CallbackContext context){
        if (context.started){
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        inputs.ChangeActionMap("Pause");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        inputs.ReturnToActionMap();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
