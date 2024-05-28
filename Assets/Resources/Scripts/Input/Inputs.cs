using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Inputs helps to change the active input map, to enable or disable multiple ones and to go back to other input maps.
/// </summary>
public class Inputs : MonoBehaviour {
    private PlayerInput playerInput;
    private List<InputActionMap> lastMaps;

    /// <summary>
    /// Finds a PlayerInput in the parent and all its children.
    /// </summary>
    void Awake() {
        Transform parent = transform.parent ?? transform;
        playerInput = parent.GetComponentInChildren<PlayerInput>();
        lastMaps = new();
    }

    /// <summary>
    /// Deactivates the input map with the given name. 
    /// </summary>
    /// <param name="mapName"> The name of the map to disable. </param>
    /// <returns> True, if the map was enabled before, else false. </returns>
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
    /// <param name="mapName"> The name of the map to enable. </param>
    /// <returns> True, if the map was disabled before, else false. </returns>
    public bool AddActionMap(string mapName){
        InputActionMap map = playerInput.actions.FindActionMap(mapName);
        bool oldValue = map.enabled;
        playerInput.actions.FindActionMap(mapName).Enable();
        return !oldValue;
    }

    /// <summary>
    /// Disables all active input maps and activates only the one with the given name.
    /// </summary>
    /// <param name="mapName"> The name of the only map to enable. </param>
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
    /// <param name="mapName"> The name of all maps to enable. </param>
    public void ChangeActionMaps(string[] mapNames){
        ChangeActionMap(mapNames[0]);
        for (int i = 1; i < mapNames.Length; i++){
            AddActionMap(mapNames[i]);
        }
    }

    /// <summary>
    /// Deactivates all active input maps.
    /// Activates all input maps, that were active before all active action maps where switched.
    /// </summary>
    /// <seealso cref="ChangeActionMaps(string[])"/>
    /// <seealso cref="ChangeActionMap(string)"\>
    public void ReturnToActionMap(){
        List<InputActionMap> newLastMaps = new();
        foreach(InputActionMap map in playerInput.actions.actionMaps){
            if (map.enabled){
                newLastMaps.Add(map);
            }
            map.Disable();
        }
        if (lastMaps.Count == 0){
            lastMaps.Add(playerInput.actions.FindActionMap("Player"));
        }
        foreach(InputActionMap map in lastMaps){
            map.Enable();
        }
    }

    /// <summary>
    /// Returns all active input maps.
    /// </summary>
    /// <returns> A list of all active input maps. </returns>
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
