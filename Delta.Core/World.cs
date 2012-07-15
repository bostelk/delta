using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Input;

namespace Delta
{
    public class World : EntityCollection
    {
        static DeltaTime _time = new DeltaTime();

        public Camera Camera { get; private set; }
        public float TimeScale { get; set; }
        public DeltaTime Time { get { return _time; } }
        public EntityCollection BelowGround { get; set; }
        public EntityCollection Ground { get; set; }
        public EntityCollection AboveGround { get; set; }
        public bool IsPaused { get; private set; }

        public World()
        {
            Camera = new Camera();
            TimeScale = 1.0f;
        }

        internal void Update(GameTime gameTime)
        {
            if (!IsPaused)
            {
                _time.IsRunningSlowly = gameTime.IsRunningSlowly;
                _time.ElapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
                _time.TotalSeconds += _time.ElapsedSeconds;
                Camera.Update(_time);
                base.Update(_time);
            }
        }

        internal virtual void Draw()
        {
            G.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.View);
            base.Draw(_time, G.SpriteBatch);
            Camera.Draw(_time, G.SpriteBatch);
            G.SpriteBatch.End();
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
        }

        public bool SecondsPast(float seconds)
        {
            return _time.TotalSeconds > seconds;
        }

        public List<T> GetEntitiesUnderMouse<T>() where T: Entity
        {
            List<T> result = new List<T>();
            //foreach (Entity entity in Entity.GlobalEntities)
            //{
            //    T entityT = entity as T;
            //    if (entityT == null) continue;
            //    if ((entityT.Parent == null || !entityT.Parent.IsVisible) && !entityT.IsVisible) continue;
            //    Rectangle hitbox = new Rectangle((int)entityT.Position.X, (int)entityT.Position.Y, (int)(entityT.Size.X * entityT.Scale.X), (int)(entityT.Size.Y * entityT.Scale.Y));
            //    if (hitbox.Contains(G.World.Camera.ToWorldPosition(G.Input.Mouse.Position).ToPoint()))
            //        result.Add(entityT);
            //}
            return result;
        }

    }
}
