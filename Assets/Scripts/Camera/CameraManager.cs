using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputManager input;
    public GameObject target;
    public Vector3 offset;
    public Vector3 firstPersonOffset;
    public float dampening = 1f;

    private int _camMode = 0;

    void Update()
    {
        if (input.toggleCam)
        {
            _camMode = (_camMode + 1) % 2;
        }

        switch (_camMode)
        {
            case 1:
            default:
                transform.position = Vector3.Lerp(transform.position, 
                                        target.transform.localPosition + target.transform.TransformDirection(offset), dampening*Time.deltaTime);
                transform.LookAt(target.transform);
                break;
        }
    }
}
