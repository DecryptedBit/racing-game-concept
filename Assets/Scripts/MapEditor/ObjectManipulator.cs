using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour {
    public KeyCode primaryActionButton = KeyCode.Mouse0;
    public KeyCode deformButton = KeyCode.C;

    private ManipulatableObject selectedObject = null;
    private List<Vector3> originalVerticies;
    private List<Vector3> modifiedVerticies;

    void Update()
    {
        if (Input.GetKeyDown(primaryActionButton))
            CastSelectRay();
        if (Input.GetKeyDown(deformButton) && selectedObject != null)
            selectedObject.ApplyLoopCut();
    }

    private void CastSelectRay()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            ManipulatableObject newlySelectedObject = hit.transform.GetComponent<ManipulatableObject>();

            if (newlySelectedObject != selectedObject)
            {
                ReleaseSelectedObject();
                
                if (newlySelectedObject != null)
                {
                    selectedObject = newlySelectedObject;
                    selectedObject.Select();
                }
            }
            else
                ReleaseSelectedObject();
        }
        else
            ReleaseSelectedObject();
    }

    private void ReleaseSelectedObject()
    {
        if (selectedObject != null)
            selectedObject.Release();

        selectedObject = null;
    }
}
