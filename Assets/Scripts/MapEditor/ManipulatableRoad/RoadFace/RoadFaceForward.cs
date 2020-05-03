using CCollections;
using UnityEngine;

public class RoadFaceForward : RoadFaceExtruded
{
    protected override bool IsSideFace {get { return true; }}
    protected override int LoopStart {get { return base.ManipulatableRoad.VertexCountZ - 1; }}
    protected override int LoopUpdate {get { return base.ManipulatableRoad.VertexCountZ; }}
    protected override int PointerUpdateInterval {get { return 1; }}

    public RoadFaceForward(ManipulatableRoad manipulatableRoad, TrackingList<Vector3> vertices, Vector3[] normals)
        : base(manipulatableRoad, vertices, normals) {}
}
