using System;

namespace Delta.Tiled
{
    public interface ILayer: IEntity
    {
        float Parallax { get; set; }
    }
}
