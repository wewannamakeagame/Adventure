using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //move right
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * 5f * Time.deltaTime;
        }
        //move left
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * 5f * Time.deltaTime;
        }
        //move forward
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * 5f * Time.deltaTime;
        }
        //move backwards
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * 5f * Time.deltaTime;
        }
    }
}