using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    

    [Header("Visuals")]
    public GameObject model;
    public float rotatingSmoothTime = 0.1f;
    float rotatingSmoothSpeed;

    [Header("Movement")]
    public float mass = 3.0f;
    public float movingVelocity = 10;
    public float knockbackForce;
    public Vector3 jumpingVelocity;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;
    public float maxDistToGround = 0.1f;

    [Header("Equipment")]
    public Sword sword;

    public int health = 5;
    public int bombAmmount = 5;
    public float throwingSpeed;
    public GameObject bombPrefab;
    public Bow bow;
    private Rigidbody playerRigidbody;
    public bool canJump;
    private Quaternion targetModelRoatation;
    private float knockbackTimer;
    private Vector3 impact = Vector3.zero;
    

    // Start is called before the first frame update
    private void Start()
    {
        bow.gameObject.SetActive(false);
        playerRigidbody = GetComponent<Rigidbody>();
        targetModelRoatation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        //Apply Impact Forces
        if (impact.magnitude > 0.2f) controller.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

       
        
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        else ProcessInput();
    }

    private void ProcessInput()
    {

        // Move in the XZ plane.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotatingSmoothSpeed, rotatingSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * movingVelocity * Time.deltaTime);

        }

        //jump up
        
        if (canJump && jumpingVelocity.y < -1000) jumpingVelocity.y = 0;
        if (Input.GetButtonDown("Jump"))
        {
            // Raycast to identify if the player can jump
            int layerMask = 1 << 8;
            //layerMask = ~layerMask;
            RaycastHit hit;

            Physics.Raycast(transform.position, Vector3.down, out hit, maxDistToGround, layerMask);
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
            if (hit.collider != null)
            {
                jumpingVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        }
        jumpingVelocity.y += gravity * Time.deltaTime;
        controller.Move(jumpingVelocity * Time.deltaTime);

        //Check equipment interaction
        if (Input.GetKeyDown("z"))
        {
            bow.gameObject.SetActive(false);

            sword.gameObject.SetActive(true);
            sword.Attack();
        }
        if (Input.GetKeyDown("x"))
        {
            sword.gameObject.SetActive(false);

            bow.gameObject.SetActive(true);
            bow.Attack();
        }
        if (Input.GetKeyDown("c"))
        {
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        if (bombAmmount > 0)
        {
            GameObject bombObject = Instantiate(bombPrefab);
            bombObject.transform.position = transform.position + new Vector3(0f,1f,0f) + model.transform.forward;
            Vector3 throwingDirection = (model.transform.forward + Vector3.up).normalized;
            bombObject.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed);
            bombAmmount--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyBullet>() != null)
        {
            Hit((transform.position - other.transform.position).normalized);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
            Hit((transform.position - collision.transform.position).normalized);
    }

    private void Hit(Vector3 direction)
    {
        Vector3 knockbackDirection = (direction + Vector3.up).normalized;
        impact += knockbackDirection * knockbackForce / mass;
        canJump = false;
    }
}