using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanGL.GLRT
{
    public class VertexBuffer<TVertex> where TVertex : struct, IVertex
    {
        private TVertex[] vertices = new TVertex[4];

        private int count;

        private readonly int handle;

        public VertexBuffer()
        {
            handle = GL.GenBuffer();
        }

        public void AddVertex(TVertex v)
        {
            if (count == vertices.Length)
                Array.Resize(ref vertices, count * 2);

            vertices[count] = v;
            ++count;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
        }

        public void BufferData()
        {
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices[0].StructSize * count), vertices, BufferUsageHint.StreamDraw);
        }

        public void Draw()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, count);
        }
    }
}
