using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    internal class FlickerTransform : ITransform
    {
        TransformableEntity _entity;
        float _minAlpha;
        float _maxAlpha;
        float _alpha;
        float _elapsed;

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

        public FlickerTransform(TransformableEntity entity, float min, float max, float duration)
        {
            _entity = entity;
            _minAlpha = min;
            _maxAlpha = max;
            Duration = duration;
        }

        public void Update(float elapsed)
        {
            if (elapsed == 0)
                _entity.Alpha = G.Random.Between(_minAlpha, _maxAlpha);
        }

    }
}
