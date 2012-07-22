//using System.Collections.Generic;
//using System.Drawing;
//using System.Text;
//using Microsoft.DirectX.Direct3D;
//using Font=Microsoft.DirectX.Direct3D.Font;

//namespace AvengersUTD.Odyssey.UserInterface.Helpers
//{
//    public delegate double FPSDelegate();

//    /// <summary>
//    /// Descrizione di riepilogo per DebugManager.
//    /// </summary>
//    public class DebugManager
//    {
//        Font font;
//        Sprite textSprite;
//        const int messageLimit = 5;
//        static DebugManager singletonDM;

//        string deviceStats = string.Empty;
//        string frameStats = string.Empty;
//        Queue<string> messageQueue = new Queue<string>(messageLimit);
//        StringBuilder stringBuffer = new StringBuilder();
//        string debugInfo = string.Empty;
//        Timer timer;

//        double lastStatsUpdateTime; // last time the stats were updated
//        uint lastStatsUpdateFrames; // frames count since last time the stats were updated
//        double frameRate; // frames per second
//        //int currentFrameNumber; // the current frame number

//        static FPSDelegate fpsDelegate;


//        DebugManager()
//        {
//            font = new Font(
//                UI.Device,
//                20,
//                0,
//                FontWeight.Bold,
//                1,
//                false,
//                CharacterSet.Default,
//                Precision.Default,
//                FontQuality.Default,
//                PitchAndFamily.FamilyDoNotCare, "Courier New"
//                );
//            textSprite = new Sprite(UI.Device);
//            timer = new Timer();
//        }

//        public static DebugManager Instance
//        {
//            get
//            {
//                if (singletonDM == null)
//                {
//                    singletonDM = new DebugManager();
//                    return singletonDM;
//                }
//                else
//                    return singletonDM;
//            }
//        }

//        public string DeviceInfo
//        {
//            get { return deviceStats; }
//        }

//        public static string FrameStats
//        {
//            get
//            {
//                Instance.UpdateFrameStats(fpsDelegate());
//                return Instance.frameStats;
//            }
//        }

//        public void DisplayStats()
//        {
//            TextManager txtManager = new TextManager(font, 15);

//            // Output statistics
//            //txtManager.Begin();
//            UI.CurrentHud.SpriteManager.Begin(SpriteFlags.AlphaBlend);
//            txtManager.SetInsertionPoint(200, 5);
//            txtManager.SetForegroundColor(Color.Yellow);
//            txtManager.DrawTextLine(FrameStats);
//            txtManager.DrawTextLine(deviceStats);
//            txtManager.DrawTextLine(debugInfo);
//            UI.CurrentHud.SpriteManager.End();
//            //txtManager.SetForegroundColor(Color.White);
//            //txtManager.End();
//        }

//        void CheckBounds()
//        {
//            if (messageQueue.Count > messageLimit)
//            {
//                messageQueue.Dequeue();
//            }

//            stringBuffer = new StringBuilder();
//            foreach (string s in messageQueue.ToArray())
//            {
//                stringBuffer.AppendLine(s);
//            }
//            debugInfo = stringBuffer.ToString();
//        }


//        public static void LogError(string method, string text, string id)
//        {
//            DebugManager dm = Instance;
//            dm.messageQueue.Enqueue(string.Format("Error in {0}: {1} ({2})", method, text, id));
//            dm.CheckBounds();
//        }

//        public static void LogToScreen(string text)
//        {
//            DebugManager dm = Instance;
//            dm.messageQueue.Enqueue(text);
//            dm.CheckBounds();
//        }

//        /// <summary>
//        /// Updates the static part of the frame stats so it doesn't have be generated every frame
//        /// </summary>
//        public static void UpdateStaticStats(string deviceInfo)
//        {
//            Instance.deviceStats = deviceInfo;
//        }

//        public static void SetFPSDelegate(FPSDelegate fpsDel)
//        {
//            fpsDelegate = fpsDel;
//        }

//        /// <summary>
//        /// Updates the frames/sec stat once per second
//        /// </summary>
//        void UpdateFrameStats(double framerate)
//        {
//            double time = timer.GetAbsoluteTime();
//            lastStatsUpdateFrames++;

//            if (time - lastStatsUpdateTime > 1.0)
//            {
//                float fps = (float) (lastStatsUpdateFrames/(time - lastStatsUpdateTime));
////				currentFrameRate = fps;
//                lastStatsUpdateFrames = 0;
//                lastStatsUpdateTime = time;

//                frameStats = "FPS: " +
//                             fps.ToString("f2") + " FrameTime: " + framerate.ToString("f6");
//            }
//        }
//    }
//}