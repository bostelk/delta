using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public class EntityManager<T> : EntityCollection<T>, IEntityManager where T : IEntity
    {
        static DeltaTime _time = new DeltaTime();

        public Camera Camera { get; private set; }
        public float TimeScale { get; set; }
        public DeltaTime Time { get { return _time; } }
        public bool IsPaused { get; set; }

        public EntityManager()
            : base()
        {
            Camera = new Camera();
            TimeScale = 1.0f;
        }

        internal virtual void Update(GameTime gameTime)
        {
            if (!IsPaused)
            {
                _time.IsRunningSlowly = gameTime.IsRunningSlowly;
                _time.ElapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
                _time.TotalSeconds += _time.ElapsedSeconds;
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

    }
}
