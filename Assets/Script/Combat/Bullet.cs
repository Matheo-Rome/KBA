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
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
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
