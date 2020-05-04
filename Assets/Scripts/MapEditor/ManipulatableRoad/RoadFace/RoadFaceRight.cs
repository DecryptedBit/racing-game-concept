using CCollections;
using UnityEngine;

namespace MapEditor.Manipulation
{
    public class RoadFaceRight : RoadFaceExtruded
    {
        protected override bool IsSideFace {get { return true; }}
        protected override bool DoIndicesCountercw {get { return true; }}
        protected override int LoopStart {get { return base.ManipulatableRoad.VertexCount - base.ManipulatableRoad.VertexCountZ; }}

        public RoadFaceRight(ManipulatableRoad manipulatableRoad, TrackingList<Vector3> vertices, Vector3[] normals)
            : base(manipulatableRoad, vertices, normals) {}
        
        protected override void CreateIndices(int xPointer, int zPointer, TrackingList<int> indices)
        {
            base.CreateIndices(zPointer, xPointer, indices);
        }
    }

}
