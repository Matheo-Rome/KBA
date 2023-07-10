using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private int startSceneIndex = 1;

    public void startGame()
    {
        SceneManager.LoadScene(startSceneIndex);
    }
}
