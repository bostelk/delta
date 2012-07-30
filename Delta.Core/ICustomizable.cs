using System;
using System.ComponentModel;

namespace Delta
{
    internal interface ICustomizable
    {
        string GetValue(string name);
        bool SetValue(string name, string value);
    }
}
