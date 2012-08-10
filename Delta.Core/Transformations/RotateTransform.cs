using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Transformations
{
    internal class RotateTransform : BaseTransform
    {
        float _startRotation;
        float _goalRotation;

        public RotateTransform() { }

        public static RotateTransform Create(TransformableEntity entity, float goalRotation, float duration)
        {
            RotateTransform transform = Pool.Fetch<RotateTransform>();
            transform._entity = entity;
            transform._goalRotation = goalRotation;
            transform.Duration = duration;
            return transform;
        }

        public override void Begin()
        {
            _startRotation = _entity.Rotation;
        }

        public override void Update(float elapsed)
        {
            PercentFinished = elapsed / Duration;
            _entity.Rotation = InterpolationMethod(_startRotation, _goalRotation, PercentFinished);
        }

        public override void End()
        {
            _entity.Rotation = _goalRotation;
        }

        protected override void Recycle(bool isReleasing)
        {
            _startRotation = 0;
            _goalRotation = 0;
            base.Recycle(isReleasing);
        }

    }
}
