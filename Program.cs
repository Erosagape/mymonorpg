using System;

namespace mymonogame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MyMonoGame())
                game.Run();
        }
    }
}
