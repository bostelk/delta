using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta
{
    public interface IEntityParent : IEntity
    {
        bool NeedsToSort { get; set; }
    }
}
