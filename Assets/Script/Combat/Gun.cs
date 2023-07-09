using System;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float heal = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float bulletSpeed = 5f;
    private Color HealColor;
    [SerializeField] private Color DmgColor;
    [SerializeField] private Material DmgMat;
    [SerializeField] private Material HealMat;
    [SerializeField] private GameObject AuraShere;
    [SerializeField] private GameObject HealPart;
    [SerializeField] private GameObject DmgPart;
    
    
    [Header("Bullet")]
    [SerializeField] private Rigidbody bulletDmg;
    [SerializeField] private Rigidbody bulletHeal;
    
    [SerializeField] private Transform player, gunPoint;
    [SerializeField] private MeshRenderer mr;

    [Header("Muzzle")]
    [SerializeField] private GameObject muzzleDmg;
    [SerializeField] private GameObject muzzleHeal;

    public Camera fpsCam;
    private bool canShoot = true;

    [Header("Sound")] 
    [SerializeField] private AudioSource dmgSound;
    [SerializeField] private AudioSource healSound;

    private void Start()
    {
        HealColor = mr.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            canShoot = false;
            Shoot();
        }  
        else if (Input.GetButtonDown("Fire3") && canShoot)
        {
            canShoot = false;
            Heal();
        }  
    }

    void Shoot()
    {
        mr.material.color  = DmgColor;
        dmgSound.Play();
        DmgPart.SetActive(true);
        HealPart.SetActive(false);
        AuraShere.GetComponent<MeshRenderer>().material = DmgMat;
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 direction = targetPoint - gunPoint.position;
        Rigidbody spawnedBullet = Instantiate(bulletDmg, gunPoint.transform.position,Quaternion.identity);

        GameObject spawnedMuzzle = Instantiate(muzzleDmg, gunPoint.transform.position, Quaternion.identity);

        spawnedMuzzle.transform.forward = direction.normalized;
        spawnedBullet.transform.forward = direction.normalized;
        spawnedBullet.gameObject.GetComponent<Bullet>().damage = damage;
        spawnedBullet.useGravity = false;
        
        spawnedBullet.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        Destroy(spawnedBullet.gameObject, 2);
        Invoke("ResetShot", 0.25f);
        Destroy(spawnedMuzzle, 0.5f);
    }

    void Heal()
    {
        mr.material.color = HealColor;
        DmgPart.SetActive(false);
        HealPart.SetActive(true);
        AuraShere.GetComponent<MeshRenderer>().material = HealMat;
        healSound.Play();
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 direction = targetPoint - gunPoint.position;
        Rigidbody spawnedBullet = Instantiate(bulletHeal, gunPoint.transform.position,Quaternion.identity);

        GameObject spawnedMuzzle = Instantiate(muzzleHeal, gunPoint.transform.position, Quaternion.identity);

        spawnedMuzzle.transform.forward = direction.normalized;
        spawnedBullet.transform.forward = direction.normalized;
        spawnedBullet.gameObject.GetComponent<Bullet>().heal = heal;
        spawnedBullet.useGravity = false;
        
        spawnedBullet.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        Destroy(spawnedBullet.gameObject, 2);
        Invoke("ResetShot", 0.25f);
        Destroy(spawnedMuzzle, 0.5f);
    }

    private void ResetShot()
    {
        canShoot = true;
    }
}
