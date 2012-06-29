using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Movement
{
    internal class FadeTransform : ITransform
    {
        public float Duration { get; set; }

        public FadeTransform(Entity entity, float duration)
        {
        }

        public void Update(float elapsed)
        {
        }
    }
}
