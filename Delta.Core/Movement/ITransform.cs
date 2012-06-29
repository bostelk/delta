using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Movement
{
    public interface ITransform
    {
        float Duration { get; }
        void Update(float elapsed);
    }
}
