using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrifiedWall : MonoBehaviour
{
    private float damage;
    private bool destroyProjectiles;

    private PlayerController player;


    private void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag is "Player")
        {
            if (player == null) 
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            }

            player.Damage(damage);
        } else if (collisionTag is "Projectile") 
        {
            Destroy(collision.gameObject);
        }
    }
}
