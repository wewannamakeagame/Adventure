using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float duration = 5f;
    public float radius = 3f;
    public float explosionDuration = 0.25f;
    public GameObject explosionModel;

    private float explosionTimer;
    private bool exploded;

    // Start is called before the first frame update
    private void Start()
    {
        explosionTimer = duration;
        explosionModel.transform.localScale = Vector3.one * radius;
        explosionModel.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0f && exploded == false)
        {
            exploded = true;
            //Collider[] hitObjects = Physics.OverlapSphere(transform.position, radius);

            //foreach (Collider collider in hitObjects)
            //{
            //    Debug.Log(collider.name + "was hit!");
            //}

            StartCoroutine(Expload());
            //Destroy(gameObject);
        }
    }

    private IEnumerator Expload()
    {
        explosionModel.SetActive(true);
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }
}