using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Movement
{
    internal class BlinkTransform : BaseTransform
    {
        static Pool<BlinkTransform> _pool;
        float _blinkRate;
        float _nextBlink;

        static BlinkTransform()
        {
            _pool = new Pool<BlinkTransform>(20);
        }

        public BlinkTransform() { }

        public static BlinkTransform Create(Entity entity, float rate, float duration)
        {
            BlinkTransform transform = _pool.Fetch();
            transform._entity = entity;
            transform._blinkRate = transform._nextBlink = MathExtensions.Clamp(rate, 0f, duration);
            transform.Duration = duration;
            return transform;
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

        public override void Recycle()
        {
            base.Recycle();
            _blinkRate = 0;
            _nextBlink = 0;

            _pool.Release(this);
        }

    }
}
