using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
                achImg[i].SetActive(true); 
        }

        
        TimePlayed.GetComponent<TextMeshProUGUI>().text = timeToString(achGlobal.timePlayed);
        Kill.GetComponent<TextMeshProUGUI>().text = achGlobal.killed.ToString();
        Save.GetComponent<TextMeshProUGUI>().text = achGlobal.saved.ToString();
        Grappling.GetComponent<TextMeshProUGUI>().text = achGlobal.nb_grappling.ToString();
        Points.GetComponent<TextMeshProUGUI>().text = "0";
    }

    string timeToString(float n)
    {
        /*n = n % (24f * 3600f);
        float hour = n / 3600f;
        */

        n %= 3600f;
        float minutes = n / 60f ;
      
        n %= 60f;
        float seconds = n;
          
        return /*Convert.ToInt32(hour) + " hours " +*/ Convert.ToInt32(minutes) + " min " + Convert.ToInt32(seconds) + " sec";
    }
}
