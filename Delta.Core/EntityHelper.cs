using System;
using System.Collections.Generic;

namespace Delta
{
    internal class EntityHelper
    {
        internal static Dictionary<string, Entity> _entityIDReferences = new Dictionary<string, Entity>();
        internal static List<Entity> _globalEntityReferences = new List<Entity>();

        internal static Entity Get(string name)
        {
            name = name.ToLower();
            if (EntityHelper._entityIDReferences.ContainsKey(name))
                return EntityHelper._entityIDReferences[name];
            return null;
        }

        internal static void AddIDReference(Entity entity)
        {
            string id = entity.Name.ToLower();
            if (_entityIDReferences.ContainsKey(id)) //if the ID already exists, append a numerical increment
            {
                for (int x = 1; x < int.MaxValue; x++)
                {
                    string newID = id + x;
                    if (!_entityIDReferences.ContainsKey(newID))
                    {
                        id = newID;
                        break;
                    }
                }
            }
            entity.Name = id;
            _entityIDReferences.Add(id, entity);
        }

        internal static void RemoveIDReference(Entity entity)
        {
            string id = entity.Name.ToLower();
            if (EntityHelper._entityIDReferences.ContainsKey(id))
                EntityHelper._entityIDReferences.Remove(id);
        }
    }
}
