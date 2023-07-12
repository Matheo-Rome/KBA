using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float heal = 10f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private Color HealColor;
    [SerializeField] private Color DmgColor;
    [SerializeField] private Material DmgMat;
    [SerializeField] private Material HealMat;
    [SerializeField] private GameObject AuraShere;
    [SerializeField] private GameObject HealPart;
    [SerializeField] private GameObject DmgPart;
    [SerializeField] private Swing swing;
    
    
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
        // Change the aspect of the staff to "Dmg"
        mr.material.color  = DmgColor;
        dmgSound.Play();
        DmgPart.SetActive(true);
        HealPart.SetActive(false);
        AuraShere.GetComponent<MeshRenderer>().material = DmgMat;

        LaunchBullet(bulletDmg, muzzleDmg);
    }

    void Heal()
    {
        // Change the aspect of the staff to "Heal'
        mr.material.color = HealColor;
        DmgPart.SetActive(false);
        HealPart.SetActive(true);
        AuraShere.GetComponent<MeshRenderer>().material = HealMat;
        
        
        healSound.Play();
        LaunchBullet(bulletHeal, muzzleHeal);
    }

    private void LaunchBullet(Rigidbody bullet, GameObject muzzle)
    {
        swing.fuel -= swing.maxfuel * (10 / 100f);
        swing.DecrementFuel();
        //Get the object in sight or a far away point
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        //Instantiate a bullet from the tip of the staff to target and a muzzle
        Vector3 direction = targetPoint - gunPoint.position;
        Rigidbody spawnedBullet = Instantiate(bullet, gunPoint.transform.position,Quaternion.identity);
        GameObject spawnedMuzzle = Instantiate(muzzle, gunPoint.transform.position, Quaternion.identity);
        
        spawnedBullet.transform.forward = direction.normalized;
        var b = spawnedBullet.gameObject.GetComponent<Bullet>();
        b.heal = heal;
        b.damage = damage;

        //So the bullet flies straight
        spawnedBullet.useGravity = false;
        
        spawnedBullet.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        //Destroy the bullet if not hit anything in 2 sec
        Destroy(spawnedBullet.gameObject, 2);
        //Can shoot again in 0.25 sec
        Invoke("ResetShot", 0.25f);
        Destroy(spawnedMuzzle, 0.5f);
    }

    private void ResetShot()
    {
        canShoot = true;
    }
}
