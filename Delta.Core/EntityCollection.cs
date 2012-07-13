using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityCollection : List<EntityBase>
    {
        [ContentSerializerIgnore]
        internal static Dictionary<string, Entity> _idReferences = new Dictionary<string, Entity>();
        [ContentSerializerIgnore]
        internal bool NeedsToSort { get; set; }

        IComparer<EntityBase> _comparer = new DefaultEntityComparer();
        [ContentSerializerIgnore]
        internal IComparer<EntityBase> Comparer
        {
            get { return _comparer; }
            set
            {
                if (_comparer != value)
                {
                    _comparer = value;
                    NeedsToSort = true;
                }
            }
        }

        [ContentSerializerIgnore]
        public Rectangle ViewingArea { get; set; }

        public EntityCollection()
            : base()
        {
            NeedsToSort = true;
        }

        internal void InternalAdd(EntityBase entity)
        {
            base.Add(entity);
            NeedsToSort = true;
            entity._collectionReference = this;
        }

        public void Add(Entity entity)
        {
            this.InternalAdd(entity);
            if (string.IsNullOrEmpty(entity.ID)) //if the ID is null, make it a unique.
                entity.ID = Guid.NewGuid().ToString();
            if (_idReferences.ContainsKey(entity.ID)) //if the ID already exists, append a numerical increment
            {
                for (int x = 1; x < int.MaxValue; x++)
                {
                    string newID = entity.ID + x;
                    if (!_idReferences.ContainsKey(newID))
                    {
                        entity.ID = newID;
                        break;
                    }
                }
            }
            _idReferences.Add(entity.ID.ToLower(), entity);
        }

        public void Add(EntityCollection collection)
        {
            foreach (EntityBase entity in collection)
                Add(entity);
        }

        internal void InternalRemove(EntityBase entity)
        {
            this.FastRemove<EntityBase>(entity);
            NeedsToSort = true;
        }

        public void Remove(Entity entity)
        {
            InternalRemove(entity);
            if (_idReferences.ContainsKey(entity.ID.ToLower()))
                _idReferences.Remove(entity.ID);
        }

        public void Remove(EntityCollection collection)
        {
            foreach (EntityBase entity in collection)
                Remove(entity);
        }

        public virtual void LoadContent()
        {
            for (int x = 0; x < base.Count; x++)
                base[x].LoadContent();
        }

        internal void Update(DeltaTime time)
        {
            if (NeedsToSort)
                Sort();
            for (int x = 0; x < base.Count; x++)
                base[x].InternalUpdate(time);
        }

        internal void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < base.Count; x++)
                base[x].InternalDraw(time, spriteBatch);
        }

        protected new virtual void Sort()
        {
            base.Sort(_comparer);
            NeedsToSort = false;
        }

        internal class DefaultEntityComparer : Comparer<EntityBase>
        {
            public override int Compare(EntityBase x, EntityBase y)
            {
                return x.MajorLayer.CompareTo(y.MajorLayer);
                //if (x.MajorLayer > y.MajorLayer)
                //    return int.MaxValue;
                //else if (x.MajorLayer < y.MajorLayer)
                //    return int.MinValue;
                //return x.MinorLayer.CompareTo(y.MinorLayer);
            }
        }

    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityCollectionReader : ContentTypeReader<EntityCollection>
    {
        protected override EntityCollection Read(ContentReader input, EntityCollection value)
        {
            if (value == null)
                value = new EntityCollection();
            int count = input.ReadObject<int>();
            for (int x = 0; x < count; x++)
                value.Add(input.ReadObject<EntityBase>());
            return value;
        }
    }

    public static class EntitySpawner
    {
        /// <summary>
        /// Spawn Entities around the circumference of a circle at set intervals.
        /// </summary>
        /// <typeparam name="T">The Entity type to spawn.</typeparam>
        /// <param name="origin">The origin of the circle.</param>
        /// <param name="radius">The raidus of the circle.</param>
        /// <param name="interval">In degrees.</param>
        /// <param name="spawn">The Entity Constructor.</param>
        /// <returns>A List of positioned Entities.</returns>
        public static List<T> InACircle<T>(Vector2 origin, float radius, float interval, Func<T> spawn) where T : Entity
        {
            radius = MathHelper.Max(0, radius);
            interval = MathHelper.Clamp(interval, 0, 360);
            List<T> result = new List<T>();
            for (float angle = 0; angle < 360; angle += interval)
            {
                Vector2 spawnPosition = Vector2.Zero;
                spawnPosition.X = (float)Math.Cos(MathHelper.ToRadians(angle)) * radius;
                spawnPosition.Y = (float)Math.Sin(MathHelper.ToRadians(angle)) * radius;
                Entity entity = spawn();
                entity.Position = spawnPosition;
                result.Add(entity as T);
            }
            return result;
        }

        /// <summary>
        /// Spawn Entities along a line at set intervals.
        /// </summary>
        /// <typeparam name="T">The Entity to spawn.</typeparam>
        /// <param name="start">The start of the line.</param>
        /// <param name="end">The end of the line.</param>
        /// <param name="interval">The seperating distance between each Entity.</param>
        /// <param name="spawn">The Entity Constructor.</param>
        /// <returns>A list of positioned Entities</returns>
        public static List<T> OnALine<T>(Vector2 start, Vector2 end, float interval, Func<T> spawn) where T : Entity
        {
            float distance = Vector2.Distance(start, end);
            distance = MathHelper.Clamp(interval, 0, distance);
            List<T> result = new List<T>();
            Vector2 direction = end - start;
            direction.Normalize();
            for (float i = 0; i < distance; i += interval)
            {
                Entity entity = spawn();
                entity.Position = start + i * direction;
                result.Add(entity as T);
            }
            return result;
        }

        /// <summary>
        /// Spawn Entities randomly displaced by an offset.
        /// </summary>
        /// <typeparam name="T">The Entity to spawn.</typeparam>
        /// <param name="origin">The start of the line.</param>
        /// <param name="minDistance">The minimum distance from the origin.</param>
        /// <param name="maxDistance">The maximum distance from the origin.</param>
        /// <param name="spawn">The Entity Constructor.</param>
        /// <returns>A list of positioned Entities.</returns>
        public static List<T> Randomly<T>(Vector2 origin, float minDistance, float maxDistance, Func<T> spawn) where T : Entity
        {
            List<T> result = new List<T>();
            Entity entity = spawn();
            entity.Position = origin + new Vector2(G.Random.Between(minDistance, maxDistance), G.Random.Between(minDistance, maxDistance));
            result.Add(entity as T);
            return result;
        }

        /// <summary>
        /// Spawn Entities in a grid
        /// </summary>
        /// <typeparam name="T">Type of Entity to spawn.</typeparam>
        /// <param name="origin">The center of the grid.</param>
        /// <param name="rows">The number of rows in the grid.</param>
        /// <param name="columns">The number of columns in the grid.</param>
        /// <param name="rowSpace">The space between rows.</param>
        /// <param name="columnSpace">The space between columns</param>
        /// <param name="spawn">The Entity Constructor.</param>
        /// <returns>A list of positioned entities.</returns>
        public static List<T> OnAGrid<T>(Vector2 origin, int rows, int columns, float rowSpace, float columnSpace, Func<T> spawn) where T : Entity
        {
            float height = rows * rowSpace; float width = columns * columnSpace;
            List<T> result = new List<T>();
            for (float y = origin.Y - height / 2; y < origin.Y + height / 2; y += rowSpace)
            {
                for (float x = origin.X - width / 2; x < origin.X + width / 2; x += columnSpace)
                {
                    Vector2 spawnPosition = new Vector2(x, y);
                    Entity entity = spawn();
                    entity.Position = spawnPosition;
                    result.Add(entity as T);
                }
            }
            return result;
        }
    }

}
