using CCollections;
using UnityEngine;

public class RoadFaceDown : RoadFaceExtruded
{
    private Vector3[] _baseNormals;
    private int _startSize;

    protected override bool DoIndicesCountercw {get { return true; }}
    protected override int LoopStart {get { return 0; }}
    protected override int LoopUpdate {get { return 1; }}
    protected override int PointerUpdateInterval {get { return base.ManipulatableRoad.VertexCountZ; }}
    protected override bool CopyBaseVertices {get { return false; }}

    public RoadFaceDown(ManipulatableRoad manipulatableRoad, TrackingList<Vector3> vertices, Vector3[] normals)
        : base(manipulatableRoad, vertices, normals) { }
}