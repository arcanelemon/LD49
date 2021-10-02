using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField]
    private int damage;

    [SerializeField]
    private int speed;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed * 100, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (tag is "Player") 
        {
            collision.gameObject.GetComponent<PlayerController>();
        } else if (tag is "Player Projectile")
        {
            Destroy(collision.gameObject);
        } else if (tag is "Junk")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}
