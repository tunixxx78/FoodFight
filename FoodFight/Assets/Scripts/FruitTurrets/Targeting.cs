using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject Host, host2, host3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Host.GetComponent<TurretAi>().Shoot();
            host2.GetComponent<TurretAi>().Shoot();
            host3.GetComponent<TurretAi>().Shoot();

            Invoke("SecondWave", 1f);
            
        }

    }
    void SecondWave()
    {
        Host.GetComponent<TurretAi>().Shoot();
        host2.GetComponent<TurretAi>().Shoot();
        host3.GetComponent<TurretAi>().Shoot();

    }
   
}
