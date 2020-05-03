using CCollections;
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
        TrackingList<Vector3> vertices = new TrackingList<Vector3>(ManipulatableRoadHelper.CalculateTotalVertexCount(this));
        TrackingList<int> indices = new TrackingList<int>(ManipulatableRoadHelper.CalculateTotalIndexCount(this));
        Vector3[] baseNormals = null;

        for (int i = 0; i < 6; i++)
        {
            // Create a road face object representing a side
            RoadFace roadFace = null;

            if (i == 0) // Create the base face that faces upwards
                roadFace = new RoadFaceUp(this);
            else if (_drawFaces[i - 1])
            {
                if (i == 1)
                    roadFace = new RoadFaceDown(this, vertices, baseNormals);
                else if (i == 2)
                    roadFace = new RoadFaceForward(this, vertices, baseNormals);
                else if (i == 3)
                    roadFace = new RoadFaceBack(this, vertices, baseNormals);
                else if (i == 4)
                    roadFace = new RoadFaceLeft(this, vertices, baseNormals);
                else if (i == 5)
                    roadFace = new RoadFaceRight(this, vertices, baseNormals);
            }

            roadFace.CreateFace(vertices, indices);

            // Calculate the base face normals that the other faces will use
            if (i == 0)
                baseNormals = ManipulatableRoadHelper.CalulateNormals(_manipulatableObject, vertices, indices);
        }

        ManipulatableRoadHelper.UpdateMesh(_manipulatableObject, vertices, indices);
    }
}
