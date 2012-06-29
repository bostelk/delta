using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Movement
{
    internal class StallTransform : ITransform
    {
        public float Duration { get; set; }

        public StallTransform(Entity entity, float duration)
        {
        }

        public void Update(float elapsed)
        {
        }
    }
}
