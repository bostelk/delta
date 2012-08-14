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
        string _contentProjectFile = string.Empty;

        public string OutputPath { get; set; }
        public string ContentRootDirectory { get; set; }
        public LoggerVerbosity LoggerVerbosity { get; set; }
        public GraphicsProfile GraphicsProfile { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
        public bool CompressContent { get; set; }
        public bool RebuildContent { get; set; }

        //TODO: Have fancy smuff happen with targets file.
        //public static string DebuggingTargets
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(SingleItem))
        //        {
        //            return String.Empty;
        //        }

        //        string targetsPath = @"Targets\Debugging.targets";
        //        return Path.Combine(BuildToolDirectory, targetsPath);
        //    }
        //}

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
        }

        public bool Build()
        {
            string originalWorkingDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.GetDirectoryName(_contentProjectFile);
            var globalProperties = new Dictionary<string, string>();
            globalProperties.Add("XnaProfile", GraphicsProfile.ToString());
            globalProperties.Add("XNAContentPipelineTargetPlatform", TargetPlatform.ToString());
            globalProperties.Add("XnaCompressContent", CompressContent.ToString());
            globalProperties.Add("OutputPath", OutputPath);
            globalProperties.Add("ContentRootDirectory", ContentRootDirectory);
            //globalProperties.Add("CustomAfterMicrosoftCommonTargets", DebuggingTargets);
            var project = ProjectCollection.GlobalProjectCollection.LoadProject(_contentProjectFile, globalProperties, "4.0");
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
