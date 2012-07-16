using System;
using System.Collections.Generic;

namespace Delta
{
    internal class EntityHelper
    {
        internal static Dictionary<string, IEntity> _idReferences = new Dictionary<string, IEntity>();

        internal static void AddIDReference(IEntity entity)
        {
            if (string.IsNullOrEmpty(entity.ID)) //if the ID is null, make it a unique.
                entity.ID = Guid.NewGuid().ToString();
            if (EntityHelper._idReferences.ContainsKey(entity.ID)) //if the ID already exists, append a numerical increment
            {
                for (int x = 1; x < int.MaxValue; x++)
                {
                    string newID = entity.ID + x;
                    if (!EntityHelper._idReferences.ContainsKey(newID))
                    {
                        entity.ID = newID;
                        break;
                    }
                }
            }
            EntityHelper._idReferences.Add(entity.ID.ToLower(), entity);
        }

        internal static void RemoveIDReference(IEntity entity)
        {
            if (EntityHelper._idReferences.ContainsKey(entity.ID.ToLower()))
                EntityHelper._idReferences.Remove(entity.ID);
        }

    }
}
