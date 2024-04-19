using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using UnityEditor.ShaderGraph;

public class AIVisionField : MonoBehaviour {

    public float viewRadius;
    public float angle;
    public float height;
    public float delayTime;
    public float meshResolution;
    public Color shapeColor;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    private bool playerIsVisible;
    private Vector3 playerPosition;
    Mesh viewMesh;

    private void Start () {
        StartCoroutine(FindPlayerDelay(delayTime));
    }

    private void LateUpdate () {
        viewMesh = CreateShapeMesh();
        GetComponent<MeshFilter>().mesh = viewMesh;
    }

    /* private void OnValidate () {
        
    } */

    private void OnDrawGizmos () {
        if (viewMesh) {
            Gizmos.color = shapeColor;
            Gizmos.DrawMesh(viewMesh, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// Searches for player with a delay
    /// </summary>
    /// <param name="delay">Time in seconds to wait</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator FindPlayerDelay (float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            TargetDetection();
        }
    }

    /// <summary>
    /// Creates a Vector3 which represents an given angle
    /// </summary>
    /// <param name="degrees">Angle in degrees</param>
    /// <param name="angleIsGlobal">Defines if the angle is local or global</param>
    /// <returns>Vector3 of direction</returns>
    private Vector3  DirFromAngle (float degrees, bool angleIsGlobal) {
        if(!angleIsGlobal) {
            degrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(degrees * Mathf.Deg2Rad), 0 , Mathf.Cos(degrees * Mathf.Deg2Rad));
    }

    /// <summary>
    /// Looks for targets inside the FOV
    /// </summary>
    private void TargetDetection () {
        SetIsPlayerVisible(false);
        Collider[] targetPlayer = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        shapeColor = new Color(0, 0, 1, 1);
        
        if (targetPlayer.Length > 0) {
            Vector3 playerPos = targetPlayer[0].transform.position;
            Vector3 dirToTarget = (playerPos - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < angle/2) {
                float distToTarget = Vector3.Distance(transform.position, playerPos);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) {
                    SetPlayerPosition(playerPos);
                    SetIsPlayerVisible(true);
                    shapeColor = new Color(0, 1, 0,1);
                }
            }
        }
    }

    /// <summary>
    /// Creates the mesh of the FOV
    /// </summary>
    /// <returns>Mesh of FOV</returns>
    private Mesh CreateShapeMesh () {
        Mesh viewMesh = new Mesh();
        int steps = Mathf.RoundToInt(angle * meshResolution);
        float stepAngleSize = angle / steps;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= steps; i++){
            float currAngle = transform.eulerAngles.y - angle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(currAngle);
            if (i==0){
                Debug.Log("Start");
            }
            Debug.Log("Point: " + newViewCast.point + "HitStatus: " + newViewCast.hit + "DIst: " + newViewCast.dist);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount * 2 + 1];
        int[] triangles = new int[(vertexCount*2+(vertexCount-1)*2)*3]; //vertexCount*4-2
        vertices[0] = Vector3.zero;

        for (int i = 0; i <  vertexCount - 1; i++) {
            vertices[i+1] = viewPoints[i] + Vector3.down * height;

            if (i < vertexCount - 2) {
                triangles[i*3] = 0;
                triangles[i*3+1] = i+2;
                triangles[i*3+2] = i+1;
            }
        }

        vertices[vertexCount] = Vector3.zero;

        for (int i = vertexCount; i < vertexCount * 2 - 1; i++) {
            vertices[i+1] = viewPoints[i % vertexCount] + Vector3.up * height;

            if (i < vertexCount * 2 - 2) {
                triangles[i*3] = 0;
                triangles[i*3+1] = i+1;
                triangles[i*3+2] = i+2;
            }
        }

        for (int i = 1; i < vertexCount; i++) {
            int triangleIndex = (vertexCount * 2 - 3) * 3;
            triangles[triangleIndex + i * 6] = i;
            triangles[triangleIndex + i * 6 + 1] = vertexCount + i + 1;
            triangles[triangleIndex + i * 6 + 2] = vertexCount + i;

            triangles[triangleIndex + i * 6 + 3] = i;
            triangles[triangleIndex + i * 6 + 4] = i+1;
            triangles[triangleIndex + i * 6 + 5] = vertexCount + i + 1;
        }
        
        triangles[triangles.Length - 6] = 0;
        triangles[triangles.Length - 5] = vertexCount * 2 - 1;
        triangles[triangles.Length - 4] = vertexCount - 1;
        
        triangles[triangles.Length - 3] = 0;
        triangles[triangles.Length - 2] = 1;
        triangles[triangles.Length - 1] = vertexCount+1;

        
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

        return viewMesh;
    }

    public ViewCastInfo ViewCast (float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) {
            return new ViewCastInfo(true, hit.distance, globalAngle, transform.parent.gameObject.transform.position + transform.InverseTransformPoint(hit.point));
        } else {
            return new ViewCastInfo(false, viewRadius, globalAngle, dir * viewRadius);
        }
    }

    public struct ViewCastInfo {
        public bool hit;
        public float dist;
        public float deg;
        public Vector3 point;

        public ViewCastInfo(bool _hit, float _dist, float _deg, Vector3 _point) {
            hit = _hit;
            dist = _dist;
            deg = _deg;
            point = _point;
        }
    }

    private void SetPlayerPosition (Vector3 pos) {
        playerPosition = pos;
    }

    public Vector3 GetPlayerPosition () {
        return playerPosition;
    }

    private void SetIsPlayerVisible (bool visible) {
        playerIsVisible = visible;
    }

    public bool GetIsPlayerVisible () {
        return playerIsVisible;
    }

}