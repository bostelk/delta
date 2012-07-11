using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Tiled
{
    public class FilenameContent
    {
        public string FileName { get; private set; }

        public FilenameContent(string fileName)
        {
            FileName = fileName;
        }

    }
}
