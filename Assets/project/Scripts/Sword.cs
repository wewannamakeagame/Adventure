using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float swingingSpeed = 5f;
    public float cooldownSpeed = 2f;
    public float attackDuration = 0.35f;

    public float cooldownDuration = 0.5f;

    private Quaternion targetRotation;
    private float cooldownTimer;
    private bool isAttacking;

    // Start is called before the first frame update
    private void Start()
    {
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localRotation =
            Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * (isAttacking ? swingingSpeed : cooldownSpeed));
        cooldownTimer -= Time.deltaTime;
    }

    public void Attack()
    {
        if (cooldownTimer > 0f)
            return;

        targetRotation = Quaternion.Euler(90, 0, 0);
        cooldownTimer = cooldownDuration;

        StartCoroutine(CooldownWait());
    }

    private IEnumerator CooldownWait()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;

        targetRotation = Quaternion.Euler(0, 0, 0);
    }
}