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
    public class World : EntityParent<IEntity>
    {
        static DeltaGameTime _time = new DeltaGameTime();

        public Camera Camera { get; private set; }
        public float TimeScale { get; set; }
        public DeltaGameTime Time { get { return _time; } }
        public bool IsPaused { get; set; }

        public IEntityCollection BelowGround { get; set; }
        public IEntityCollection Ground { get; set; }
        public IEntityCollection AboveGround { get; set; }

        public World()
            : base()
        {
            Camera = new Camera();
            TimeScale = 1.0f;
        }

        protected override bool CanUpdate()
        {
            if (IsPaused)
                return false;
            return base.CanUpdate();
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            _time.ElapsedSeconds = G._time.ElapsedSeconds * TimeScale;
            _time.TotalSeconds += _time.ElapsedSeconds;
            Camera.Update(_time);
            base.LightUpdate(time);
        }

        protected override void OnBeginDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.OnBeginDraw(time, spriteBatch);
            G.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.View);
        }

        protected override void OnEndDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.OnEndDraw(time, spriteBatch);
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
