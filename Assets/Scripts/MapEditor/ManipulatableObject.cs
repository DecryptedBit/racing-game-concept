using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(MeshFilter), typeof(MeshCollider))]
public class ManipulatableObject : MonoBehaviour
{
    private ManipulatableObjectDeformer _deformer;

    public Material selectedMat;
    private Material _standardMat;

    #region Variables: Components
    private Renderer _renderer;
    public Renderer Renderer
    {
        get { return _renderer; }
    }

    private MeshFilter _meshFilter;
    public MeshFilter MeshFilter
    {
        get { return _meshFilter; }
    }
    public Mesh Mesh
    {
        get { return _meshFilter.mesh; }
    }

    private MeshCollider _meshCollider;
    public MeshCollider MeshCollider
    {
        get { return _meshCollider; }
    }
    #endregion

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();

        _standardMat = _renderer.material;

        _deformer = GetComponent<ManipulatableObjectDeformer>();
        if (_deformer != null)
            _deformer.ManipulatableObject = this;
    }

    public void Select()
    {
        _renderer.material = selectedMat;
    }

    public void Release()
    {
        _renderer.material = _standardMat;
    }

    public void ApplyLoopCut()
    {
        if (_deformer != null)
        {
            _deformer.LoopCutsX += 1;
            _deformer.LoopCutsZ += 1;
        }
    }
}
