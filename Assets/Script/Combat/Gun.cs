using System;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private Transform player, gunPoint;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float upwardForce;

    [SerializeField] private GameObject muzzle;

    public Camera fpsCam;
    private bool canShoot = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            canShoot = false;
            Shoot();
        }  
        else if (Input.GetButtonDown("Fire3"))
        {
            Heal();
        }  
    }

    void Shoot()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 direction = targetPoint - gunPoint.position;
        Rigidbody spawnedBullet = Instantiate(bullet, gunPoint.transform.position,Quaternion.identity);

        GameObject spawnedMuzzle = Instantiate(muzzle, gunPoint.transform.position, Quaternion.identity);

        spawnedMuzzle.transform.forward = direction.normalized;
        spawnedBullet.transform.forward = direction.normalized;
        spawnedBullet.gameObject.GetComponent<Bullet>().damage = damage;
        spawnedBullet.useGravity = false;
        
        spawnedBullet.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        Destroy(spawnedBullet.gameObject, 2);
        Invoke("ResetShot", 0.5f);
        Destroy(spawnedMuzzle, 0.5f);
        /*RaycastHit hit;
        bool res = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);
        if (res)
        {
            Debug.Log(hit.transform.name);  
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }*/
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

    private void ResetShot()
    {
        canShoot = true;
    }
}
