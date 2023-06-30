using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 30f;
    public float corruption = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    public void TakeHealing(float amount)
    {
        corruption -= amount;
        if (corruption <= 0f)
        {
            Debug.Log("Saved !");
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
