using CCollections;
using UnityEngine;

public class RoadFaceExtruded : RoadFace
{
    private Vector3[] _baseNormals;

    protected virtual bool CopyBaseVertices {get { return true; }}

    public RoadFaceExtruded(ManipulatableRoad manipulatableRoad, TrackingList<Vector3> vertices, Vector3[] normals) : base(manipulatableRoad)
    {
        base.VerticesArrStartSize = vertices.PopulatedCount;
        _baseNormals = normals;
    }

    protected override void DoAlgorithmStep(TrackingList<Vector3> vertices, int xPointer, int zPointer, int index)
    {
        // Create the new vertices
        if (CopyBaseVertices)
        {
            Vector3 copiedVertex = vertices.Get(index);
            vertices.Add(copiedVertex);
        }

        Vector3 extrudedVertex = vertices.Get(index) - (_baseNormals[index] * base.ManipulatableRoad.Width);
        vertices.Add(extrudedVertex);
    }
}
