using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Attack()
    {
        GameObject arrowObject = Instantiate(arrowPrefab);
        arrowObject.transform.position = transform.position + transform.forward;
        arrowObject.transform.forward = transform.forward;
    }
}