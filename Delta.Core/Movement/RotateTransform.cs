using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    internal class RotateTransform : ITransform
    {
        TransformableEntity _entity;
        float _startRotation;
        float _goalRotation;

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

        public RotateTransform(TransformableEntity entity, float goalRotation, float duration)
        {
            _entity = entity;
            _goalRotation = goalRotation;
            Duration = duration;
        }

        public void Update(float elapsed)
        {
            if (elapsed == 0)
                _startRotation = _entity.Rotation;
            PercentFinished = elapsed / Duration;
            float newRotation = 0;
            newRotation = InterpolationMethod(_startRotation, _goalRotation, PercentFinished);
            _entity.Rotation = newRotation;
        }

    }
}
