using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Content;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityParentReader<T> : ContentTypeReader<EntityParent<T>> where T : IEntity
    {
        protected override EntityParent<T> Read(ContentReader input, EntityParent<T> value)
        {
            if (value == null)
                value = new EntityParent<T>();

            input.ReadRawObject<Entity>(value as Entity);
            int count = input.ReadObject<int>();
            for (int x = 0; x < count; x++)
                value.Add(input.ReadObject<T>());

            return value;
        }
    }
}
