using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    internal class BlinkTransform : ITransform
    {
        TransformableEntity _entity;
        float _blinkRate;
        float _nextBlink;

        public float SecondsLeft
        {
            get;
            set;
        }

        public float Duration
        {
            get;
            private set;
        }

        public Func<float, float, float, float> InterpolationMethod = MathHelper.Lerp;

        public float PercentFinished
        {
            get;
            private set;
        }

        public BlinkTransform(TransformableEntity entity, float rate, float duration)
        {
            _entity = entity;
            _blinkRate = rate;
            _nextBlink = rate;
            Duration = duration;
        }

        public void Update(float elapsed)
        {
            if (elapsed >= _nextBlink)
            {
                _entity.Alpha = 1;
                _nextBlink = _blinkRate + elapsed;
            }
            else
                _entity.Alpha = 0;
        }

    }
}
