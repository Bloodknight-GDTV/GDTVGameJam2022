using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{

    PlayerMovement player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger triggered by {other};");

        ReloadScene();
    }

    private void ReloadScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        //GetComponent<Movement>().enabled = true;
    }

    // private void LoadNextScene()
    // {
    //     int currentScene = SceneManager.GetActiveScene().buildIndex;
    //     int nextScene = ++currentScene;
    //     if (nextScene == SceneManager.sceneCountInBuildSettings)
    //     {
    //         nextScene = 0;
    //     }
    //     SceneManager.LoadScene(nextScene);
    //     //GetComponent<Movement>().enabled = true;
    // }
    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
