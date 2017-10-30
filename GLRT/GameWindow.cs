//#define DEBUG_RENDERING

using DanGL.Engine;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanGL.GLRT
{
    public class GameWindow : OpenTK.GameWindow
    {
        public GameWindow() : base(
#if DEBUG_RENDERING
            1280, 720,
#else
            DGLEngine.Width, DGLEngine.Height, 
#endif
            GraphicsMode.Default, "GL Console Window", OpenTK.GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            VSync = VSyncMode.Adaptive;
        }

        VertexBuffer<ColouredVertexNormal> vbo;
        VertexArray<ColouredVertexNormal> vao;
        ShaderProgram shader;
        Matrix4 projMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 16f / 9, 0.1f, 100f);
        Matrix4 viewMat = Matrix4.LookAt(new Vector3(0.0f, 1.5f, -2.0f), Vector3.Zero, Vector3.UnitY);
        Matrix4 modelMat = Matrix4.Identity;
        RenderTarget rt;
        Color4[] screenData;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            screenData = new Color4[Width * Height];

            GL.Enable(EnableCap.DepthTest);

#if !DEBUG_RENDERING
            rt = new RenderTarget(Width, Height);
#endif
            vbo = new VertexBuffer<ColouredVertexNormal>();
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, 1), new Vector3(0.0f, 0.0f, 1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, 1), new Vector3(0.0f, 0.0f, 1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, 1), new Vector3(0.0f, 0.0f, 1.0f), Color4.Blue));

            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, 1), new Vector3(0.0f, 0.0f, 1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, 1), new Vector3(0.0f, 0.0f, 1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, 1), new Vector3(0.0f, 0.0f, 1.0f), Color4.Blue));


            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, -1), new Vector3(0.0f, 0.0f, -1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, -1), new Vector3(0.0f, 0.0f, -1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, -1), new Vector3(0.0f, 0.0f, -1.0f), Color4.Blue));

            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, -1), new Vector3(0.0f, 0.0f, -1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, -1), new Vector3(0.0f, 0.0f, -1.0f), Color4.Blue));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, -1), new Vector3(0.0f, 0.0f, -1.0f), Color4.Blue));


            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, -1), new Vector3(1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, 1), new Vector3(1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, 1), new Vector3(1.0f, 0.0f, 0.0f), Color4.Red));

            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, -1), new Vector3(1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, 1), new Vector3(1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, -1), new Vector3(1.0f, 0.0f, 0.0f), Color4.Red));


            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, -1), new Vector3(-1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, 1), new Vector3(-1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, 1), new Vector3(-1.0f, 0.0f, 0.0f), Color4.Red));

            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, -1), new Vector3(-1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, 1), new Vector3(-1.0f, 0.0f, 0.0f), Color4.Red));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, -1), new Vector3(-1.0f, 0.0f, 0.0f), Color4.Red));


            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, -1), new Vector3(0.0f, -1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, 1), new Vector3(0.0f, -1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, 1), new Vector3(0.0f, -1.0f, 0.0f), Color4.Lime));

            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, -1, -1), new Vector3(0.0f, -1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, 1), new Vector3(0.0f, -1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, -1, -1), new Vector3(0.0f, -1.0f, 0.0f), Color4.Lime));


            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, -1), new Vector3(0.0f, 1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, 1), new Vector3(0.0f, 1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, 1), new Vector3(0.0f, 1.0f, 0.0f), Color4.Lime));

            vbo.AddVertex(new ColouredVertexNormal(new Vector3(-1, 1, -1), new Vector3(0.0f, 1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, 1), new Vector3(0.0f, 1.0f, 0.0f), Color4.Lime));
            vbo.AddVertex(new ColouredVertexNormal(new Vector3(1, 1, -1), new Vector3(0.0f, 1.0f, 0.0f), Color4.Lime));

            Shader vertShader = new Shader(ShaderType.VertexShader, @"
# version 130

// attributes of our vertex
in vec4 vPosition;
in vec4 vNormal;
in vec4 vColor;

// a projection transformation to apply to the vertex' position
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

out vec4 fColor; // must match name in fragment shader
out vec4 vWorldPos;
out vec4 vWorldNormal;

void main()
{
    // gl_Position is a special variable of OpenGL that must be set
    vWorldPos = modelMatrix * vPosition;
    gl_Position = projectionMatrix * viewMatrix * vWorldPos;
    vWorldNormal = modelMatrix * vNormal;
    //gl_Position = vec4(vPosition.xy, 0, 1.0);
    fColor = vColor;
}");
            Shader fragShader = new Shader(ShaderType.FragmentShader, @"

# version 130

in vec4 fColor; // must match name in vertex shader
in vec4 vWorldPos;
in vec4 vWorldNormal; // must match name in vertex shader

out vec4 fragColor; // first out variable is automatically written to the screen

uniform float elapsedTime;

void main()
{
    float intensity = max(0.0, 1.0 - distance(vec3(1.5, 1.5, -1.5), vWorldPos.xyz) / 2.5);
    //intensity = 1.0;
    fragColor = fColor * intensity;
    //fragColor.b = sin(elapsedTime) * 0.5 + 0.5;
}");
            shader = new ShaderProgram(vertShader, fragShader);

            vao = new VertexArray<ColouredVertexNormal>(vbo, shader,
                new VertexAttribute("vPosition", 4, VertexAttribPointerType.Float, ColouredVertexNormal.CVStructSize, 0),
                new VertexAttribute("vNormal", 4, VertexAttribPointerType.Float, ColouredVertexNormal.CVStructSize, 16),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, ColouredVertexNormal.CVStructSize, 32));
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            DGLEngine.Instance.Update((float)e.Time);
            modelMat = Matrix4.CreateRotationY(DGLEngine.ElapsedTime);
            //viewMat = Matrix4.LookAt(new Vector3(0.0f, (float)(Math.Sin(CGLEngine.ElapsedTime) * 1.5), -3.0f), Vector3.Zero, Vector3.UnitY);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

#if !DEBUG_RENDERING
            rt.Bind(Width, Height);
#endif
            shader.Use();
            GL.UniformMatrix4(shader.GetUniformLocation("projectionMatrix"), false, ref projMat);
            GL.UniformMatrix4(shader.GetUniformLocation("viewMatrix"), false, ref viewMat);
            GL.UniformMatrix4(shader.GetUniformLocation("modelMatrix"), false, ref modelMat);
            GL.Uniform1(shader.GetUniformLocation("elapsedTime"), DGLEngine.ElapsedTime);

            vbo.Bind();
            vao.Bind();

            vbo.BufferData();
            vbo.Draw();

            SwapBuffers();

#if !DEBUG_RENDERING
            GL.ReadPixels(0, 0, Width, Height, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, screenData);
            DGLEngine.Instance.Draw(screenData);
#endif
        }
    }
}
