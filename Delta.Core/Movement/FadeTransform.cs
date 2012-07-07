using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    internal class FadeTransform : ITransform
    {
        TransformableEntity _entity;
        float _startAlpha;
        float _goalAlpha;

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

        public FadeTransform(TransformableEntity entity, float goalAlpha, float duration)
        {
            _entity = entity;
            _goalAlpha = goalAlpha;
            Duration = duration;
        }

        public void Update(float elapsed)
        {
            if (_startAlpha == 0)
                _startAlpha = _entity.Alpha;
            PercentFinished = elapsed / Duration;
            float newAlpha = 0;
            newAlpha = InterpolationMethod(_startAlpha, _goalAlpha, PercentFinished);
            _entity.Alpha = newAlpha;
        }

    }
}
