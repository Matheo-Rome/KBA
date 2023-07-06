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
    public bool achActive = false;
    public GameObject achTitle;
    public GameObject achDesc;
    public List<bool> achEvent;
    List<bool> achUnlocked;

    // Stats Variable
    public int killed = 0;
    public int saved = 0;

    // Achievements images
    public GameObject ach00Img;
    
    void Start()
    {
        achUnlocked = new List<bool>();
        achEvent = new List<bool>();
        for (int i = 0; i < 1; i++)
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
        {
            StartCoroutine(Trigger00Ach());
        }
    }

    void UpdateEvents()
    {
        if (killed != 0)
            achEvent[0] = true;
    }

    void ResetPanel()
    {
        achNote.SetActive(false);
        achTitle.GetComponent<TextMeshProUGUI>().text = "";
        achDesc.GetComponent<TextMeshProUGUI>().text = "";
        achActive = false;
    }

    IEnumerator Trigger00Ach()
    {
        achActive = true;
        achUnlocked[0] = true;
        //PlayerPrefs.SetInt("Ach01", ach01Code);
        achSound.Play();
        ach00Img.SetActive(true);
        achTitle.GetComponent<TextMeshProUGUI>().text = "First blood";
        achDesc.GetComponent<TextMeshProUGUI>().text = "Kill a enemy";
        achNote.SetActive(true);
        yield return new WaitForSeconds(7);
        // Resetting UI
        ResetPanel();
        //ach01Img.SetActive(false);
    }
}