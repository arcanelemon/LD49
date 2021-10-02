using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity;

    [SerializeField]
    private float health;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float throwForce;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private int maxJumps;

    [SerializeField]
    private GameObject orbPrefab;

    [SerializeField]
    private GameObject projectilePrefab;

    private int numJumps;

    private float xRot;

    private float yRot;

    private bool grounded;

    private bool orbStatus;

    private Vector3 movementInputs;

    private FirstPersonSway fpSway;

    private Camera cam;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        MouseUtils.LockMouse();

        fpSway = GetComponentInChildren<FirstPersonSway>();
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementInputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Look();

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire2")) 
        {
            ThrowOrb();
        }
    }

    private void FixedUpdate()
    {
        Move();
        
        if (Input.GetButtonDown("Jump") && grounded) 
        {
            Jump();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if ((tag is "Wall" || tag is "Ground") && !grounded) 
        {
            grounded = true;
        }
    }

    private void Move() 
    {
        rb.MovePosition(transform.position + ((transform.forward * movementInputs.z) + (transform.right * movementInputs.x)) * speed * Time.deltaTime);
    }

    private void Look() 
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        fpSway.AddLookSway(mouseX, mouseY);

        xRot = mouseX * mouseSensitivity;
        yRot -= mouseY * mouseSensitivity;

        //RotY
        yRot = Mathf.Clamp(yRot, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(yRot, 0, 0);

        //RotX
        transform.Rotate(0, xRot, 0);
    }


    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
        fpSway.AddImpactSway();
    }


    private void ThrowOrb() {

        if (!orbStatus) 
        {
            GameObject orb = Instantiate(orbPrefab, cam.transform.position, cam.transform.rotation);
            orb.GetComponent<Orb>().Bind(this);
            orb.GetComponent<Rigidbody>().AddForce(new Vector3(0, transform.up.y / 2, transform.forward.z) * throwForce * 10);

            orbStatus = true;
        }
    }

    private void Shoot() {
        Instantiate(projectilePrefab, cam.transform.position, cam.transform.rotation);
    }

    private void Die() 
    {
    
    }

    private IEnumerator RegenHealth()
    {
        yield return new WaitForSeconds(5);

        while (health < 100) 
        {
            health += 5;
            yield return new WaitForSeconds(.1f);
        }

        health = 100;
        yield break;
    }

    public void Teleport(Vector3 position)
    { 
        
    }

    public void SetOrbStatus(bool status) 
    {
        orbStatus = status;
    }

    public void Damage(float damage)
    {
        // StopRegenHealthCoroutine();

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
}