using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12, gravityy = -9.81f, groundDistance = 0.4f, jump = 3f, damage = 10f, range = 100f, turnSmoothTime = 0.1f;
    public Transform groundCheck, bulletSpawnPoint;
    public LayerMask groundMask;
    bool isGrounded;
    GameManager gameManager;
    public GameObject bullet;
    public float shootForce;
    float turnSmoothVelocity;

    public Camera mainCamera;
    public Transform cam;

    [SerializeField] GameObject virtual1, virtual2;
    Vector3 velocity;

    SFXManager sFXManager;
    [SerializeField] Animator playerAnimator;

    [SerializeField] CinemachineVirtualCamera vcam0, vcam1, vcam2;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        sFXManager = FindObjectOfType<SFXManager>();
        vcam0.Priority = 2;
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
        

       /* float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
       */
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravityy);
            playerAnimator.SetTrigger("Jump");
            
            
        }
        

        velocity.y += gravityy * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(isGrounded == false)
        {
            playerAnimator.SetBool("InAir", true);
        }
        else { playerAnimator.SetBool("InAir", false); }
       

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            sFXManager.CharacterShooting();
        }

        if (Input.GetKey(KeyCode.W) && isGrounded || Input.GetKey(KeyCode.S) && isGrounded)
        {
            vcam0.Priority = 2;
            playerAnimator.SetBool("Run", true);
        }
        else { playerAnimator.SetBool("Run", false); vcam0.Priority = 1;}


        if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            vcam1.Priority = 2;
            playerAnimator.SetBool("RunLeft", true);
        }
        else { playerAnimator.SetBool("RunLeft", false); vcam1.Priority = 1; }

        if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            vcam2.Priority = 2;
            playerAnimator.SetBool("RunRight", true);
        }
        else { playerAnimator.SetBool("RunRight", false); vcam2.Priority = 1; }

        
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

        //Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Faling")
        {
            Debug.Log("OSUMA TRIGGERIIN");
            
        }
    }


}
