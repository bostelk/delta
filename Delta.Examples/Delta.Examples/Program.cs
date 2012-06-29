using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Delta;
using System.Diagnostics;

namespace Delta.Examples
{
#if WINDOWS || XBOX
    static class Program
    {
        enum Examples
        {
            Input,
            Animation,
            Game,
            Audio,
            Tiled,
            Physics,
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if WINDOWS
        [STAThread]
#endif
        static void Main(string[] args)
        {
            //Scripting.Load(@"../../../Scripts/TestScript.cs", "Test").RunScript();

            Examples example = Examples.Physics;

            switch (example)
            {
                case Examples.Input:
                    RunExample<InputExample>();
                    break;
                case Examples.Animation:
                    RunExample<AnimationExample>();
                    break;
                case Examples.Game:
                    RunExample<GameExample>();
                    break;
                case Examples.Audio:
                    RunExample<AudioExample>();
                    break;
                case Examples.Tiled:
                    RunExample<TiledExample>();
                    break;
                case Examples.Physics:
                    RunExample<PhysicsExample>();
                    break;
            }
        }

        static void RunExample<T>() where T: DeltaGame, new()
        {
            using (T game = new T())
            {
                game.Run();
            }
        }
    }
#endif
}

