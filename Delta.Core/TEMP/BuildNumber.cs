//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Reflection;

//namespace Delta
//{
//    public struct BuildNumber
//    {
//        /// <summary>
//        /// Incremented when there are significant jumps in functionality.
//        /// </summary>
//        public int Major { get; private set; }
        
//        /// <summary>
//        /// Incremented when only minor features or significant fixes have been added.
//        /// </summary>
//        public int Minor  get; private set; }

//        /// <summary>
//        /// Incremented when minor bugs are fixed.
//        /// </summary>
//        public int Revision;

//        /// <summary>
//        /// The build number will be specified by the supplied assembly, or by the executing assembly if null.
//        /// </summary>
//        public BuildNumber(Assembly assembly)
//        {
//            if (assembly == null)
//                assembly = Assembly.GetExecutingAssembly();
//            Major = assembly.GetName().Version.Major;
//            Minor = assembly.GetName().Version.Minor;
//            Revision = assembly.GetName().Version.Revision;
//        }

//        /// <summary>
//        /// Explicitly define the build number.
//        /// </summary>
//        /// <param name="major"></param>
//        /// <param name="minor"></param>
//        /// <param name="revision"></param>
//        public BuildNumber(int major, int minor, int revision)
//        {
//            if (major < 0)
//            {
//                throw new ArgumentOutOfRangeException("must be positive");
//            }
//            if (minor < 0)
//            {
//                throw new ArgumentOutOfRangeException("must be positive");
//            }
//            if (revision < 0)
//            {
//                throw new ArgumentOutOfRangeException("must be positive");
//            }

//            Major = major;
//            Minor = minor;
//            Revision = revision;
//        }

//        public override string ToString()
//        {
//            return String.Format("{0}.{1}.{2}", Major, Minor, Revision);
//        }
//    }
//}
