using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftOff : MonoBehaviour {

    public int liftSpeed;
    public int liftTime;
    public int exitSpeed;
    public float totalTime;
    public float endTime;
    public new Camera camera;
    public GameObject userInterface;
    private float time;

    private void Start() {
        userInterface.SetActive(false);
    }

    private void Update () {
        time += Time.deltaTime;
        if (time < liftTime) {
            transform.position = new Vector3(transform.position.x, transform.position.y + liftSpeed * Time.deltaTime, transform.position.z);
            camera.transform.rotation = Quaternion.LookRotation(transform.position-camera.transform.position);
        } else if (time < totalTime) {
            transform.position = new Vector3(transform.position.x + exitSpeed * Time.deltaTime, transform.position.y + exitSpeed * Time.deltaTime, transform.position.z);
        } else if (time < endTime) {
            userInterface.SetActive(true);
        } else {
            SceneManager.LoadScene("MainMenu");
        }
    }

}