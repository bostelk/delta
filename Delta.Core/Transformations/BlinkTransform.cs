using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Transformations
{
    internal class BlinkTransform : BaseTransform
    {
        float _blinkRate;
        float _nextBlink;

        public BlinkTransform() { }

        public static BlinkTransform Create(TransformableEntity entity, float rate, float duration)
        {
            BlinkTransform transform = Pool.Fetch<BlinkTransform>();
            transform._entity = entity;
            transform._blinkRate = transform._nextBlink = MathExtensions.Clamp(rate, 0f, duration);
            transform.Duration = duration;
            return transform;
        }

        protected override void Recycle(bool isReleasing)
        {
            _blinkRate = 0;
            _nextBlink = 0;
            base.Recycle(isReleasing);
        }

        public override void Update(float elapsed)
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
