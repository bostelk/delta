using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Movement
{
    internal class RotateTransform : BaseTransform
    {
        static Pool<RotateTransform> _pool;

        float _startRotation;
        float _goalRotation;

        static RotateTransform()
        {
            _pool = new Pool<RotateTransform>(100);
        }

        public RotateTransform() { }

        public static RotateTransform Create(TransformableEntity entity, float goalRotation, float duration)
        {
            RotateTransform transform = _pool.Fetch();
            transform._entity = entity;
            transform._goalRotation = goalRotation;
            transform.Duration = duration;
            return transform;
        }

        public override void Update(float elapsed)
        {
            if (elapsed == 0)
                _startRotation = _entity.Rotation;
            PercentFinished = elapsed / Duration;
            float newRotation = 0;
            newRotation = InterpolationMethod(_startRotation, _goalRotation, PercentFinished);
            _entity.Rotation = newRotation;
        }

        public override void Recycle()
        {
            _startRotation = 0;
            _goalRotation = 0;
            base.Recycle();

        }

    }
}
