using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float throttle;
    public float steer;
    public bool toggleLights;
    public bool toggleCam;
    public bool toggleNOS;
    public bool brake;

    void Update()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");

        toggleLights = Input.GetKeyDown(KeyCode.L);
        toggleCam = Input.GetKeyDown(KeyCode.C);
        toggleNOS = Input.GetKeyDown(KeyCode.LeftShift);
        brake = Input.GetKey(KeyCode.Space);
    }
}
