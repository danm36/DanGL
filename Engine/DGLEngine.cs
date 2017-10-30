using Microsoft.Win32.SafeHandles;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanGL.Engine
{
    public class DGLEngine
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static float ElapsedTime { get; private set; } = 0.0f;

        public static DGLEngine Instance { get; private set; }

        SafeFileHandle consoleHandle = DGLInterop.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        DGLInterop.CharInfo[] consoleBuffer;
        DGLInterop.SmallRect outputRect;

        public DGLEngine()
        {
            Instance = this;

            Console.CursorVisible = false;
            Width = Console.BufferWidth = Console.WindowWidth = 164;
            Height = Console.BufferHeight = Console.WindowHeight = 48;
            consoleBuffer = new DGLInterop.CharInfo[Width * Height];
            outputRect = new DGLInterop.SmallRect() { Left = 0, Top = 0, Right = (short)Width, Bottom = (short)Height };
        }

        public void Update(float delta)
        {
            ElapsedTime += delta;
        }

        public void Draw(Color4[] screenData)
        {
            int i = 0;
            for(int y = Height - 1; y >= 0; --y)
            {
                for(int x = 0; x < Width; ++x, ++i)
                {
                    SetColorCharInfo(ref consoleBuffer[i], screenData[y * Width + x]);
                }
            }
            DGLInterop.WriteConsoleOutput(consoleHandle, consoleBuffer, new DGLInterop.Coord() { X = (short)Width, Y = (short)Height }, new DGLInterop.Coord() { X = 0, Y = 0 }, ref outputRect);
        }


        static Dictionary<ConsoleColor, Color4> colorMappings = new Dictionary<ConsoleColor, Color4>()
        {
            [ConsoleColor.Black] = new Color4(0, 0, 0, 255),
            [ConsoleColor.DarkBlue] = new Color4(0, 0, 128, 255),
            [ConsoleColor.DarkGreen] = new Color4(0, 128, 0, 255),
            [ConsoleColor.DarkCyan] = new Color4(0, 128, 128, 255),
            [ConsoleColor.DarkRed] = new Color4(128, 0, 0, 255),
            [ConsoleColor.DarkMagenta] = new Color4(128, 0, 128, 255),
            [ConsoleColor.DarkYellow] = new Color4(128, 128, 0, 255),
            [ConsoleColor.Gray] = new Color4(128, 128, 128, 255),
            [ConsoleColor.DarkGray] = new Color4(196, 196, 196, 255),
            [ConsoleColor.Blue] = new Color4(0, 0, 255, 255),
            [ConsoleColor.Green] = new Color4(0, 255, 0, 255),
            [ConsoleColor.Cyan] = new Color4(0, 255, 255, 255),
            [ConsoleColor.Red] = new Color4(255, 0, 0, 255),
            [ConsoleColor.Magenta] = new Color4(255, 0, 255, 255),
            [ConsoleColor.Yellow] = new Color4(255, 255, 0, 255),
            [ConsoleColor.White] = new Color4(255, 255, 255, 255),
        };
        public void SetColorCharInfo(ref DGLInterop.CharInfo ci, Color4 col)
        {
            ConsoleColor colA = colorMappings.OrderBy(kvp => ColorDist(kvp.Value, col)).First().Key;
            ConsoleColor colB = colorMappings.Where(kvp => kvp.Key != colA).OrderBy(kvp => ColorDist(kvp.Value, col)).First().Key;
            float distA = ColorDist(colorMappings[colA], col);
            float distB = ColorDist(colorMappings[colB], col);
            if (distA == 0.0f || distB > 0.16f)
                colB = colA;

            ci.Attributes = (short)((int)colA << 4 | (int)colB);
            ci.Char.AsciiChar = 177;
        }

        private float ColorDist(Color4 a, Color4 b)
        {
            float dR = b.R - a.R;
            float dG = b.G - a.G;
            float dB = b.B - a.B;
            return dR * dR + dG * dG + dB * dB;
        }
    }
}
