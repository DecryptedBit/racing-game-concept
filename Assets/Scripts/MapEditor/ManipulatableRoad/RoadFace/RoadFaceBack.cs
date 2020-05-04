using CCollections;
using UnityEngine;

namespace MapEditor.Manipulation
{
    public class RoadFaceBack : RoadFaceExtruded
    {
        protected override bool IsSideFace {get { return true; }}
        protected override bool DoIndicesCountercw {get { return true; }}
        protected override int LoopUpdate {get { return base.ManipulatableRoad.VertexCountZ; }}
        protected override int PointerUpdateInterval {get { return 1; }}

        public RoadFaceBack(ManipulatableRoad manipulatableRoad, TrackingList<Vector3> vertices, Vector3[] normals)
            : base(manipulatableRoad, vertices, normals){}
    }
}
