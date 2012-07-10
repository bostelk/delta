using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Movement
{
    internal class FadeTransform : BaseTransform
    {
        static Pool<FadeTransform> _pool;

        float _startAlpha;
        float _goalAlpha;

        static FadeTransform()
        {
            _pool = new Pool<FadeTransform>(100);
        }

        public static FadeTransform Create(TransformableEntity entity, float goalAlpha, float duration)
        {
            FadeTransform transform = _pool.Fetch();
            transform._entity = entity;
            transform._goalAlpha = MathExtensions.Clamp(goalAlpha, 0f, 1f);
            transform.Duration = duration;
            return transform;
        }

        public override void Update(float elapsed)
        {
            if (elapsed == 0)
                _startAlpha = _entity.Alpha;
            PercentFinished = elapsed / Duration;
            float newAlpha = 0;
            newAlpha = InterpolationMethod(_startAlpha, _goalAlpha, PercentFinished);
            _entity.Alpha = newAlpha;
        }

        public override void Recycle()
        {
            base.Recycle();
            _startAlpha = 0;
            _goalAlpha = 0;

            _pool.Release(this);
        }
    }
}
