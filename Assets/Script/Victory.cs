using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [Header("Score")] 
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private Scoreboard sb;
    private GlobalAchievement ach;
    
    [Header("Target")] private Transform target;

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        ach = FindObjectOfType<GlobalAchievement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            scoreboard.SetActive(true);
            if (ach.timePlayed == 0f)
                ach.timePlayed = Time.timeSinceLevelLoad;
           sb.UpdateScore();
        
           //destroy some key component to stop moving a fire spells
           Destroy(target.gameObject.GetComponent<PlayerMovement>());
           Destroy(target.gameObject.GetComponentInChildren<Swing>(false));
           Destroy(target.gameObject.GetComponentInChildren<Gun>(false));
        
           //Stop all sound and show cursor
           AudioListener.pause = true;
           Cursor.visible = true;
           Cursor.lockState = CursorLockMode.None;
        }
    }
}
