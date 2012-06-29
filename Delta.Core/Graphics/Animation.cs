using System;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Delta.Graphics
{

    public class Animation
    {
        [ContentSerializer] //xml serialization is overriden in Delta.Pipeline.AnimationSerializer, but keep these for binary reading/writing
        public string Name { get; private set; }
        [ContentSerializer]
        public List<int> Frames { get; private set; }
        [ContentSerializer]
        public float FrameDuration { get; private set; }
        [ContentSerializer]
        public string ImageName { get; set; }

        public Animation()
            : base()
        {
            Frames = new List<int>();
        }

        public Animation(string animationName, string frames, float frameDuration)
            : this()
        {
            Name = animationName;
            foreach (string frame in frames.Split(new string[] { ",", ":", ";", " ", "/", "-", "~" }, StringSplitOptions.RemoveEmptyEntries))
                Frames.Add(Convert.ToInt32(frame));
            FrameDuration = frameDuration;
        }

    }

}
