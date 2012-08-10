using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Delta.Transformations
{
    internal class FlickerTransform : BaseTransform
    {
   
        float _minAlpha;
        float _maxAlpha;

        public FlickerTransform() { }

        public static FlickerTransform Create(TransformableEntity entity, float min, float max, float duration)
        {
            FlickerTransform transform = Pool.Fetch<FlickerTransform>();
            transform._entity = entity;
            transform._minAlpha = MathExtensions.Clamp(min, 0f, max);
            transform._maxAlpha = MathExtensions.Clamp(max, min, 1f);
            transform.Duration = duration;
            return transform;
        }

        protected override void Recycle(bool isReleasing)
        {
            _minAlpha = 0;
            _maxAlpha = 0;
            base.Recycle(isReleasing);
        }

        public override void Begin()
        {
            _entity.Alpha = G.Random.Between(_minAlpha, _maxAlpha);
        }

    }
}
