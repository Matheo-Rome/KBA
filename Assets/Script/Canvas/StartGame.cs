using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private int startSceneIndex = 1;
    [SerializeField] private int MainMenu = 0;

    public void startGame()
    {
        SceneManager.LoadScene(startSceneIndex);
        AudioListener.pause = false;
        Cursor.visible = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene(MainMenu);
    }
}
