using UnityEngine;

namespace MapEditor.Manipulation
{
    [RequireComponent(typeof(Renderer), typeof(MeshFilter), typeof(MeshCollider))]
    public class ManipulatableObject : MonoBehaviour
    {
        private ManipulatableRoad _manipulatableRoad;

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

            _manipulatableRoad = GetComponent<ManipulatableRoad>();
            if (_manipulatableRoad != null)
                _manipulatableRoad.ManipulatableObject = this;
        }

        public void Select(Material material)
        {
            _renderer.material = material;
        }

        public void Release()
        {
            _renderer.material = _standardMat;
        }

        public void Regenerate()
        {
            if (_manipulatableRoad != null)
                _manipulatableRoad.RegenerateMesh();
        }

        public void AddLoopCut(bool x, bool z)
        {
            if (_manipulatableRoad == null)
                return;

            if (x && z)
                _manipulatableRoad.LoopCuts = (_manipulatableRoad.LoopCutsX + 1, _manipulatableRoad.LoopCutsZ + 1);
            else if (x)
                _manipulatableRoad.LoopCutsX += 1;
            else if (z)
                _manipulatableRoad.LoopCutsZ += 1;
                
        }

        public void RemoveLoopCut()
        {
            if (_manipulatableRoad != null)
                _manipulatableRoad.LoopCuts = (_manipulatableRoad.LoopCutsX - 1, _manipulatableRoad.LoopCutsZ - 1);
        }

        public void AddWidth()
        {
            if (_manipulatableRoad != null)
                _manipulatableRoad.Width += 0.2f;
        }

        public void RemoveWidth()
        {
            if (_manipulatableRoad != null)
                _manipulatableRoad.Width -= 0.2f;
        }

        public void SetDrawFaces(bool state)
        {
            if (_manipulatableRoad != null)
                _manipulatableRoad.DrawFaces = new bool[] { state, state, state, state, state };
        }

        public bool GetGlobalDrawFaces()
        {
            if (_manipulatableRoad == null)
                return false;
            
            return _manipulatableRoad.DrawFaces[0];
        }
    }
}
