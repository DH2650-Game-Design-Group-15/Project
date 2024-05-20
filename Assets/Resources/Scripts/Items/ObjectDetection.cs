using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    public float detectionAngle = 45f;
    public float detectionDistance = 10f;
    public LayerMask obstacleMask;

    public GameObject[] DetectObjects()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionDistance);
        List<GameObject> objects = new();

        foreach (Collider obj in detectedObjects) {
            if (obj.gameObject != gameObject) {
                Vector3 directionToObject = obj.transform.position - transform.position;
                float angleToObject = Vector3.Angle(transform.forward, directionToObject);

                if (angleToObject <= detectionAngle * 0.5f)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);

        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle * 0.5f, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionDistance);
    }

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
