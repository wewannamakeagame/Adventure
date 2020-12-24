using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject model;

    public float rotatingSpeed = 2f;

    [Header("Movement")]
    public float movingVelocity = 10;

    public float jumpingVelocity = 150;

    [Header("Equipment")]
    public Sword sword;

    public int bombAmmount = 5;
    public float throwingSpeed;
    public GameObject bombPrefab;
    public Bow bow;
    private Rigidbody playerRigidbody;
    private bool canJump;
    private Quaternion targetModelRoatation;

    // Start is called before the first frame update
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        targetModelRoatation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        // Raycast to identify if the player can jump
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.01f))
        {
            canJump = true;
        }
        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, targetModelRoatation,
            Time.deltaTime * rotatingSpeed);
        ProcessInput();
    }

    private void ProcessInput()
    {
        playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
        // Move in the XZ plane.
        //move right
        if (Input.GetKey(KeyCode.D))
        {
            playerRigidbody.velocity = new Vector3(movingVelocity, playerRigidbody.velocity.y,
                playerRigidbody.velocity.z);
            targetModelRoatation = Quaternion.Euler(0, 90, 0);
        }
        //move left
        if (Input.GetKey(KeyCode.A))
        {
            playerRigidbody.velocity = new Vector3(-movingVelocity, playerRigidbody.velocity.y,
                playerRigidbody.velocity.z);
            targetModelRoatation = Quaternion.Euler(0, 270, 0);
        }
        //move forward
        if (Input.GetKey(KeyCode.W))
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y,
                movingVelocity);
            targetModelRoatation = Quaternion.Euler(0, 0, 0);
        }
        //move backwards
        if (Input.GetKey(KeyCode.S))
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y,
                -movingVelocity);
            targetModelRoatation = Quaternion.Euler(0, 180, 0);
        }

        //jump up
        if (canJump && Input.GetKeyDown("space"))
        {
            canJump = false;
            playerRigidbody.AddForce(Vector3.up * jumpingVelocity);
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpingVelocity, playerRigidbody.velocity.z);
        }

        //Check equipment interaction
        if (Input.GetKeyDown("z"))
        {
            sword.Attack();
        }
        if (Input.GetKeyDown("x"))
        {
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
            bombObject.transform.position = transform.position + model.transform.forward;
            Vector3 throwingDirection = (model.transform.forward + Vector3.up).normalized;
            bombObject.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed);
            bombAmmount--;
        }
    }
}