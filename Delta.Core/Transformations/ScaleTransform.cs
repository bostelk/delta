using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Transformations
{
    internal class ScaleTransform : BaseTransform
    {
        static Pool<ScaleTransform> _pool;

        Vector2 _startScale;
        Vector2 _goalScale;

        static ScaleTransform()
        {
            _pool = new Pool<ScaleTransform>(100);
        }

        public ScaleTransform() { }

        public static ScaleTransform Create(TransformableEntity entity, Vector2 goalScale, float duration)
        {
            ScaleTransform transform = _pool.Fetch();
            transform._entity = entity;
            transform._goalScale = goalScale;
            transform.Duration = duration;
            return transform;
        }

        public override void Begin()
        {
            _startScale = _entity.Scale;
        }

        public override void Update(float elapsed)
        {
            PercentFinished = elapsed / Duration;
            Vector2 newScale = Vector2.Zero;
            newScale.X = InterpolationMethod(_startScale.X, _goalScale.X, PercentFinished);
            newScale.Y = InterpolationMethod(_startScale.Y, _goalScale.Y, PercentFinished);
            _entity.Scale = newScale;
        }

        public override void End()
        {
            _entity.Scale = _goalScale;
        }

        public override void Recycle()
        {
            base.Recycle();
            _startScale = Vector2.Zero;
            _goalScale = Vector2.Zero;

            _pool.Release(this);
        }

    }
}
