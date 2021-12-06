using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public AudioSource ShotOne, bulletImpact, cannonShot;

    public void CharacterShooting()
    {
        ShotOne.Play();
    }
    public void BulletImpact()
    {
        bulletImpact.Play();
    }

    public void CannonShooting()
    {
        cannonShot.Play();
    }
}
