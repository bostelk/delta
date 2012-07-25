using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Tweakable : Attribute
    {
    }
}
