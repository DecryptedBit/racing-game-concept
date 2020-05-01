using UnityEngine;

public static class ManipulatableRoadHelper
{
    public static void CreateBaseFace(ManipulatableRoad manipulatableRoad, TrackingArray<Vector3> vertices, TrackingArray<int> indices)
    {
        // Calculate the amount of vertices 
        int vertexCountX = manipulatableRoad.VertexCountX;
        int vertexCountZ = manipulatableRoad.VertexCountZ;

        // Calculate the sizes between each vertex
        float vertexStepSizeX = 1f / (manipulatableRoad.LoopCutsX + 1f);
        float vertexStepSizeZ = 1f / (manipulatableRoad.LoopCutsZ + 1f);

        // Create pointers used for indexing and setting sizes
        int xPointer = 0;
        int zPointer = 0;

        // Create vertices and indices
        for (int i = 0; i < manipulatableRoad.VertexCount; i++)
        {
            // Set and reset the pointers
            if (zPointer == vertexCountZ)
            {
                xPointer += 1;
                zPointer = 0;
            }

            // Create the new vertex
            Vector3 vertex = new Vector3(
                -0.5f + ((float)xPointer * vertexStepSizeX), 
                0f,
                -0.5f + ((float)zPointer * vertexStepSizeZ)
            );

            // Add the new vertex
            vertices.Add(vertex);

            // Do a check for bounds
            if (zPointer != vertexCountZ - 1 && xPointer != vertexCountX - 1)
                CreateIndices(manipulatableRoad, xPointer, zPointer, indices);

            zPointer += 1;
        }
    }

    public static void ExtrudeBaseFaceBottom(ManipulatableRoad manipulatableRoad, Vector3 faceDirection, 
        TrackingArray<Vector3> vertices, TrackingArray<int> indices, Vector3[] baseNormals)
    {
        int startSize = vertices.PopulatedCount;

        // Create pointers used for indexing and setting sizes
        int xPointer = 0;
        int zPointer = 0;

        // Extude the vertices
        for (int j = 0; j < manipulatableRoad.VertexCount; j++)
        {
            // Set and reset the pointers
            if (zPointer == manipulatableRoad.VertexCountZ)
            {
                xPointer += 1;
                zPointer = 0;
            }

            // Create the new vertices
            Vector3 extrudedVertex = vertices.Get(j) - (baseNormals[j] * manipulatableRoad.Width);

            // Add the new vertex
            vertices.Add(extrudedVertex);

            // Do a check for bounds
            if (zPointer != manipulatableRoad.VertexCountZ - 1 && xPointer != manipulatableRoad.VertexCountX - 1)
                CreateIndices(manipulatableRoad, xPointer, zPointer, indices, startSize, false, true);

            zPointer += 1;
        }
    }

    public static void ExtrudeBaseFaceFB(ManipulatableRoad manipulatableRoad, Vector3 faceDirection, 
        TrackingArray<Vector3> vertices, TrackingArray<int> indices, Vector3[] baseNormals)
    {
        TrackingArray<ManipulatableRoadSide> verticesToExtrude = new TrackingArray<ManipulatableRoadSide>(manipulatableRoad.VertexCountX);
        int startSize = vertices.PopulatedCount;
        bool copyVertex = true;
        bool doCCIndices = true; // Draw the triangles counter-clockwise

        // Create pointers used for indexing and setting sizes
        int xPointer = 0;
        int zPointer = 0;

        /*
            Get the vertices that need to be extruded.
            The format is the following:
                Starting vertex, ending vertex, pointer increment index
        */
        if (faceDirection == Vector3.forward)
        {
            for (int i = 0; i < manipulatableRoad.VertexCountX; i++)
            {
                verticesToExtrude.Add(new ManipulatableRoadSide(
                    (i + 1) * manipulatableRoad.VertexCountZ - 1,
                    (i + 1) * manipulatableRoad.VertexCountZ
                ));
            }
        }
        else if (faceDirection == Vector3.back)
        {
            for (int i = 0; i < manipulatableRoad.VertexCountX; i++)
            {
                verticesToExtrude.Add(new ManipulatableRoadSide(
                    i * manipulatableRoad.VertexCountZ,
                    i * manipulatableRoad.VertexCountZ + 1
                ));
            }

            doCCIndices = false;
        }

        // Extude the vertices
        for (int i = 0; i < verticesToExtrude.PopulatedCount; i++)
        {
            int startingVertex = verticesToExtrude.Get(i).StartingVertex;
            int endingVertex = verticesToExtrude.Get(i).EndingVertex;
            int pointerIncrementIndex = verticesToExtrude.Get(i).PointerIncrementIndex;

            for (int j = startingVertex; j < endingVertex; j++)
            {
                // Set and reset the pointers
                if (zPointer == pointerIncrementIndex)
                {
                    xPointer += 1;
                    zPointer = 0;
                }

                // Create the new vertices
                Vector3 extrudedVertex = vertices.Get(j) - (baseNormals[j] * manipulatableRoad.Width);
                Vector3 copiedVertex = Vector3.zero;
                if (copyVertex)
                    copiedVertex = vertices.Get(j);

                // Add the new vertex
                vertices.Add(extrudedVertex);
                if (copiedVertex != Vector3.zero)
                    vertices.Add(copiedVertex);

                // Do a check for bounds
                if (zPointer != manipulatableRoad.VertexCountZ - 1 && xPointer != manipulatableRoad.VertexCountX - 1)
                    CreateIndices(manipulatableRoad, xPointer, zPointer, indices, startSize, true, doCCIndices);

                zPointer += 1;
            }
        }
    }

    public static void ExtrudeBaseFaceLR(ManipulatableRoad manipulatableRoad, Vector3 faceDirection, 
        TrackingArray<Vector3> vertices, TrackingArray<int> indices, Vector3[] baseNormals)
    {
        TrackingArray<ManipulatableRoadSide> verticesToExtrude = new TrackingArray<ManipulatableRoadSide>(1);
        int startSize = vertices.PopulatedCount;
        bool copyVertex = true;
        bool doCCIndices = true; // Draw the triangles counter-clockwise

        // Create pointers used for indexing and setting sizes
        int xPointer = 0;
        int zPointer = 0;

        if (faceDirection == Vector3.left)
        {
            verticesToExtrude.Add(new ManipulatableRoadSide(
                0,
                manipulatableRoad.VertexCountZ,
                manipulatableRoad.VertexCountZ
            ));
        }
        else if (faceDirection == Vector3.right)
        {
            verticesToExtrude.Add(new ManipulatableRoadSide(
                manipulatableRoad.VertexCount - manipulatableRoad.VertexCountZ,
                manipulatableRoad.VertexCount,
                manipulatableRoad.VertexCount
            ));

            doCCIndices = false;
        }

        // Extude the vertices
        for (int i = 0; i < verticesToExtrude.PopulatedCount; i++)
        {
            int startingVertex = verticesToExtrude.Get(i).StartingVertex;
            int endingVertex = verticesToExtrude.Get(i).EndingVertex;
            int pointerIncrementIndex = verticesToExtrude.Get(i).PointerIncrementIndex;

            for (int j = startingVertex; j < endingVertex; j++)
            {
                // Set and reset the pointers
                if (zPointer == pointerIncrementIndex)
                {
                    xPointer += 1;
                    zPointer = 0;
                }

                // Create the new vertices
                Vector3 extrudedVertex = vertices.Get(j) - (baseNormals[j] * manipulatableRoad.Width);
                Vector3 copiedVertex = Vector3.zero;
                if (copyVertex)
                    copiedVertex = vertices.Get(j);

                // Add the new vertex
                vertices.Add(extrudedVertex);
                if (copiedVertex != Vector3.zero)
                    vertices.Add(copiedVertex);

                // Do a check for bounds
                if (zPointer != manipulatableRoad.VertexCountZ - 1 && xPointer != manipulatableRoad.VertexCountX - 1)
                    CreateIndices(manipulatableRoad, zPointer, xPointer, indices, startSize, true, doCCIndices);

                zPointer += 1;
            }
        }
    }

    private static void CreateIndices(ManipulatableRoad manipulatableRoad, int xPointer, int zPointer, TrackingArray<int> indices, 
        int startSize=0, bool forSideFace = false, bool counterClockwise=false)
    {
        int toRemove = 0;

        if (forSideFace)
            toRemove = manipulatableRoad.LoopCutsZ;

        int currRowPos    = xPointer * (manipulatableRoad.VertexCountZ - toRemove);
        int nextRowPos    = (xPointer + 1) * (manipulatableRoad.VertexCountZ - toRemove);

        // First triangle
        indices.Add(startSize + currRowPos + zPointer + 1);

        if (!counterClockwise)
            indices.Add(startSize + nextRowPos + zPointer);

        indices.Add(startSize + currRowPos + zPointer);

        if (counterClockwise)
            indices.Add(startSize + nextRowPos + zPointer);

        // Second triangle
        indices.Add(startSize + currRowPos + zPointer + 1);

        if (!counterClockwise)
            indices.Add(startSize + nextRowPos + zPointer + 1);

        indices.Add(startSize + nextRowPos + zPointer);
        
        if (counterClockwise)
            indices.Add(startSize + nextRowPos + zPointer + 1);
    }

    #region Vertex and index counts
    public static int CalculateTotalVertexCount(ManipulatableRoad manipulatableRoad)
    {
        int totalVertexCount = 0;

        // Add the base face vertex count
        totalVertexCount += manipulatableRoad.VertexCount;

        // Add the sides and bottom vertex counts
        for (int i = 0; i < manipulatableRoad.DrawFaces.Length; i++)
        {
            if (!manipulatableRoad.DrawFaces[i])
                continue;

            if (i == 0)
                totalVertexCount += manipulatableRoad.VertexCount;
            else if (i == 1 || i == 2)
                totalVertexCount += manipulatableRoad.VertexCountX * 2;
            else if (i == 3 || i == 4)
                totalVertexCount += manipulatableRoad.VertexCountZ * 2;
        }

        return totalVertexCount;
    }

    public static int CalculateTotalIndexCount(ManipulatableRoad manipulatableRoad)
    {
        int totalIndexCount = 0;

        // Add the base face vertex count
        totalIndexCount += manipulatableRoad.IndexCount;

        // Add the sides and bottom vertex counts
        for (int i = 0; i < manipulatableRoad.DrawFaces.Length; i++)
        {
            if (!manipulatableRoad.DrawFaces[i])
                continue;

            if (i == 0)
                totalIndexCount += manipulatableRoad.IndexCount;
            else if (i == 1 || i == 2)
                totalIndexCount += (manipulatableRoad.VertexCountX - 1) * 6;
            else if (i == 3 || i == 4)
                totalIndexCount += (manipulatableRoad.VertexCountZ - 1) * 6;
        }

        return totalIndexCount;
    }
    #endregion

    #region Calculate normals
    public static Vector3[] CalulateNormals(ManipulatableObject manipulatableObject, TrackingArray<Vector3> vertices, TrackingArray<int> triangles)
    {
        Vector3[] normals = new Vector3[vertices.PopulatedCount];
        int triangleCount = triangles.PopulatedCount / 3;

        for (int i = 0; i < triangleCount; i++)
        {
            int index = i * 3;

            // Get indices for the triangle
            int vIndexA = triangles.Get(index);
            int vIndexB = triangles.Get(index + 1);
            int vIndexC = triangles.Get(index + 2);

            // Calculate the normal for the triangle
            Vector3 triangleNormal = NormalFromVertices(vertices.Get(vIndexA), vertices.Get(vIndexB), vertices.Get(vIndexC));

            // Add the normals to the normals array
            normals[vIndexA] += triangleNormal;
            normals[vIndexB] += triangleNormal;
            normals[vIndexC] += triangleNormal;
        }

        // Normalize all normals
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }

        return normals;
    }

    private static Vector3 NormalFromVertices(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
    {
        Vector3 edgeAB = vertexB - vertexA;
        Vector3 edgeAC = vertexC - vertexA;

        return Vector3.Cross(edgeAB, edgeAC).normalized;
    }
    #endregion

    public static void UpdateMesh(ManipulatableObject manipulatableObject, TrackingArray<Vector3> vertices, TrackingArray<int> triangles)
    {
        manipulatableObject.MeshCollider.sharedMesh = null;
        
        if (vertices != null)
        {
            manipulatableObject.Mesh.Clear();
            manipulatableObject.Mesh.vertices = vertices.ToArray();
        }

        if (triangles != null)
        {
            manipulatableObject.Mesh.triangles = triangles.ToArray();
            manipulatableObject.Mesh.RecalculateNormals();
        }
        
        manipulatableObject.MeshCollider.sharedMesh = manipulatableObject.Mesh;
    }
}
