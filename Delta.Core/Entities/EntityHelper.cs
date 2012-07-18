using System;
using System.Collections.Generic;

namespace Delta
{
    internal class EntityHelper
    {
        internal static Dictionary<string, IEntity> _idReferences = new Dictionary<string, IEntity>();

        internal static void AddIDReference(IEntity entity)
        {
            string id = entity.Name.ToLower();
            if (_idReferences.ContainsKey(id)) //if the ID already exists, append a numerical increment
            {
                for (int x = 1; x < int.MaxValue; x++)
                {
                    string newID = id + x;
                    if (!_idReferences.ContainsKey(newID))
                    {
                        id = newID;
                        break;
                    }
                }
            }
            entity.Name = id;
            _idReferences.Add(id, entity);
        }

        internal static void RemoveIDReference(IEntity entity)
        {
            string id = entity.Name.ToLower();
            if (EntityHelper._idReferences.ContainsKey(id))
                EntityHelper._idReferences.Remove(id);
        }

    }
}
