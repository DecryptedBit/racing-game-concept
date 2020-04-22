using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManipulatableObjectDeformer : MonoBehaviour
{
    private ManipulatableObject _manipulatableObject;
    public ManipulatableObject ManipulatableObject
    {
        get { return _manipulatableObject; }
        set { _manipulatableObject = value; }
    }

    private int _loopCutsX = 0;
    public int LoopCutsX
    {
        get { return _loopCutsX; }
        set
        {
            _loopCutsX = value;
            MeshManipulator.GenerateMeshFaces(this);
        }
    }

    private int _loopCutsZ = 0;
    public int LoopCutsZ
    {
        get { return _loopCutsZ; }
        set
        {
            _loopCutsZ = value; 
            MeshManipulator.GenerateMeshFaces(this);
        }
    }

    #region Unity methods
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (_manipulatableObject != null)
        {
            foreach (Vector3 verticy in _manipulatableObject.Mesh.vertices)
            {
                Gizmos.DrawWireSphere(transform.TransformPoint(verticy), 0.03f);
            }
        }
    }
    #endregion
}
