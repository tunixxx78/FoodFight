using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAi : MonoBehaviour
{
    public GameObject targetLocation;
    public GameObject ammoSpawn;
    public GameObject ammo;
    public GameObject gunRotator;
    public float force;
    public Vector3 gravity;

    private int angleMultiplier; // Tämä on 1 jos ammutaan positiivisen suuntaan(z) ja -1 jos ammutaan negatiiviseen suuntaan(z).


    // Start is called before the first frame update
    void Start()
    {
        gravity = Physics.gravity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAt = new Vector3(targetLocation.transform.position.x, gameObject.transform.position.y, targetLocation.transform.position.z);
        gameObject.transform.LookAt(lookAt);
    }

    public void Shoot()
    {
        //gunRotator.GetComponent<RotateGun>().xAngle = -83;
        StartCoroutine(ShootBalls());
    }


    IEnumerator ShootBalls()
    {
        // annetaan pieni randomisaation sijaintiin

        Vector3 randomTarget = new Vector3(targetLocation.transform.position.x + Random.Range(-6, 6), targetLocation.transform.position.y, targetLocation.transform.position.z + Random.Range(-6, 6));

        Debug.Log("Ammutaan ammus");
        //Vector3[] direction = hitTargetBySpeed(ammoSpawn.transform.position, targetLocation.transform.position, gravity, force); // suora osuma
        Vector3[] direction = hitTargetBySpeed(ammoSpawn.transform.position, randomTarget, gravity, force); //randomilla

        if (gameObject.transform.position.z < targetLocation.transform.position.z)
        {
            angleMultiplier = -1;
        }
        else
        {
            angleMultiplier = 1;
        }

        Debug.Log("Tykin tulisi kääntyä kulmaan: " + Mathf.Atan(direction[0].y / direction[0].z) * Mathf.Rad2Deg * angleMultiplier);
        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[0].y / direction[0].z) * Mathf.Rad2Deg * angleMultiplier;

        // Käsketään coroutinea odottamaan niin kauan kun kääntyminen on käynnissä ja ammu heti sen jälkeen
        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);

        // Eka ammus
        GameObject projectile = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(direction[0], ForceMode.Impulse);

        yield return new WaitForSeconds(2);


        Debug.Log("Tykin tulisi kääntyä kulmaan: " + Mathf.Atan(direction[1].y / direction[1].z) * Mathf.Rad2Deg * angleMultiplier);
        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[1].y / direction[1].z) * Mathf.Rad2Deg * angleMultiplier;

        // Käsketään coroutinea odottamaan niin kauan kun kääntyminen on käynnissä ja ammu heti sen jälkeen
        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);

        // Toka ammus
        GameObject projectile2 = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        projectile2.GetComponent<Rigidbody>().AddRelativeForce(direction[1], ForceMode.Impulse);



    }

    public Vector3[] hitTargetBySpeed(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float launchSpeed)
    {
        Vector3 AtoB = targetPosition - startPosition;
        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase, startPosition);
        float horizontalDistance = horizontal.magnitude;

        Vector3 vertical = GetVerticalVector(AtoB, gravityBase, startPosition);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase));

        float x2 = horizontalDistance * horizontalDistance;
        float v2 = launchSpeed * launchSpeed;
        float v4 = launchSpeed * launchSpeed * launchSpeed * launchSpeed;
        float gravMag = gravityBase.magnitude;

        // LaunchTest

        float launchTest = v4 - (gravMag * ((gravMag * x2) + (2 * verticalDistance)));
        Debug.Log("LAUNCHTEST" + launchTest);

        Vector3[] launch = new Vector3[2];

        if (launchTest < 0)
        {
            Debug.Log("ei voi osua maaliin");
            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad)) - gravity.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad);
            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad)) - (gravity.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad));
        }
        else
        {
            Debug.Log("Voidaan osua maaliin! ");
            float[] tanAngle = new float[2];
            tanAngle[0] = (v2 - Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);
            tanAngle[1] = (v2 + Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);

            // kulmat

            float[] finalAngle = new float[2];
            finalAngle[0] = Mathf.Atan(tanAngle[0]);
            finalAngle[1] = Mathf.Atan(tanAngle[1]);

            Debug.Log("Kulmat:" + finalAngle[0] * Mathf.Rad2Deg + "ja" + finalAngle[1] * Mathf.Rad2Deg);

            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[0])) - (gravity.normalized * launchSpeed * Mathf.Sin(finalAngle[0]));
            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[1])) - (gravity.normalized * launchSpeed * Mathf.Sin(finalAngle[1]));
        }

        return launch;
    }

    public Vector3 GetHorizontalVector(Vector3 AtoB, Vector3 gravityBase, Vector3 startPos)
    {
        Vector3 output;
        Vector3 perpendicular = Vector3.Cross(AtoB, gravityBase);
        perpendicular = Vector3.Cross(gravityBase, perpendicular);
        output = Vector3.Project(AtoB, perpendicular);
        Debug.DrawRay(startPos, output, Color.green, 10f);

        return output;
    }

    public Vector3 GetVerticalVector(Vector3 AtoB, Vector3 gravityBase, Vector3 startPos)
    {
        Vector3 output;
        output = Vector3.Project(AtoB, gravity);
        Debug.DrawRay(startPos, output, Color.cyan, 10f);

        return output;
    }
}
