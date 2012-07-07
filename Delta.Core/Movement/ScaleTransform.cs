using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    internal class ScaleTransform : ITransform
    {
        TransformableEntity _entity;
        Vector2 _startScale;
        Vector2 _goalScale;

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

        public ScaleTransform(TransformableEntity entity, Vector2 goalScale, float duration)
        {
            _entity = entity;
            _goalScale = goalScale;
            Duration = duration;
        }

        public void Update(float elapsed)
        {
            if (_startScale == Vector2.Zero)
                _startScale = _entity.Scale;
            PercentFinished = elapsed / Duration;
            Vector2 newScale = Vector2.Zero;
            newScale.X = InterpolationMethod(_startScale.X, _goalScale.X, PercentFinished);
            newScale.Y = InterpolationMethod(_startScale.Y, _goalScale.Y, PercentFinished);
            _entity.Scale = newScale;
        }

    }
}
