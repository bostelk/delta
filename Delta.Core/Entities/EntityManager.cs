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

        internal void Update()
        {
            if (!IsPaused)
            {
                _time.IsRunningSlowly = G._time.IsRunningSlowly;
                _time.ElapsedSeconds = G._time.ElapsedSeconds * TimeScale;
                _time.TotalSeconds += _time.ElapsedSeconds;
                Camera.Update(_time);
                base.InternalUpdate(_time);
            }
        }

        internal void Draw()
        {
            G.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.View);
            base.InternalDraw(_time, G.SpriteBatch);
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
