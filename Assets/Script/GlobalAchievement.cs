using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalAchievement : MonoBehaviour
{
    // Panel Variable
    public GameObject achNote;
    public AudioSource achSound;
    public GameObject achTitle;
    public GameObject achDesc;
    public List<bool> achEvent;
    List<bool> achUnlocked;

    // Stats Variable
    public int killed = 0;
    public int saved = 0;
    public int nb_grappling = 0;
    public bool hundredMeter = false;
    public float timePlayed = 0f;

    // Achievements images
    public List<GameObject> achImg;

    void Start()
    {
        achUnlocked = new List<bool>();
        achEvent = new List<bool>();
        for (int i = 0; i <= 9; i++)
        {
            achUnlocked.Add(false);
            achEvent.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEvents();
        if (achEvent[0] && !achUnlocked[0])
            StartCoroutine(TriggerAch(0, "First blood", "Kill a enemy"));
        if (achEvent[1] && !achUnlocked[1])
            StartCoroutine(TriggerAch(1, "It was a bunny?", "Save a enemy"));
        if (achEvent[2] && !achUnlocked[2])
            StartCoroutine(TriggerAch(2, "On fire", "Kill 10 enemies"));
        if (achEvent[3] && !achUnlocked[3])
            StartCoroutine(TriggerAch(3, "Savior", "Save 10 enemies"));
        if (achEvent[4] && !achUnlocked[4])
            StartCoroutine(TriggerAch(4, "Genocide", "Kill 40 enemies"));
        if (achEvent[5] && !achUnlocked[5])
            StartCoroutine(TriggerAch(5, "Pacifist", "Save 40 enemies"));
        if (achEvent[6] && !achUnlocked[6])       
            StartCoroutine(TriggerAch(6, "Wassup gamer", "Play 5 minutes"));
        if (achEvent[7] && !achUnlocked[7])       
            StartCoroutine(TriggerAch(7, "Widowmaker", "Grappling"));
        if (achEvent[8] && !achUnlocked[8])       
            StartCoroutine(TriggerAch(8, "Attack on titan", "Grappling 50 times"));
        if (achEvent[9] && !achUnlocked[9])       
            StartCoroutine(TriggerAch(9, "High enough", "Fly at 100m"));
    }

    void UpdateEvents()
    {
        if (killed != 0)
            achEvent[0] = true;
        if (saved != 0)
            achEvent[1] = true;
        if (killed >= 10)
            achEvent[2] = true;
        if (saved >= 10)
            achEvent[3] = true;
        if (killed >= 40)
            achEvent[4] = true;
        if (saved >= 40)
            achEvent[5] = true;
        if (Time.time >= 300f)
            achEvent[6] = true;
        if (nb_grappling != 0)
            achEvent[7] = true;
        if (nb_grappling >= 50)
            achEvent[8] = true;
        if (hundredMeter)
            achEvent[9] = true;
    }

    void ResetPanel()
    {
        achNote.SetActive(false);
        achTitle.GetComponent<TextMeshProUGUI>().text = "";
        achDesc.GetComponent<TextMeshProUGUI>().text = "";
    }
    
    IEnumerator TriggerAch(int index, string Title, string Desc)
    {
        achUnlocked[index] = true;
        achSound.Play();
        achImg[index].SetActive(true);
        achTitle.GetComponent<TextMeshProUGUI>().text = Title;
        achDesc.GetComponent<TextMeshProUGUI>().text = Desc;
        achNote.SetActive(true);
        yield return new WaitForSeconds(7);
        // Resetting UI
        ResetPanel();
        achImg[index].SetActive(false);
    }
}