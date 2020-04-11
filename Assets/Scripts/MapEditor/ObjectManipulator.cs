using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour {
    public KeyCode primaryActionButton = KeyCode.Mouse0;

    private ManipulatableObject selectedObject = null;

    void Update() {
        if (Input.GetKeyDown(primaryActionButton)) {
            ReleaseSelectedObject();
            CastSelectRay();
        }
    }

    private void CastSelectRay() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            selectedObject = hit.transform.GetComponent<ManipulatableObject>();

            if (selectedObject != null)
                selectedObject.Select();
        }
    }

    private void ReleaseSelectedObject() {
        if (selectedObject != null)
            selectedObject.Release();
    }
}
