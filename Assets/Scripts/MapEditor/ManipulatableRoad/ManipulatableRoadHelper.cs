using CCollections;
using UnityEngine;

namespace MapEditor.Manipulation
{
    public static class ManipulatableRoadHelper
    {
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
        public static Vector3[] CalulateNormals(ManipulatableObject manipulatableObject, TrackingList<Vector3> vertices, TrackingList<int> triangles)
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

        public static void UpdateMesh(ManipulatableObject manipulatableObject, TrackingList<Vector3> vertices, TrackingList<int> triangles)
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
}
