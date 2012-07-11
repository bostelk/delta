﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Movement
{
    internal class EmptyTransform : ITransform
    {
        public float Duration { get; set; }

        public EmptyTransform(Entity entity, float duration)
        {
        }

        public void Begin() { }
        public void End() { }
        public void Update(float elapsed) { }
    }
}