using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Transformations
{
    internal class ScaleTransform : BaseTransform
    {
        Vector2 _startScale;
        Vector2 _goalScale;

        public ScaleTransform() { }

        public static ScaleTransform Create(TransformableEntity entity, Vector2 goalScale, float duration)
        {
            ScaleTransform transform = Pool.Fetch<ScaleTransform>();
            transform._entity = entity;
            transform._goalScale = goalScale;
            transform.Duration = duration;
            return transform;
        }

        protected override void Recycle(bool isReleasing)
        {
            _startScale = Vector2.Zero;
            _goalScale = Vector2.Zero;
            base.Recycle(isReleasing);
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

    }
}
