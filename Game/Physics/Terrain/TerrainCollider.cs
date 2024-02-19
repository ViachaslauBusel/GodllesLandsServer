using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using System.Numerics;

namespace Game.Physics.Terrain
{
    public class TerrainCollider
    {
        public Mesh Mesh { get; init; }
        public TerrainCollider(int heightMapResolution, float terrainSize, float[] heightMap, BufferPool pool)
        {
            Mesh = CreateDeformedPlane(heightMapResolution, heightMapResolution, terrainSize, heightMap, pool);
        }

        private Mesh CreateDeformedPlane(int width, int height, float terrainSize, float[] heightMap, BufferPool pool)
        {
            pool.Take<Vector3>(width * height, out var vertices);
            float scaleWidth = terrainSize / (width - 1);
            float scaleHeight = terrainSize / (height - 1);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    vertices[height * i + j] = new Vector3(j * scaleWidth, heightMap[i * width + j], i * scaleHeight);
                }
            }

            var quadWidth = width - 1;
            var quadHeight = height - 1;
            var triangleCount = quadWidth * quadHeight * 2;
            pool.Take<Triangle>(triangleCount, out var triangles);

            for (int i = 0; i < quadHeight; ++i)
            {
                for (int j = 0; j < quadWidth; ++j)
                {
                    var triangleIndex = (i * quadWidth + j) * 2;
                    ref var triangle0 = ref triangles[triangleIndex];
                    ref var v00 = ref vertices[width * i + j];
                    ref var v01 = ref vertices[width * i + j + 1];
                    ref var v10 = ref vertices[width * (i + 1) + j];
                    ref var v11 = ref vertices[width * (i + 1) + j + 1];
                    triangle0.A = v00;
                    triangle0.B = v01;
                    triangle0.C = v10;
                    ref var triangle1 = ref triangles[triangleIndex + 1];
                    triangle1.A = v01;
                    triangle1.B = v11;
                    triangle1.C = v10;
                }
            }
            pool.Return(ref vertices);
            return new Mesh(triangles, Vector3.One, pool);
        }
    }
}