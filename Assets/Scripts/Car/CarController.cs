using UnityEngine;

public class CarController : MonoBehaviour
{

    public Rigidbody rb;
    public float force = 500f;
    public float torque = 500f;
    public float gravAcceleration = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dt = Time.deltaTime;

        // Update gravity
        rb.AddForce(0, -gravAcceleration*dt, 0);
        
        // Turn left/right
        if (Input.GetKey(KeyCode.A))
            rb.AddTorque(-transform.up * torque);

        if (Input.GetKey(KeyCode.D))
            rb.AddTorque(transform.up * torque);
        
        // Forward/backward
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(0, 0, force*dt);

        if (Input.GetKey(KeyCode.S))
            rb.AddForce(0, 0, -force*dt);
    }
}
