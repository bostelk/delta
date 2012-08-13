using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Transformations
{
    internal class FadeTransform : BaseTransform
    {
        float _startAlpha;
        float _goalAlpha;

        public static FadeTransform Create(TransformableEntity entity, float goalAlpha, float duration)
        {
            FadeTransform transform = Pool.Acquire<FadeTransform>();
            transform._entity = entity;
            transform._goalAlpha = MathExtensions.Clamp(goalAlpha, 0f, 1f);
            transform.Duration = duration;
            return transform;
        }

        protected override void Recycle(bool isReleasing)
        {
            _startAlpha = 0;
            _goalAlpha = 0;
            base.Recycle(isReleasing);
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
    }
}
