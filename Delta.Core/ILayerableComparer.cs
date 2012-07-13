using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ILayerableComparer : Comparer<ILayerable>
    {
        public override int Compare(ILayerable x, ILayerable y)
        {
            return x.Layer.CompareTo(y.Layer);
            //if (x.MajorLayer > y.MajorLayer)
            //    return int.MaxValue;
            //else if (x.MajorLayer < y.MajorLayer)
            //    return int.MinValue;
            //return x.MinorLayer.CompareTo(y.MinorLayer);
        }
    }
}
