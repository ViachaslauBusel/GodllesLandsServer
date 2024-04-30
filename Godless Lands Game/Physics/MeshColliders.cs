using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Game.Resources;
using System.Numerics;

namespace Game.Physics
{
    struct MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;
    }
    public class MeshColliders
    {
        private Dictionary<string, MeshData> m_meshColliders = new Dictionary<string, MeshData>();

        public void Load(string file)
        {
            ResourceReader.Read(file, (r) =>
            {
                int meshCount = r.ReadInt32();
                for (int i = 0; i < meshCount; i++)
                {
                    string guid = r.ReadString();
                    if (string.IsNullOrEmpty(guid)) continue;

                    Vector3[] vertices = new Vector3[r.ReadInt32()];
                    for (int v = 0; v < vertices.Length; v++)
                    {
                        vertices[v] = new Vector3();
                        vertices[v].X = r.ReadSingle();
                        vertices[v].Y = r.ReadSingle();
                        vertices[v].Z = r.ReadSingle();
                    }

                    int[] triangles = new int[r.ReadInt32()];
                    for (int t = 0; t < triangles.Length; t++)
                    {
                        triangles[t] = r.ReadInt32();
                    }

                    m_meshColliders.Add(guid, new MeshData { vertices = vertices, triangles = triangles });
                }
            });
        }
        internal bool Create(BufferPool bufferPool, string guid, Vector3 scale, out Mesh mesh)
        {
            if (m_meshColliders.TryGetValue(guid, out MeshData data))
            {
                var vertices = data.vertices;

                bufferPool.Take<Triangle>(data.triangles.Length / 3, out var triangles);
                for (int t = 0; t < triangles.Length; t++)
                {
                    triangles[t].A = vertices[data.triangles[(t * 3) + 2]];
                    triangles[t].B = vertices[data.triangles[(t * 3) + 1]];
                    triangles[t].C = vertices[data.triangles[t * 3]];
                }
                mesh = new Mesh(triangles, scale, bufferPool);
                return true;
            }
            mesh = default;
            return false;
        }
        public void Dispose()
        {
            m_meshColliders.Clear();
        }


    }
}