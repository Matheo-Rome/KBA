using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private int startSceneIndex = 1;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject HelpMenu;
    [SerializeField] private GameObject Controles;
    [SerializeField] private GameObject Goal;
    [SerializeField] private GameObject Bc;
    [SerializeField] private GameObject Bce;
    public void startGame()
    {
        SceneManager.LoadScene(startSceneIndex);
        AudioListener.pause = false;
        Cursor.visible = false;
    }
    
    public void quitGame()
    {
        Application.Quit();
    }

    public void help()
    {
        MainMenu.SetActive(false);
        Bc.SetActive(false);
        Bce.SetActive(true);
        HelpMenu.SetActive(true);
    }

    public void goBackHelp()
    {
        HelpMenu.SetActive(false);
        Bc.SetActive(true);
        Bce.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void control()
    {
        Controles.SetActive(true);
        HelpMenu.SetActive(false);
    }

    public void goBackControl()
    {
        Controles.SetActive(false);
        HelpMenu.SetActive(true);
    }

    public void goal()
    {
        Goal.SetActive(true);
        HelpMenu.SetActive(false);
    }

    public void goBackGoal()
    {
        HelpMenu.SetActive(true);
        Goal.SetActive(false);
    }
    
    
}
