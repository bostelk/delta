using System;
using System.Collections.Generic;

namespace Delta
{
    internal class EntityHelper
    {
        internal static Dictionary<string, IEntity> _idReferences = new Dictionary<string, IEntity>();

        internal static void AddIDReference(IEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Name)) //if the ID is null, make it a unique.
                entity.Name = Guid.NewGuid().ToString();
            if (EntityHelper._idReferences.ContainsKey(entity.Name)) //if the ID already exists, append a numerical increment
            {
                for (int x = 1; x < int.MaxValue; x++)
                {
                    string newID = entity.Name + x;
                    if (!EntityHelper._idReferences.ContainsKey(newID))
                    {
                        entity.Name = newID;
                        break;
                    }
                }
            }
            EntityHelper._idReferences.Add(entity.Name.ToLower(), entity);
        }

        internal static void RemoveIDReference(IEntity entity)
        {
            if (EntityHelper._idReferences.ContainsKey(entity.Name.ToLower()))
                EntityHelper._idReferences.Remove(entity.Name);
        }

    }
}
