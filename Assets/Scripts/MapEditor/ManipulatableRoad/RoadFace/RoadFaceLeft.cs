using CCollections;
using UnityEngine;

public class RoadFaceLeft : RoadFaceExtruded
{
    protected override bool IsSideFace {get { return true; }}
    protected override int LoopEnd {get { return base.ManipulatableRoad.VertexCountZ; }}

    public RoadFaceLeft(ManipulatableRoad manipulatableRoad, TrackingList<Vector3> vertices, Vector3[] normals)
        : base(manipulatableRoad, vertices, normals){}

    protected override void CreateIndices(int xPointer, int zPointer, TrackingList<int> indices)
    {
        base.CreateIndices(zPointer, xPointer, indices);
    }
}
