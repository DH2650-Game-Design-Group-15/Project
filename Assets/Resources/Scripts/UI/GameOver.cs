using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    private Inputs inputs;

    private void Start () {
        inputs = FindObjectOfType<Inputs>();
        inputs.ChangeActionMap("GameOver");
    }

    public void PlayAgain()
    {
        inputs.ChangeActionMap("Player");
        SceneManager.LoadScene(2);
    }
}
