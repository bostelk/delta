using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Delta
{
    public interface IEntityParent : IEntity
    {
        bool NeedsToSort { get; set; }
        bool Add(IEntity entity);
        bool Remove(IEntity entity);
    }
}
