using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        string _contentProject = string.Empty;

        public LoggerVerbosity LoggerVerbosity { get; set; }
        public string OutputPath { get; set; }
        public string ContentRootDirectory { get; set; }
        public GraphicsProfile GraphicsProfile { get; set; }
        public TargetPlatform TargetPlatform { get; set; }
        public bool CompressContent { get; set; }

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

        public ContentBuilder(string contentProjectFile)
        {
            FileInfo fi = new FileInfo(contentProjectFile);
            if (fi.Extension != ".contentproj")
                throw new NotSupportedException(string.Format("The file '{0}' is not a XNA C# content project.", contentProjectFile));
            if (!fi.Exists)
                throw new FileNotFoundException(String.Format("The file '{0}' does not exist.", contentProjectFile), contentProjectFile);
            _contentProject = Path.GetFullPath(contentProjectFile);
            LoggerVerbosity = LoggerVerbosity.Normal;
            GraphicsProfile = GraphicsProfile.HiDef;
            TargetPlatform = TargetPlatform.Windows;
            CompressContent = false;
        }

        public bool Build()
        {
            string originalWorkingDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.GetDirectoryName(_contentProject);
            var globalProperties = new Dictionary<string, string>();
            globalProperties.Add("XnaProfile", GraphicsProfile.ToString());
            globalProperties.Add("XNAContentPipelineTargetPlatform", TargetPlatform.ToString());
            globalProperties.Add("XnaCompressContent", CompressContent.ToString());
            globalProperties.Add("OutputPath", OutputPath);
            globalProperties.Add("ContentRootDirectory", ContentRootDirectory);
            //globalProperties.Add("CustomAfterMicrosoftCommonTargets", DebuggingTargets);
            var project = ProjectCollection.GlobalProjectCollection.LoadProject(_contentProject, globalProperties, "4.0");
            bool r = project.Build(new ConsoleLogger(LoggerVerbosity));
            Environment.CurrentDirectory = originalWorkingDirectory;
            return r;
        }

    }
}
