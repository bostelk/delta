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
    public class World : DeltaGameComponentCollection<IGameComponent>
    {
        static DeltaTime _time = new DeltaTime();

        public Camera Camera { get; private set; }
        public float TimeScale { get; set; }
        public DeltaTime Time { get { return _time; } }
        public IGameComponentCollection BelowGround { get; set; }
        public IGameComponentCollection Ground { get; set; }
        public IGameComponentCollection AboveGround { get; set; }
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
                base.InternalUpdate(_time);
            }
        }

        internal virtual void Draw()
        {
            G.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.View);
            base.Draw(_time, G.SpriteBatch);
            G.SpriteBatch.End();

            G.SpriteBatch.Begin();
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
        
        public string ToDebugString()
        {
            StringBuilder text = new StringBuilder();
            text.Append(String.Format("{0,-60}{1,2}\n", "Name", "Layer"));
            foreach (IGameComponent gameComponent in _components)
                text.Append(gameComponent.ToString());
            return text.ToString();
        }

        public void DebugDraw()
        {
            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawString(G.Font, this.ToDebugString(), new Vector2(0, 100), Color.White);
            G.SpriteBatch.End();
        }

    }
}
