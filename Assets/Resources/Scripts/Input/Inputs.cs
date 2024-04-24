using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Helper functions for the Input maps.
/// Functions can return all active maps or activate or deactivate a map
/// </summary>
public class Inputs : MonoBehaviour {
    private PlayerInput playerInput;
    private List<InputActionMap> lastMaps;

    /// <summary>
    /// Finds a playerInput in the parents and all their children, including itself and siblings.
    /// </summary>
    void Start() {
        Transform parent = transform.parent ?? transform;
        playerInput = parent.GetComponentInChildren<PlayerInput>();
        lastMaps = new();
    }

    /// <summary>
    /// Deactivates the input map with the given name. 
    /// </summary>
    public bool RemoveActionMap(string mapName){
        InputActionMap map = playerInput.actions.FindActionMap(mapName);
        bool oldValue = map.enabled;
        playerInput.actions.FindActionMap(mapName).Disable();
        return oldValue;
    }

    /// <summary>
    /// Activates the input map with the given name. Doesn't activate different action maps.
    /// This can lead to different events on the same key.
    /// </summary>
    public bool AddActionMap(string mapName){
        InputActionMap map = playerInput.actions.FindActionMap(mapName);
        bool oldValue = map.enabled;
        playerInput.actions.FindActionMap(mapName).Enable();
        return !oldValue;
    }

    /// <summary>
    /// Disables all active input maps and activates only the one with the given name.
    /// </summary>
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

    /// <summary>
    /// Deactivates all active input maps and activates all input maps with the given names.
    /// </summary>
    public void ChangeActionMaps(string[] mapNames){
        ChangeActionMap(mapNames[0]);
        for (int i = 1; i < mapNames.Length; i++){
            AddActionMap(mapNames[i]);
        }
    }

    /// <summary>
    /// Deactivates all active input maps.
    /// Activates all input maps, that were active before last time ChangeActionMap or ChangeActionMaps was called
    /// </summary>
    public void ReturnToActionMap(){
        List<InputActionMap> newLastMaps = new();
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

    /// <summary>
    /// Returns all active input maps.
    /// </summary>
    public List<string> GetActionMaps(){
        List<string> maps = new();
        foreach(InputActionMap map in playerInput.actions.actionMaps){
            if(map.enabled){
                maps.Add(map.name);
            }
        }
        return maps;
    }
}
