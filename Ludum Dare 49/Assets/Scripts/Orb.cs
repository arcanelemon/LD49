using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{

    private int bounceCounter = 0;
    private int maxBounce = 2;

    private PlayerController player;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground" )
        {
            bounceCounter += 1;

            if (bounceCounter >= maxBounce) 
            {
                CueTeleport();
            }
        }
    }

    public void CueTeleport() {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        player.Teleport(transform.position);
    }

    public void Bind(PlayerController player) 
    {
        this.player = player;
    }

    private void OnDestroy()
    {
        if (player != null) 
        {
            player.SetOrbStatus(false);
        }
    }
}
