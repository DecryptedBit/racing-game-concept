using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MeshManipulator
{
    public static void GenerateMeshFaces(ManipulatableObjectDeformer moDeformer)
    {
        Vector3 scale = moDeformer.transform.lossyScale;

        int loopCutsX = moDeformer.LoopCutsX;
        int loopCutsZ = moDeformer.LoopCutsZ;

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        GenerateMeshFace(Vector3.forward, scale.x,    scale.y,    scale.z,    loopCutsX,  0,          vertices, indices);
        GenerateMeshFace(Vector3.back,    scale.x,    scale.y,    scale.z,    loopCutsX,  0,          vertices, indices);
        GenerateMeshFace(Vector3.right,   scale.z,    scale.y,    scale.x,    loopCutsZ,  0,          vertices, indices);
        GenerateMeshFace(Vector3.left,    scale.z,    scale.y,    scale.x,    loopCutsZ,  0,          vertices, indices);
        GenerateMeshFace(Vector3.up,      scale.x,    scale.z,    scale.y,    loopCutsX,  loopCutsZ,  vertices, indices);
        GenerateMeshFace(Vector3.down,    scale.x,    scale.z,    scale.y,    loopCutsX,  loopCutsZ,  vertices, indices);

        UpdateMesh(moDeformer.ManipulatableObject, vertices, indices);
    }

    // In this function we look straight at every face making the z-axis always be depth
    private static void GenerateMeshFace(Vector3 direction, 
        float scaleX, float scaleY, float scaleZ, int loopCutsX, int loopCutsY,
        List<Vector3> totalVertices, List<int> totalIndices)
    {
        // Calculate the amount of vertices 
        int vertexCountX = 2 + loopCutsX;
        int vertexCountY = 2 + loopCutsY;

        // Calculate the sizes between each vertex
        float vertexStepSizeX = 1f / (loopCutsX + 1f);
        float vertexStepSizeY = 1f / (loopCutsY + 1f);

        // Create an array to hold all vertices based on area size
        Vector3[] vertices = new Vector3[vertexCountX * vertexCountY];

        /*
            Create an array to hold all indices
            Calculation for the amount of quads:
                (vertex_count_x - 1) * (vertex_count_y - 1),
            We multiply this by six since every quad is two triangles
            and every triangle is 3 vertices/indices
        */
        int[] indices = new int[(vertexCountX - 1) * (vertexCountY - 1) * 6]; 

        // Determine the rotation these vertices need to receive
        Quaternion rotation = vectorDirectionToQuaternion(direction);

        // Generate the vertices
        int vArrayPointer = 0; // Vertex array pointer
        int iArrayPointer = 0; // Index array pointer

        for (int x = 0; x < vertexCountX; x++)
        {
            int xPosCurr    = x * vertexCountY;
            int xPosNext    = (x + 1) * vertexCountY;

            for (int y = 0; y < vertexCountY; y++)
            {
                // Create the new vertex and rotate it according to the direction
                Vector3 vertex = new Vector3(
                    -0.5f + ((float)x * vertexStepSizeX), 
                    -0.5f + ((float)y * vertexStepSizeY),
                    0.5f
                );
                vertex = rotation * vertex;
                
                // Add the vertex
                vertices[vArrayPointer] = vertex;

                // Create the set of indices in a clockwise manner
                if (y != vertexCountY - 1 && x != vertexCountX - 1) // Do not create triangles outside the vertex bounds
                {
                    // First triangle
                    indices[iArrayPointer]      = totalVertices.Count + xPosCurr + y + 1;
                    indices[iArrayPointer + 1]  = totalVertices.Count + xPosCurr + y;
                    indices[iArrayPointer + 2]  = totalVertices.Count + xPosNext + y;

                    // Second triangle
                    indices[iArrayPointer + 3]  = totalVertices.Count + xPosCurr + y + 1;
                    indices[iArrayPointer + 4]  = totalVertices.Count + xPosNext + y;
                    indices[iArrayPointer + 5]  = totalVertices.Count + xPosNext + y + 1;

                    // Increment the index array pointer
                    iArrayPointer += 6;
                }

                // Increment the vertex array pointer
                vArrayPointer ++;
            }
        }

        // Add the vertices and indices to the totals
        totalVertices.AddRange(vertices.ToList());
        totalIndices.AddRange(indices.ToList());
    }

    private static Quaternion vectorDirectionToQuaternion(Vector3 direction)
    {
        if (direction == Vector3.back)
            return Quaternion.Euler(0, 180, 0);
        else if (direction == Vector3.right || direction == Vector3.left)
            return Quaternion.Euler(0, 90 * direction.x, 0);
        else if (direction == Vector3.up || direction == Vector3.down)
            return Quaternion.Euler(-90 * direction.y, 0, 0);

        return Quaternion.Euler(0, 0, 0); // Forward is the base orientation
    }

    public static void UpdateMesh(ManipulatableObject manipulatableObject, List<Vector3> vertices, List<int> triangles)
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
