using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private float fallTime;

    private IEnumerator fallCoroutine;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Fall() 
    {
        rb.isKinematic = false;
    }

    private void StartFallCoroutine() 
    {
        if (fallCoroutine == null) 
        {
            fallCoroutine = FallCoroutine();
            StartCoroutine(fallCoroutine);
        }
    }

    private IEnumerator FallCoroutine() 
    {
        yield return new WaitForSeconds(fallTime);
        Fall();
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag is "Player") 
        {
            StartFallCoroutine();
        }
    }

    public void SetFallTime(float fallTime) 
    {
        this.fallTime = fallTime;
    }
}
