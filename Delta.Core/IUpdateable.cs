using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta
{
    public interface IUpdateable : ILayerable
    {
        void Update(DeltaTime time);
        void LoadContent();
    }
}
