using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour {
    public KeyCode primaryActionButton = KeyCode.Mouse0;

    public KeyCode forceRegenBtn = KeyCode.R;
    public KeyCode loopCutDownBtn = KeyCode.Alpha1;
    public KeyCode loopCutXUpBtn = KeyCode.Alpha2;
    public KeyCode loopCutZUpBtn = KeyCode.Alpha3;
    public KeyCode drawFacesBtn = KeyCode.Alpha4;
    public KeyCode widthDownBtn = KeyCode.Alpha5;
    public KeyCode widthUpBtn = KeyCode.Alpha6;

    public Material selectedMaterial;

    private ManipulatableObject selectedObject = null;
    private List<Vector3> originalVerticies;
    private List<Vector3> modifiedVerticies;

    void Update()
    {
        if (Input.GetKeyDown(primaryActionButton))
            CastSelectRay();

        if (selectedObject != null)
        {
            if (Input.GetKeyDown(forceRegenBtn))
                selectedObject.Regenerate();

            if (Input.GetKeyDown(loopCutDownBtn))
                selectedObject.RemoveLoopCut();
            if (Input.GetKeyDown(loopCutXUpBtn))
                selectedObject.AddLoopCut(true, false);
            if (Input.GetKeyDown(loopCutZUpBtn))
                selectedObject.AddLoopCut(false, true);

            if (Input.GetKeyDown(drawFacesBtn))
            {
                bool state = !selectedObject.GetGlobalDrawFaces();
                selectedObject.SetDrawFaces(state);
            }
            
            if (Input.GetKeyDown(widthDownBtn))
                selectedObject.RemoveWidth();
            if (Input.GetKeyDown(widthUpBtn))
                selectedObject.AddWidth();
        }
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
                ReleaseManipulatableObject();
                
                if (newlySelectedObject != null)
                    SelectManipulatableObject(newlySelectedObject);
            }
            else
                ReleaseManipulatableObject();
        }
        else
            ReleaseManipulatableObject();
    }

    private void SelectManipulatableObject(ManipulatableObject manipulatableObject)
    {
        selectedObject = manipulatableObject;
        selectedObject.Select(selectedMaterial);
    }

    private void ReleaseManipulatableObject()
    {
        if (selectedObject != null)
            selectedObject.Release();

        selectedObject = null;
    }
}
