using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

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
        if (other.gameObject.tag == "Player")
            return;
        var createdImpact = Instantiate(impact, other.transform.position, Quaternion.identity);
        var target = other.gameObject.GetComponent<Target>();
        if(target) 
            target.TakeDamage(damage);
        Destroy(gameObject);
        Destroy(createdImpact, 0.5f);
    }
}
