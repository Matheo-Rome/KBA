using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();  
    }

    // Update is called once per frame
    void Update()
    {
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
              if (Physics.CheckSphere(transform.position, attackRange, whatIsAttackable))
              {
                  agent.SetDestination(target.position);
                  transform.LookAt(target);
                  if (!alreadyAttacked)
                  {
                      Debug.Log("Oh no you taking damages !");
                      var createdSlash = Instantiate(slash, transform.position, Quaternion.identity);
                      createdSlash.transform.forward = transform.forward;
                      alreadyAttacked = true;
                      Invoke(nameof(ResetAttack), timeBetweenAttackes);
                  }

              }

              /*RaycastHit hit;
              if (Physics.SphereCast(gameObject.transform.position, attackRange, transform.forward, out hit, 15,
                  whatIsAttackable))
              {
                  Debug.Log("Oh no you taking damages !");
                  var createdSlash = Instantiate(slash, transform.position, Quaternion.identity);
                  createdSlash.transform.forward = hit.transform.forward;
              }*/
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
