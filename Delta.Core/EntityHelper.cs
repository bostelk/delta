using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Delta
{
    internal class EntityHelper
    {
        internal static Dictionary<string, IEntity> _idReferences = new Dictionary<string, IEntity>();
        internal static List<IEntity> _globalEntitiesList = new List<IEntity>();
        internal static ReadOnlyCollection<IEntity> _globalEntities = new ReadOnlyCollection<IEntity>(_globalEntitiesList);

        internal static bool AddReferenceID(IEntity item, string newID)
        {
            if (string.IsNullOrEmpty(newID))
                newID = Guid.NewGuid().ToString();
            if (_idReferences.ContainsKey(newID))
            {
                for (int x = 0; x < int.MaxValue; x++)
                {
                    string offsetID = newID + x;
                    if (!_idReferences.ContainsKey(offsetID))
                    {
                        newID = offsetID;
                        break;
                    }
                }
            }
            _idReferences.Add(newID, item);
            return true;
        }

        internal static void RemoveReferenceID(string id)
        {
            if (_idReferences.ContainsKey(id.ToLower()))
                _idReferences.Remove(id);
        }

        internal static bool ChangeReferenceID(string oldID, string newID)
        {
            IEntity item = Entity.Get(oldID);
            if (item == null)
                return false;
            if (!AddReferenceID(item, newID))
                return false;
            RemoveReferenceID(oldID);
            return true;
        }
    }
}
