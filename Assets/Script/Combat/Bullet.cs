using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float heal;
    public bool Dmg = true;
    private bool alreaydHit = false;

    [SerializeField] private GameObject impact;

    
    //If a collider which is not a player is hit make an impact effet and if the collider has a Target component remove
    //health or corruption depending on the bullet 
    private void OnTriggerEnter(Collider other)
    {
        //To avoid hitting multiple time the entity or getting stuck in player collider
        if (other.gameObject.tag == "Player" || alreaydHit)
            return;
        alreaydHit = true;
        var createdImpact = Instantiate(impact, transform.position, Quaternion.identity);
        var target = other.gameObject.GetComponent<Target>();
        if (target)
            if (Dmg)
                target.TakeDamage(damage);
            else
                target.TakeHealing(heal);
                 

    Destroy(gameObject);
    }
}
