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

        public override void Begin()
        {
            _startAlpha = _entity.Alpha;
        }

        public override void Update(float elapsed)
        {
            PercentFinished = elapsed / Duration;
            _entity.Alpha = InterpolationMethod(_startAlpha, _goalAlpha, PercentFinished);
        }

        public override void End()
        {
            _entity.Alpha = _goalAlpha;
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
