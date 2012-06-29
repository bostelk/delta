using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    internal class TranslateTransform : ITransform
    {
        Entity _entity;
        Vector2 _goalPosition;
        Vector2 _startPosition;

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

        public TranslateTransform(Entity entity, Vector2 goalPosition, float duration)
        {
            _entity = entity;
            _goalPosition = goalPosition;
            Duration = duration;
        }

        public void Update(float elapsed)
        {
            if (_startPosition == Vector2.Zero)
                _startPosition = _entity.Position;
            PercentFinished =  elapsed / Duration;
            Vector2 newPosition = Vector2.Zero;
            newPosition.X = InterpolationMethod(_startPosition.X, _goalPosition.X, PercentFinished);
            newPosition.Y = InterpolationMethod(_startPosition.Y, _goalPosition.Y, PercentFinished);
            _entity.Position = newPosition;
        }

    }
}
