using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Delta;

namespace DeltaContentBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            ContentBuilder contentBuilder = new ContentBuilder(@"../../../Delta.Examples/Delta.ExamplesContent/Delta.ExamplesContent.contentproj");
            contentBuilder.ContentRootDirectory = "Content";
            contentBuilder.CompressContent = true;
            contentBuilder.GraphicsProfile = GraphicsProfile.HiDef;
            contentBuilder.TargetPlatform = TargetPlatform.Windows;
            contentBuilder.Rebuild = true;
            switch (contentBuilder.TargetPlatform)
            {
                case TargetPlatform.Windows:
                    contentBuilder.OutputPath = @"../../Examplebin/Windows/";
                    break;
                case TargetPlatform.Xbox360:
                    contentBuilder.OutputPath = @"../../Examplebin/Xbox360/";
                    break;
            }
            contentBuilder.Build();
            Console.WriteLine("Press the enter key to close.");
            Console.ReadLine();
        }
    }
}
