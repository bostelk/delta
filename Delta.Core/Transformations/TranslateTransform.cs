using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Transformations
{
    internal class TranslateTransform : BaseTransform
    {
        Vector2 _goalPosition;
        Vector2 _startPosition;

        public TranslateTransform() { }
        
        public static TranslateTransform Create(TransformableEntity entity, Vector2 goalPosition, float duration)
        {
            TranslateTransform transform = Pool.Fetch<TranslateTransform>();
            transform._entity = entity;
            transform._goalPosition = goalPosition;
            transform.Duration = duration;
            return transform;
        }

        protected override void Recycle(bool isReleasing)
        {
            _goalPosition = Vector2.Zero;
            _startPosition = Vector2.Zero;
            base.Recycle(isReleasing);
        }

        public override void Begin()
        {
            _startPosition = _entity.Position;
        }

        public override void Update(float elapsed)
        {
            PercentFinished =  elapsed / Duration;
            Vector2 newPosition = Vector2.Zero;
            newPosition.X = InterpolationMethod(_startPosition.X, _goalPosition.X, PercentFinished);
            newPosition.Y = InterpolationMethod(_startPosition.Y, _goalPosition.Y, PercentFinished);
            _entity.Position = newPosition;
        }

        public override void End()
        {
            _entity.Position = _goalPosition;
        }

    }
}
