using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBullet : MonoBehaviour
{
    SFXManager sFXManager;
    private void Awake()
    {
        Destroy(this.gameObject, 3f);
        sFXManager = FindObjectOfType<SFXManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            sFXManager.BulletImpact();
            Destroy(this.gameObject);
        }
        if (collision.collider.CompareTag("Ammo"))
        {
            sFXManager.BulletImpact();
            Destroy(this.gameObject);
        }
    }
}
