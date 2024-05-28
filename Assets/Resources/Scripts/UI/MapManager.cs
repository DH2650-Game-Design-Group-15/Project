using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class MapManager : MonoBehaviour
{   
    public int maxZoomLevel;
    public int startZoomLevel;
    public int minZoomLevel;
    public int zoomStep;
    public int mapHeight;
    public GameObject map;
    public GameObject mapCameraPosition;
    public GameObject player;
    public InventoryInput inventoryInput;
    private bool isMapOpen = false;
    private bool isMovingMap;
    private int zoomLevel;
    private Vector2 startPos;
    private Vector3 prevMapPos;
    private Camera mapCamera;

    private void Start()
    {   
        SetZoomLevel(startZoomLevel);
        map.SetActive(false);
        mapCamera = mapCameraPosition.GetComponent<Camera>();
    }
    
    private void LateUpdate () {
        if (GetIsMovingMap() && GetISMapOpen()) {
            Vector2 mapTraversal = Mouse.current.position.ReadValue() - startPos;
            mapCameraPosition.transform.position = prevMapPos + new Vector3(-mapTraversal.x, 0, -mapTraversal.y);
        }
    }

    public void SetIsMovingMap (InputAction.CallbackContext context) {
        if (context.started) {
            isMovingMap = true;
            startPos = Mouse.current.position.ReadValue();
            prevMapPos = mapCameraPosition.transform.position;
        } else if (context.canceled) {
            isMovingMap = false;
        }
    }

    private bool GetIsMovingMap() {
        return isMovingMap;
    }

    public void SetISMapOpen (InputAction.CallbackContext context) {
        if (context.started) {
            isMapOpen = !GetISMapOpen();
            inventoryInput.SetCursor(GetISMapOpen());
            if (GetISMapOpen()) {
                mapCameraPosition.transform.position = player.transform.position + new Vector3(0, mapHeight, 0);
                mapCamera.orthographicSize = startZoomLevel;
            }
            map.SetActive(GetISMapOpen());
        }
    }

    private bool GetISMapOpen () {
        return isMapOpen;
    }

    public void mapZoom (InputAction.CallbackContext context) {
        float scrollDir = -context.ReadValue<float>();
        if (scrollDir < 0 && GetZoomLevel() >= minZoomLevel + zoomStep) {
            SetZoomLevel(GetZoomLevel() - zoomStep);
        } else if (scrollDir > 0 && GetZoomLevel() <= maxZoomLevel - zoomStep) {
            SetZoomLevel(GetZoomLevel() + zoomStep);
        }
        mapCamera.orthographicSize = GetZoomLevel();
    }

    private void SetZoomLevel (int height) {
        zoomLevel = height;
    }

    private int GetZoomLevel () {
        return zoomLevel;
    }

}
