using CCollections;
using UnityEngine;

namespace MapEditor.Manipulation
{
    public class RoadFaceUp : RoadFace
    {
        private float _vertexStepSizeX;
        private float _vertexStepSizeZ;

        public RoadFaceUp(ManipulatableRoad manipulatableRoad) : base(manipulatableRoad)
        {
            // Calculate the sizes between each vertex
            _vertexStepSizeX = 1f / (base.ManipulatableRoad.LoopCutsX + 1f);
            _vertexStepSizeZ = 1f / (base.ManipulatableRoad.LoopCutsZ + 1f);
        }

        protected override void DoAlgorithmStep(TrackingList<Vector3> vertices, int xPointer, int zPointer, int index)
        {
            // Create the new vertex
            Vector3 vertex = new Vector3(
                -0.5f + ((float)xPointer * _vertexStepSizeX), 
                0f,
                -0.5f + ((float)zPointer * _vertexStepSizeZ)
            );

            // Add the new vertex
            vertices.Add(vertex);
        }
    }   
}
