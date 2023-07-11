using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float timeBetweenAttackes = 1f;

    public Transform target;

    [SerializeField] private GameObject slash;
    [SerializeField] private LayerMask whatIsAttackable;

    NavMeshAgent agent;
    public bool following = false;
    private bool alreadyAttacked = false;

    public GameObject scoreboard;
    GlobalAchievement ach;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>(); 
        ach = FindObjectOfType<GlobalAchievement>();
    }

    // Update is called once per frame
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
        {
            following = false;
        }

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
                      Debug.Log("Oh no you taking damages !");
                      scoreboard.SetActive(true);
                      Destroy(target.gameObject.GetComponent<PlayerMovement>());
                      Destroy(target.gameObject.GetComponentInChildren<Swing>(false));
                      Destroy(target.gameObject.GetComponentInChildren<Gun>(false));
                      AudioListener.pause = true;
                      Cursor.visible = true;
                      Cursor.lockState = CursorLockMode.None;
                      if (ach.timePlayed == 0f)
                        ach.timePlayed = Time.time;
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
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var createdSlash = Instantiate(slash, other.transform.position, Quaternion.identity);
            createdSlash.transform.forward = gameObject.transform.forward;
            Debug.Log("Oh no you taking damages !");
        }
    }*/
}
