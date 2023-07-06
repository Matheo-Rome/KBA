using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 30f;
    public float corruption = 50f;
    [SerializeField] private Renderer ren;
    public GameObject bunny;
    GlobalAchievement ach;

    void Start()
    {
        ren.material.color = Color.black;
        ach = FindObjectOfType<GlobalAchievement>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        switch (health)
        {
            case <= 0f:
                Die();
                ach.killed++;
                break;
            case <= 10f:
                ren.material.color = Color.magenta;
                break;
            case <= 20f:
                ren.material.color = Color.red;
                break;    
            default:
                ren.material.color = Color.black;
                break;
        }
    }

    public void TakeHealing(float amount)
    {
        corruption -= amount;
        switch (corruption)
        {
            case <= 0f:
                Die();
                Instantiate(bunny, transform.position, transform.rotation);
                ach.saved++;
                break;
            case <= 10f:
                ren.material.color = Color.cyan;
                break;
            case <= 20f:
                ren.material.color = Color.blue;
                break;
            default:
                ren.material.color = Color.black;
                break;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
