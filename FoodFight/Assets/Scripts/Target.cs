using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 10f;
    [SerializeField] int scoreAmount = 1;

    private void Update()
    {
        if (transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
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
}
