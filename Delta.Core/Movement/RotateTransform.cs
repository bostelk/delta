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

        public static RotateTransform Create(Entity entity, float goalRotation, float duration)
        {
            RotateTransform transform = _pool.Fetch();
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

        public override void Recycle()
        {
            _startRotation = 0;
            _goalRotation = 0;
            base.Recycle();

        }

    }
}
