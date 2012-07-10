using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Movement
{
    internal class FlickerTransform : BaseTransform
    {
        static Pool<FlickerTransform> _pool;

        float _minAlpha;
        float _maxAlpha;

        static FlickerTransform() 
        {
            _pool = new Pool<FlickerTransform>(20);
        }

        public FlickerTransform() { }

        public static FlickerTransform Create(TransformableEntity entity, float min, float max, float duration)
        {
            FlickerTransform transform = new FlickerTransform();
            transform._entity = entity;
            transform._minAlpha = MathExtensions.Clamp(min, 0f, max);
            transform._maxAlpha = MathExtensions.Clamp(max, min, 1f);
            transform.Duration = duration;
            return transform;
        }

        public override void Update(float elapsed)
        {
            if (elapsed == 0)
                _entity.Alpha = G.Random.Between(_minAlpha, _maxAlpha);
        }

        public override void Recycle()
        {
            base.Recycle();
            _minAlpha = 0;
            _maxAlpha = 0;

            _pool.Release(this);
        }

    }
}
