using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour {
    private PlayerInput playerInput;
    private ArrayList lastMaps;

    /// <summary>
    /// Finds a playerInput in the parents and all their children, including itself and siblings.
    /// </summary>
    void Start() {
        Transform parent = transform.parent ?? transform;
        playerInput = parent.GetComponentInChildren<PlayerInput>();
        lastMaps = new ArrayList();
    }

    public bool RemoveActionMap(string mapName){
        InputActionMap map = playerInput.actions.FindActionMap(mapName);
        bool oldValue = map.enabled;
        playerInput.actions.FindActionMap(mapName).Disable();
        return oldValue;
    }

    public bool AddActionMap(string mapName){
        InputActionMap map = playerInput.actions.FindActionMap(mapName);
        bool oldValue = map.enabled;
        playerInput.actions.FindActionMap(mapName).Enable();
        return !oldValue;
    }

    public void ChangeActionMap(string mapName){
        lastMaps.Clear();
        foreach (InputActionMap map in playerInput.actions.actionMaps) {
            if (map.enabled){
                lastMaps.Add(map);
                map.Disable();
            }
        }
        AddActionMap(mapName);
    }

    public void ChangeActionMaps(string[] mapNames){
        ChangeActionMap(mapNames[0]);
        for (int i = 1; i < mapNames.Length; i++){
            AddActionMap(mapNames[i]);
        }
    }

    public void ReturnToActionMap(){
        ArrayList newLastMaps = new ArrayList();
        foreach(InputActionMap map in playerInput.actions.actionMaps){
            if (map.enabled){
                newLastMaps.Add(map);
            }
            map.Disable();
        }
        foreach(InputActionMap map in lastMaps){
            map.Enable();
        }
    }
}
