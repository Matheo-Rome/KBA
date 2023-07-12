using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Values")] [SerializeField] float lookRadius = 10f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float timeBetweenAttackes = 1f;

    [Header("Target")] private Transform target;

    [Header("Attack")] [SerializeField] private GameObject slash;
    [SerializeField] private LayerMask whatIsAttackable;

    [Header("Navmesh")] private NavMeshAgent agent;
    public bool following = false;
    public bool alreadyAttacked = false;

    [Header("Score")] [SerializeField] private GameObject scoreboard;
    [SerializeField] private Scoreboard sb;
    private GlobalAchievement ach;


    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        ach = FindObjectOfType<GlobalAchievement>();
    }

    void Update()
    {
        //try to go toward the target (the player) but only if in sight range (else start wandering see "WanderingHostile.cs")
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            following = true;
        }
        else
            following = false;

        if (following)
        {
            //if the player is in sight range check if he is in attack range
            if (Physics.CheckSphere(transform.position, attackRange, whatIsAttackable))
            {
                //Launch an attack and instantiate effect;
                agent.SetDestination(target.position);
                transform.LookAt(target);
                if (!alreadyAttacked)
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

                    // Create attack animation (usless but cool if we had more life to the player)
                    var createdSlash = Instantiate(slash, transform.position, Quaternion.identity);
                    createdSlash.transform.forward = transform.forward;
                    alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttackes);
                    Destroy(this);
                }

            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
