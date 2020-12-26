using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;

    public virtual void Hit()
    {
        health--;
        if (health <= 0)
            Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Sword>() != null)
        {
            if (other.GetComponent<Sword>().IsAttacking)
                Hit();
        }
        else if (other.GetComponent<Arrow>() != null)
        {
            Hit();
            Destroy(other.gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}