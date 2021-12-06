using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12, gravityy = -9.81f, groundDistance = 0.4f, jump = 3f, damage = 10f, range = 100f;
    public Transform groundCheck, bulletSpawnPoint;
    public LayerMask groundMask;
    bool isGrounded;
    GameManager gameManager;
    public GameObject bullet;
    public float shootForce;

    public Camera mainCamera;

    Vector3 velocity;

    SFXManager sFXManager;
    [SerializeField] Animator playerAnimator;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        sFXManager = FindObjectOfType<SFXManager>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravityy);
            playerAnimator.SetTrigger("Jump");
            
            
        }
        

        velocity.y += gravityy * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

       

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            sFXManager.CharacterShooting();
        }

        if (Input.GetKey(KeyCode.W) && isGrounded || Input.GetKey(KeyCode.S) && isGrounded)
        {
            playerAnimator.SetBool("Run", true);
        }
        else { playerAnimator.SetBool("Run", false); }

        if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            playerAnimator.SetBool("RunLeft", true);
        }
        else { playerAnimator.SetBool("RunLeft", false); }

        if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            playerAnimator.SetBool("RunRight", true);
        }
        else { playerAnimator.SetBool("RunRight", false); }

        
    }

    void Shoot()
    {


        /* RaycastHit hit;
             if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
             {
             Debug.Log(hit.transform.name);

             Target target = hit.transform.GetComponent<Target>();
             if (target != null)
             {
                 target.TakeDamage(damage);
             }
         }*/

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else { targetPoint = ray.GetPoint(75); }

        Vector3 direction = targetPoint - bulletSpawnPoint.position;

        GameObject currentBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);

        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ammo"))
        {
            Debug.Log("OSUMA");
            playerAnimator.SetTrigger("Death");
            Invoke("KillPlayer", 3f);
        }
    }

    void KillPlayer()
    {
        gameManager.GameOver();
    }


}
