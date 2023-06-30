using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }  
        else if (Input.GetButtonDown("Fire3"))
        {
            Heal();
        }  
    }

    void Shoot()
    {
        RaycastHit hit;
        bool res = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);
        if (res)
        {
            Debug.Log(hit.transform.name);  
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    void Heal()
    {
        RaycastHit hit;
        bool res = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);
        if (res)
        {
            Debug.Log(hit.transform.name);  
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeHealing(damage);
            }
        }
    }
}
