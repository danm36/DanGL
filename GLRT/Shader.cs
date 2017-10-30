using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanGL.GLRT
{
    public sealed class Shader
    {
        public int Handle { get; private set; }

        public Shader(ShaderType type, string code)
        {
            Handle = GL.CreateShader(type);

            GL.ShaderSource(Handle, code);
            GL.CompileShader(Handle);

            int status;
            GL.GetShader(Handle, ShaderParameter.CompileStatus, out status);
            if (status == 0)
            {
                throw new GraphicsException($"Error compiling {type.ToString()} shader: {GL.GetShaderInfoLog(Handle)}");
            }
        }
    }

    public sealed class ShaderProgram
    {
        public int Handle { get; private set; }

        public ShaderProgram(params Shader[] shaders)
        {
            Handle = GL.CreateProgram();

            foreach (var shader in shaders)
                GL.AttachShader(Handle, shader.Handle);

            GL.LinkProgram(Handle);

            foreach (var shader in shaders)
                GL.DetachShader(Handle, shader.Handle);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(Handle, name);
        }

        public int GetUniformLocation(string name)
        {
            // get the location of a uniform variable
            return GL.GetUniformLocation(Handle, name);
        }
    }
}
