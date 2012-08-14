using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public class ContentBuilder
    {
        static Dictionary<string, string> _globalProperties = new Dictionary<string, string>();

        string _contentProjectFile = string.Empty;

        public string OutputPath { get; set; }
        public string ContentRootDirectory { get; set; }
        public LoggerVerbosity LoggerVerbosity { get; set; }
        public GraphicsProfile GraphicsProfile { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
        public bool CompressContent { get; set; }
        public bool RebuildContent { get; set; }

        public ContentBuilder(string contentProjectFile, GraphicsProfile graphicsProfile = GraphicsProfile.HiDef, TargetPlatform targetPlatform = TargetPlatform.Windows, bool compressContent = true, LoggerVerbosity loggerVerbosity = LoggerVerbosity.Normal, bool rebuildContent = false)
        {
            FileInfo fileInfo = new FileInfo(contentProjectFile);
            _contentProjectFile = Path.GetFullPath(fileInfo.FullName);
            if (fileInfo.Extension != ".contentproj")
                throw new NotSupportedException(string.Format("The file '{0}' is not a XNA content project.", _contentProjectFile));
            if (!fileInfo.Exists)
                throw new FileNotFoundException(String.Format("The file '{0}' does not exist.", _contentProjectFile, _contentProjectFile);
            GraphicsProfile = graphicsProfile;
            TargetPlatform = targetPlatform;
            CompressContent = compressContent;
            LoggerVerbosity = loggerVerbosity;
            RebuildContent = rebuildContent;
            if (!_globalProperties.ContainsKey("XnaProfile"))
                _globalProperties.Add("XnaProfile", GraphicsProfile.ToString());
            if (!_globalProperties.ContainsKey("XNAContentPipelineTargetPlatform"))
                _globalProperties.Add("XNAContentPipelineTargetPlatform", TargetPlatform.ToString());
            if (!_globalProperties.ContainsKey("XnaCompressContent"))
                _globalProperties.Add("XnaCompressContent", CompressContent.ToString());
            if (!_globalProperties.ContainsKey("OutputhPath"))
                _globalProperties.Add("OutputPath", OutputPath);
            if (!_globalProperties.ContainsKey("ContentRootDirectory"))
                _globalProperties.Add("ContentRootDirectory", ContentRootDirectory);
        }

        public bool Build()
        {
            string originalWorkingDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.GetDirectoryName(_contentProjectFile);
            _globalProperties["XnaProfile"] = GraphicsProfile.ToString();
            _globalProperties["XNAContentPipelineTargetPlatform"] = TargetPlatform.ToString();
            _globalProperties["XnaCompressContent"] = CompressContent.ToString();
            _globalProperties["OutputPath"] = OutputPath;
            _globalProperties["ContentRootDirectory"] = ContentRootDirectory;
            var project = ProjectCollection.GlobalProjectCollection.LoadProject(_contentProjectFile, _globalProperties, "4.0");
            ConsoleLogger logger = new ConsoleLogger(LoggerVerbosity);
            bool buildSucceeded = false;
            if (!RebuildContent)
                buildSucceeded = project.Build(logger);
            else
                buildSucceeded = project.Build("rebuild", new ILogger[] { logger });
            Environment.CurrentDirectory = originalWorkingDirectory;
            return buildSucceeded;
        }

    }
}
