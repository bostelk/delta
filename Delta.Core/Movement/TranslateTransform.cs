using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Movement
{
    internal class TranslateTransform : BaseTransform
    {
        static Pool<TranslateTransform> _pool;

        Vector2 _goalPosition;
        Vector2 _startPosition;

        static TranslateTransform()
        {
            _pool = new Pool<TranslateTransform>(200);
        }

        public TranslateTransform() { }
        
        public static TranslateTransform Create(TransformableEntity entity, Vector2 goalPosition, float duration) {
            TranslateTransform transform = _pool.Fetch();
            transform._entity = entity;
            transform._goalPosition = goalPosition;
            transform.Duration = duration;
            return transform;
        }

        public override void Update(float elapsed)
        {
            if (elapsed == 0)
                _startPosition = _entity.Position;
            PercentFinished =  elapsed / Duration;
            Vector2 newPosition = Vector2.Zero;
            newPosition.X = InterpolationMethod(_startPosition.X, _goalPosition.X, PercentFinished);
            newPosition.Y = InterpolationMethod(_startPosition.Y, _goalPosition.Y, PercentFinished);
            _entity.Position = newPosition;
        }

        public override void Recycle()
        { 
            base.Recycle();            
            _goalPosition = Vector2.Zero;
            _startPosition = Vector2.Zero;
            _pool.Release(this);
        }

    }
}
