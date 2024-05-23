using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    public float detectionAngle = 45f;  // Horizontaler Sichtwinkel
    public float verticalDetectionAngle = 45f;  // Vertikaler Sichtwinkel
    public float detectionDistance = 10f;
    public LayerMask obstacleMask;

    public GameObject[] DetectObjects()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionDistance);
        List<GameObject> objects = new();

        foreach (Collider obj in detectedObjects)
        {
            if (obj.gameObject != gameObject)
            {
                Vector3 directionToObject = obj.transform.position - transform.position;
                float horizontalAngleToObject = Vector3.Angle(transform.forward, new Vector3(directionToObject.x, 0, directionToObject.z).normalized);
                float verticalAngleToObject = Vector3.Angle(transform.forward, new Vector3(0, directionToObject.y, directionToObject.z).normalized);

                if (Math.Abs(horizontalAngleToObject) <= detectionAngle * 0.5f && verticalAngleToObject <= Math.Abs(verticalDetectionAngle) * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToObject, out hit, detectionDistance, obstacleMask))
                    {
                        if (hit.collider.gameObject == obj.gameObject)
                        {
                            objects.Add(obj.gameObject);
                        }
                    }
                }
            }
        }
        return objects.ToArray();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);

        // Draw filled arc for horizontal detection angle
        Handles.color = new Color(1, 0, 0, 0.3f); // Red with some transparency
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward, detectionAngle, detectionDistance);

        // Draw filled arc for vertical detection angle
        Handles.color = new Color(0, 1, 0, 0.3f); // Green with some transparency
        Handles.DrawSolidArc(transform.position, transform.right, Quaternion.Euler(-verticalDetectionAngle * 0.5f, 0, 0) * transform.forward, verticalDetectionAngle, detectionDistance);
    }
/*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);

        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle * 0.5f, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
        Vector3 upBoundary = Quaternion.Euler(-verticalDetectionAngle * 0.5f, 0, 0) * transform.forward;
        Vector3 downBoundary = Quaternion.Euler(verticalDetectionAngle * 0.5f, 0, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + upBoundary * detectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + downBoundary * detectionDistance);
    }*/

    public (GameObject, float) ClosestObject(GameObject[] objects){
        float distance = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject obj in objects) {
            float d = Vector3.Distance(obj.transform.position, gameObject.transform.position);
            if (d < distance){
                closest = obj;
                distance = d;
            }
        }
        return (closest, distance);
    }

    /// <summary> Returns all objects in this array, that contain this component. It checks also children and parents. </summary>
    /// <param name="objects"> Array of GameObjects to check </param>
    /// <param name="type"> Type to find </param>
    /// it checks first all children and then the parents. </param>
    /// <returns> A list of all GameObjects with this component. If a child or parent had the component it returns the child with this component. </returns>
    public static GameObject[] ObjectsWithComponent(GameObject[] objects, Type type){
        return ObjectsWithComponent(objects, type, true, true);
    }

    /// <summary> Returns all objects in this array, that contain this component. </summary>
    /// <param name="objects"> Array of GameObjects to check </param>
    /// <param name="type"> Type to find </param>
    /// <param name="children"> If true, it checks also the children of each GameObject if it contains this component. </param>
    /// <param name="parents"> If true, it checks also the parents of each GameObject if it contains this component. If parents and children is true, 
    /// it checks first all children and then the parents. </param>
    /// <returns> A list of all GameObjects with this component. If a child or parent had the component it returns the child with this component. </returns>
    public static GameObject[] ObjectsWithComponent(GameObject[] objects, Type type, bool children, bool parents){
        List<GameObject> confirmedObjects = new();
        foreach (GameObject obj in objects) {
            if (children){
                GameObject child = obj.GetComponentInChildren(type)?.gameObject;
                if (child != null) {
                    confirmedObjects.Add(child);
                    continue;
                }
            } else {
                if (obj.GetComponent(type) != null){
                    confirmedObjects.Add(obj);
                    continue;
                }
            }
            if (parents){
                GameObject p = Parent.FindParent(obj, type)?.gameObject;
                if (p != null){
                    confirmedObjects.Add(p);
                }
            }
        }
        return confirmedObjects.ToArray();
    }
}
