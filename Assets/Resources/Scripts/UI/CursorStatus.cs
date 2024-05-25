using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStatus : MonoBehaviour {

    public FirstPersonController firstPersonController;
    public InputManager inputManager;

    /// <summary> Enables or disables the cursor </summary>
    /// <param name="active" true, if the cursor should be enabled, false if the cursor should be disabled. </param>
    public void SetCursor(bool active){
        if (active){
            Cursor.lockState = CursorLockMode.Confined;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        firstPersonController.enabled = !active;
        inputManager.enabled = !active;
        Cursor.visible = active;
    }
    
}
