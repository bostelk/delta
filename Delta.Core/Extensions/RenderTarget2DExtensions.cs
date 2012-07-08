using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Reflection;

namespace Delta.Extensions
{
    public static class RenderTarget2DExtensions
    {
        public static void SaveAsPng(this RenderTarget2D target)
        {
            string filename = DateTime.Now.ToString("dd.MM-h.mm.sst") + ".png";
            using(FileStream fs = new FileStream(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename), FileMode.Create)) 
            {
                target.SaveAsPng(fs, target.Width, target.Height);
            };
        }
    }
}
