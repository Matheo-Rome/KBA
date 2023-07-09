using MimicSpace;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 30f;
    public float corruption = 50f;
    [SerializeField] private Renderer ren;
    public GameObject bunny;
    GlobalAchievement ach;
    private bool spawnedB = false;

    void Start()
    {
        ren.material.color = Color.black;
        ach = FindObjectOfType<GlobalAchievement>();
    }

    private void changeLegColor(Color color)
    {
         var legs = gameObject.GetComponentsInChildren<Leg>();
         foreach (var leg in legs)
         {
             leg.gameObject.GetComponent<LineRenderer>().material.color = color;
         }
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
                changeLegColor(Color.magenta);
                break;
            case <= 20f:
                ren.material.color = Color.red;
                changeLegColor(Color.red);
                break;    
            default:
                ren.material.color = Color.black;
                changeLegColor(Color.black);
                break;
        }
    }

    public void TakeHealing(float amount)
    {
        corruption -= amount;
        switch (corruption)
        {
            case <= 0f:
                if (!spawnedB)
                {
                    spawnedB = true;
                    Instantiate(bunny, transform.position, transform.rotation);
                    ach.saved++;
                    Die();
                }

                break;
                case <= 10f:
                ren.material.color = Color.cyan;
                changeLegColor(Color.cyan);
                break;
            case <= 20f:
                ren.material.color = Color.blue;
                changeLegColor(Color.blue);
                break;
            default:
                ren.material.color = Color.black;
                changeLegColor(Color.black);
                break;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
