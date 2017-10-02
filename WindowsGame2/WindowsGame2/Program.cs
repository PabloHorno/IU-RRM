using System;

namespace VentanaRender
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Render game = new Render())
            {
                game.Run();
            }
        }
    }
#endif
}

