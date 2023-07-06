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
    List<bool> achEvent;
    List<bool> achUnlocked;

    // Stats Variable
    public int killed = 0;
    public int saved = 0;

    // Achievements images
    public List<GameObject> achImg;

    void Start()
    {
        achUnlocked = new List<bool>();
        achEvent = new List<bool>();
        for (int i = 0; i <= 1; i++)
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
    }

    void UpdateEvents()
    {
        if (killed != 0)
            achEvent[0] = true;
        if (saved != 0)
            achEvent[1] = true;
    }

    void ResetPanel()
    {
        achNote.SetActive(false);
        achTitle.GetComponent<TextMeshProUGUI>().text = "";
        achDesc.GetComponent<TextMeshProUGUI>().text = "";
        achActive = false;
    }
    
    IEnumerator TriggerAch(int index, string Title, string Desc)
    {
        achActive = true;
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