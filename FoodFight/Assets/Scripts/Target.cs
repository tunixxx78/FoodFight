using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 10f;
    [SerializeField] int scoreAmount = 1;

    private void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            ScoringSystem.theScore += scoreAmount;
            ScoringSystem.theHighScore += scoreAmount;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log("Osuma Palloon!");
            ScoringSystem.theScore += scoreAmount;
            ScoringSystem.theHighScore += scoreAmount;
            Destroy(this.gameObject);
        }
        if (collision.collider.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
