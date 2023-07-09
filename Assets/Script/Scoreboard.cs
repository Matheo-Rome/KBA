using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{

    // Achievements images
    public List<GameObject> achImg;

    // Stats Variable
    public GameObject TimePlayed;
    public GameObject Kill;
    public GameObject Save;
    public GameObject Grappling;
    public GameObject Points;
    public GameObject achUnlocked;

    // Update is called once per frame
    void Update()
    {
        GlobalAchievement achGlobal = FindObjectOfType<GlobalAchievement>();
        achUnlocked.SetActive(true);
        for (int i = 0; i <= 9; i++)
        {
            if (achGlobal.achEvent[i])
            {
                achImg[i].SetActive(true);
                Debug.Log("hellooo");
            }
                
        }
        TimePlayed.GetComponent<TextMeshProUGUI>().text = "0";
        Kill.GetComponent<TextMeshProUGUI>().text = achGlobal.killed.ToString();
        Save.GetComponent<TextMeshProUGUI>().text = achGlobal.saved.ToString();
        Grappling.GetComponent<TextMeshProUGUI>().text = achGlobal.nb_grappling.ToString();
        Points.GetComponent<TextMeshProUGUI>().text = "0";
    }
}
