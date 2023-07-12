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
    
    public void UpdateScore()
    {
        GlobalAchievement achGlobal = FindObjectOfType<GlobalAchievement>();
        achUnlocked.SetActive(true);
        for (int i = 0; i <= 9; i++)
        {
            if (achGlobal.achEvent[i])
                achImg[i].SetActive(true); 
        }

        // Give flat 125 point by ennemy killed
        // Give increasing number of point for each ennemy healed sum(i = 1 to saved) { 50 + sum(j =1 to i){ i + j / 6}}
        var saved = achGlobal.saved;
        var killed = achGlobal.killed;

        float point = killed * 125;
        for (int i = 1; i <= saved; i++)
        {
            point += 50;
            for (int j = 1; j <= i; j++)
            {
                point += ((i + j) / 5);
            }
        }

        point = Mathf.Ceil(point);
        TimePlayed.GetComponent<TextMeshProUGUI>().text = timeToString(achGlobal.timePlayed);
        Kill.GetComponent<TextMeshProUGUI>().text = killed.ToString();
        Save.GetComponent<TextMeshProUGUI>().text = saved.ToString();
        Grappling.GetComponent<TextMeshProUGUI>().text = achGlobal.nb_grappling.ToString();
        Points.GetComponent<TextMeshProUGUI>().text = point.ToString();
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
