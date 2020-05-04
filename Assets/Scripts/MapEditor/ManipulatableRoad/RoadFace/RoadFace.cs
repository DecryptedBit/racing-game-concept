using CCollections;
using UnityEngine;

namespace MapEditor.Manipulation
{
    public abstract class RoadFace
    {
        #region Variables and properties
        private ManipulatableRoad _manipulatableRoad;
        protected ManipulatableRoad ManipulatableRoad {get { return _manipulatableRoad; }}

        protected virtual bool IsSideFace {get { return false; }}
        protected virtual bool DoIndicesCountercw {get { return false; }}

        private int _verticesArrStartSize;
        protected virtual int VerticesArrStartSize
        {
            get { return _verticesArrStartSize; }
            set { _verticesArrStartSize = value; }
        }

        protected virtual int LoopStart {get { return 0; }}
        protected virtual int LoopEnd {get { return _manipulatableRoad.VertexCount; }}
        protected virtual int LoopUpdate {get { return 1; }}
        protected virtual int PointerUpdateInterval {get { return _manipulatableRoad.VertexCountZ; }}
        #endregion

        public RoadFace(ManipulatableRoad manipulatableRoad)
        {
            _verticesArrStartSize = 0;
            _manipulatableRoad = manipulatableRoad;
        }

        public void CreateFace(TrackingList<Vector3> vertices, TrackingList<int> indices)
        {
            // Create pointers used for indexing and setting sizes
            int xPointer = 0;
            int zPointer = 0;

            for (int i = LoopStart; i < LoopEnd; i += LoopUpdate)
            {
                // Set and reset the pointers
                if (zPointer == PointerUpdateInterval)
                {
                    xPointer += 1;
                    zPointer = 0;
                }

                DoAlgorithmStep(vertices, xPointer, zPointer, i);

                // Do a check for bounds and create indices if necessary
                if (zPointer != _manipulatableRoad.VertexCountZ - 1 && xPointer != _manipulatableRoad.VertexCountX - 1)
                    CreateIndices(xPointer, zPointer, indices);

                zPointer++;
            }
        }

        protected abstract void DoAlgorithmStep(TrackingList<Vector3> vertices, int xPointer, int zPointer, int index);

        protected virtual void CreateIndices(int xPointer, int zPointer, TrackingList<int> indices)
        {
            int sideFaceCorrection = 0;

            if (IsSideFace)
                sideFaceCorrection = ManipulatableRoad.LoopCutsZ;

            int currRowPos    = xPointer * (ManipulatableRoad.VertexCountZ - sideFaceCorrection);
            int nextRowPos    = (xPointer + 1) * (ManipulatableRoad.VertexCountZ - sideFaceCorrection);

            // First triangle
            indices.Add(VerticesArrStartSize + currRowPos + zPointer + 1);

            if (!DoIndicesCountercw)
                indices.Add(VerticesArrStartSize + nextRowPos + zPointer);

            indices.Add(VerticesArrStartSize + currRowPos + zPointer);

            if (DoIndicesCountercw)
                indices.Add(VerticesArrStartSize + nextRowPos + zPointer);

            // Second triangle
            indices.Add(VerticesArrStartSize + currRowPos + zPointer + 1);

            if (!DoIndicesCountercw)
                indices.Add(VerticesArrStartSize + nextRowPos + zPointer + 1);

            indices.Add(VerticesArrStartSize + nextRowPos + zPointer);
            
            if (DoIndicesCountercw)
                indices.Add(VerticesArrStartSize + nextRowPos + zPointer + 1);
        }
    }
}
