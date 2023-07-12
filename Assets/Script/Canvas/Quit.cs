using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Quit : MonoBehaviour
{

    [SerializeField] private int MainMenu = 0;
    
    public void QuitGame()
    {
           SceneManager.LoadScene(MainMenu);
    }

}
