using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public class EntityCollection : IDrawable, IUpdateable, ILayerable
    {
        internal static Dictionary<string, Entity> _idReferences = new Dictionary<string, Entity>();
        internal bool NeedsToSort { get; set; }

        public float Layer { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<IUpdateable> _updateables = new List<IUpdateable>();
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<IDrawable> _drawables = new List<IDrawable>();

        public IComparer<ILayerable> Comparer { get; set; }

        //[ContentSerializerIgnore]
        //public Rectangle ViewingArea { get; set; }

        public EntityCollection()
            : base()
        {
            NeedsToSort = true;
            Comparer = new ILayerableComparer();
        }

        public void Add(IUpdateable updateable)
        {
            _updateables.Add(updateable);
            NeedsToSort = true;
        }

        public void Add(IDrawable drawable)
        {
            _drawables.Add(drawable);
            NeedsToSort = true;
        }

        public void Add(Entity entity)
        {
            _updateables.Add(entity);
            _drawables.Add(entity);
            NeedsToSort = true;
            entity._collectionReference = this;
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
            _updateables.AddRange(collection._updateables);
            _drawables.AddRange(collection._drawables);
           NeedsToSort = true;
        }

        public void Remove(IUpdateable updateable)
        {
            _updateables.FastRemove<IUpdateable>(updateable);
            NeedsToSort = true;
        }

        public void Remove(IDrawable drawable)
        {
            _drawables.FastRemove<IDrawable>(drawable);
            NeedsToSort = true;
        }

        public void Remove(Entity entity)
        {
            _updateables.FastRemove<IUpdateable>(entity);
            _drawables.FastRemove<IDrawable>(entity);
            NeedsToSort = true;
            if (_idReferences.ContainsKey(entity.ID.ToLower()))
                _idReferences.Remove(entity.ID);
        }

        public void Remove(EntityCollection collection)
        {
            for (int x = 0; x < collection._updateables.Count; x++)
                _updateables.FastRemove<IUpdateable>(collection._updateables[x]);
            for (int x = 0; x < collection._drawables.Count; x++)
                _drawables.FastRemove<IDrawable>(collection._drawables[x]);
        }

        public virtual void LoadContent()
        {
            for (int x = 0; x < _updateables.Count; x++)
                _updateables[x].LoadContent();
        }

        void IUpdateable.Update(DeltaTime time)
        {
            Update(time);
        }

        public void Update(DeltaTime time)
        {
            if (NeedsToSort)
                Sort();
            for (int x = 0; x < _updateables.Count; x++)
                _updateables[x].Update(time);
        }

        public void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < _drawables.Count; x++)
                _drawables[x].Draw(time, spriteBatch);
        }

        protected virtual void Sort()
        {
            _updateables.Sort(Comparer);
            _drawables.Sort(Comparer);
            NeedsToSort = false;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityCollectionReader : ContentTypeReader<EntityCollection>
    {
        protected override EntityCollection Read(ContentReader input, EntityCollection value)
        {
            if (value == null)
                value = new EntityCollection();
            value._updateables = input.ReadObject<List<IUpdateable>>();
            value._drawables = input.ReadObject<List<IDrawable>>();
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
