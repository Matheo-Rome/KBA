using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDead : MonoBehaviour
{
    [SerializeField] private GameObject sp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.transform.position = sp.transform.position;
        //Destroy(collision.gameObject);
    }
}
