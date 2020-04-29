using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(LightingManager))]
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public InputManager input;
    public LightingManager lm;
    public UIManager ui;
    
    public List<WheelCollider> throttleWheels;
    public List<GameObject> steeringWheels;
    public List<GameObject> meshes;

    public Transform cm;
    public Rigidbody rb;

    public float speedCoefficient;
    public float brakeCoefficient;
    public float turnCoefficient;
    public float nos;

    private float _wheelRadius = 1f;
    private float _s;

    void Start()
    {
        input = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();

        if (cm)
            rb.centerOfMass = cm.localPosition;
    }

    void Update()
    {
        // Handle lighting / UI
    }

    void FixedUpdate()
    {
        if (input.toggleNOS)
            _s = speedCoefficient * nos;
        else
            _s = speedCoefficient;
        
        foreach (WheelCollider wheel in throttleWheels)
        {
            if (input.brake)
            {
                wheel.motorTorque = 0f;
                wheel.brakeTorque = brakeCoefficient* Time.fixedDeltaTime;
            }
            else
            {
                wheel.motorTorque = _s * Time.fixedDeltaTime * input.throttle;
                wheel.brakeTorque = 0f;
            }
        }

        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = turnCoefficient * input.steer;
            wheel.transform.localEulerAngles = new Vector3(0f, input.steer * turnCoefficient, 0f);
        }

        foreach (GameObject mesh in meshes)
        {
            var sign = transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1;
            mesh.transform.Rotate(sign * rb.velocity.magnitude / (2 * Mathf.PI * _wheelRadius), 0f, 0f);
        }
    }
}
