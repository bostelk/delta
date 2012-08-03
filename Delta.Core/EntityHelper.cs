using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta
{
    internal class EntityHelper
    {
        internal static Dictionary<string, Entity> _idReferences = new Dictionary<string, Entity>();

        internal static Entity Get(string name)
        {
            name = name.ToLower();
            if (EntityHelper._idReferences.ContainsKey(name))
                return EntityHelper._idReferences[name];
            return null;
        }

        internal static void AddIDReference(Entity entity)
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

        internal static void RemoveIDReference(Entity entity)
        {
            string id = entity.Name.ToLower();
            if (EntityHelper._idReferences.ContainsKey(id))
                EntityHelper._idReferences.Remove(id);
        }
    }
}
