using System.Linq;
using UnityEngine;

public class ManipulatableRoad : MonoBehaviour
{
    private ManipulatableObject _manipulatableObject;
    public ManipulatableObject ManipulatableObject
    {
        get { return _manipulatableObject; }
        set { _manipulatableObject = value; }
    }

    #region Variables: Vertex and index counts
    private int _vertexCountX = 2;
    public int VertexCountX
    {
        get { return _vertexCountX; }
    }

    private int _vertexCountZ = 2;
    public int VertexCountZ
    {
        get { return _vertexCountZ; }
    }

    public int VertexCount
    {
        get { return _vertexCountX * _vertexCountZ; }
    }

    public int IndexCount
    {
        get { return (_vertexCountX - 1) * (_vertexCountZ - 1) * 6; }
    }
    #endregion

    #region Variables: Loop cuts
    private int _loopCutsX = 0;
    public int LoopCutsX
    {
        get { return _loopCutsX; }
        set
        {
            if (value < 0)
                return;

            _loopCutsX = value;
            _vertexCountX = 2 + _loopCutsX;
            RegenerateMesh();
        }
    }

    private int _loopCutsZ = 0;
    public int LoopCutsZ
    {
        get { return _loopCutsZ; }
        set
        {
            if (value < 0)
                return;

            _loopCutsZ = value; 
            _vertexCountZ = 2 + _loopCutsZ;
            RegenerateMesh();
        }
    }

    public (int, int) LoopCuts
    {
        get { return (_loopCutsX, _loopCutsZ); }
        set
        {
            bool doRegen = false;

            if (value.Item1 >= 0)
            {
                _loopCutsX = value.Item1;
                _vertexCountX = 2 + _loopCutsX;
                doRegen = true;
            }
            
            if (value.Item2 >= 0)
            {
                _loopCutsZ = value.Item2;
                _vertexCountZ = 2 + _loopCutsZ;
                doRegen = true;
            }

            if (doRegen)
                RegenerateMesh();
        }
    }
    #endregion

    #region Variables: Road width
    private float _width = 1f;
    private bool _widthZero = false; // Indicates wether the with is zero or not

    public float Width
    {
        get { return _width; }
        set
        {
            _width = value;

            if (Mathf.Abs(_width) < 0.001f)
            {
                _widthZero = true;
                _drawFaces = new bool[]{ _drawFaces[0], false, false, false, false };
            }
            else
            {
                if (_widthZero)
                    _drawFaces = _fallBackDrawFaces;

                _widthZero = false;
            }
                
            RegenerateMesh();
        }
    }
    #endregion

    #region Variables: Draw faces
    // Boolean array with a length of 5 representing: Down, forward, back, left and right
    private bool[] _drawFaces = { true, true, true, true, true };
    private bool[] _fallBackDrawFaces = new bool[5]; // Used for width changes

    public bool[] DrawFaces
    {
        get { return _drawFaces; }
        set
        {
            if (value.Length == 5)
            {
                _drawFaces = value;
                _fallBackDrawFaces = _drawFaces;
                RegenerateMesh();
            }
        }
    }
    #endregion

    #region Unity methods
    void Awake()
    {
        _fallBackDrawFaces = _drawFaces;
    }

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

    public void RegenerateMesh()
    {
        // Create arrays to hold the base vertices and indices
        TrackingArray<Vector3> vertices = new TrackingArray<Vector3>(ManipulatableRoadHelper.CalculateTotalVertexCount(this));
        TrackingArray<int> indices = new TrackingArray<int>(ManipulatableRoadHelper.CalculateTotalIndexCount(this));

        // Create the base face that face upwards
        ManipulatableRoadHelper.CreateBaseFace(this, vertices, indices);
        
        // Calculate the normals for the base vertices
        Vector3[] baseNormals = ManipulatableRoadHelper.CalulateNormals(_manipulatableObject, vertices, indices);

        // Extrude sides and bottom from base face.
        if (_drawFaces[0]) // Down
            ManipulatableRoadHelper.ExtrudeBaseFaceBottom(this, Vector3.down,      vertices, indices, baseNormals);
        if (_drawFaces[1]) // Forward
            ManipulatableRoadHelper.ExtrudeBaseFaceFB(this, Vector3.forward,   vertices, indices, baseNormals);
        if (_drawFaces[2]) // Back
            ManipulatableRoadHelper.ExtrudeBaseFaceFB(this, Vector3.back,      vertices, indices, baseNormals);
        if (_drawFaces[3]) // Left
            ManipulatableRoadHelper.ExtrudeBaseFaceLR(this, Vector3.left,      vertices, indices, baseNormals);
        if (_drawFaces[4]) // Right
            ManipulatableRoadHelper.ExtrudeBaseFaceLR(this, Vector3.right,     vertices, indices, baseNormals);

        ManipulatableRoadHelper.UpdateMesh(_manipulatableObject, vertices, indices);
    }
}
