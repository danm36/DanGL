using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanGL.GLRT
{
    public interface IVertex
    {
        int StructSize { get; }
    }

    public struct ColouredVertex : IVertex
    {
        public const int CVStructSize = (4 + 4) * 4;
        public int StructSize => (4 + 4) * 4;

        private readonly Vector4 position;
        private readonly Color4 color;

        public ColouredVertex(Vector3 position, Color4 color)
        {
            this.position = new Vector4(position, 1.0f);
            this.color = color;
        }
    }

    public struct ColouredVertexNormal : IVertex
    {
        public const int CVStructSize = (4 + 4 + 4) * 4;
        public int StructSize => (4 + 4 + 4) * 4;

        private readonly Vector4 position;
        private readonly Vector4 normal;
        private readonly Color4 color;

        public ColouredVertexNormal(Vector3 position, Vector3 normal, Color4 color)
        {
            this.position = new Vector4(position, 1.0f);
            this.normal = new Vector4(normal, 1.0f);
            this.color = color;
        }
    }
}
