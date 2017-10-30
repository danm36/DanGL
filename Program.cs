using DanGL.Engine;
using DanGL.GLRT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanGL
{
    class Program
    {
        static void Main(string[] args)
        {
            DGLEngine engine = new DGLEngine();

            GameWindow gw = new GameWindow();
            gw.Run();
        }
    }
}
