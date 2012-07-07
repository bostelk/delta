using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    public class Transformer : Entity
    {
        float _elapsed;
        int _repeat;
        TransformableEntity _entity;
        Action _onFinish;
        Queue<ITransform> _transforms;

        Transformer(TransformableEntity entity)
        {
            _entity = entity;
            _transforms = new Queue<ITransform>();
        }

        public static Transformer InWorldThisEntity(TransformableEntity entity)
        {
            Transformer transform = new Transformer(entity);
            G.World.Add(transform);
            return transform;
        }

        /// <summary>
        /// Clear the remainging transforms in the queue.
        /// </summary>
        public void ClearRemaining()
        {
            _transforms.Clear();
        }

        /// <summary>
        /// Skip the current transform.
        /// </summary>
        public void SkipTransform() 
        {
            _transforms.Dequeue();
        }

        /// <summary>
        /// Scale the entity over a period of time.
        /// </summary>
        /// <param name="scale">The goal scale.</param>
        /// <param name="duration">Duration of the transform in seconds.</param>
        /// <returns></returns>
        public Transformer ScaleTo(Vector2 size, float duration)
        {
            ScaleTransform scale = new ScaleTransform(_entity, size, duration);
            _transforms.Enqueue(scale);
            return this;
        }

        /// <summary>
        /// Translate the Entity over a period of time.
        /// </summary>
        /// <param name="position">The goal position.</param>
        /// <param name="duration">Duration of the transform in seconds.</param>
        /// <returns></returns>
        public Transformer TranslateTo(Vector2 position, float duration)
        {
            TranslateTransform translate = new TranslateTransform(_entity, position, duration);
            _transforms.Enqueue(translate);
            return this;
        }

        /// <summary>
        /// Rotate the Entity over a period of time.
        /// </summary>
        /// <param name="angle">The goal angle in degrees.</param>
        /// <param name="duration">Duration of the transform in seconds.</param>
        /// <returns></returns>
        public Transformer RotateTo(float angle, float duration)
        {
            RotateTransform translate = new RotateTransform(_entity, angle, duration);
            _transforms.Enqueue(translate);
            return this;
        }

        /// <summary>
        /// Rotate the Entity over a period of time.
        /// </summary>
        /// <param name="angle">The goal angle in degrees.</param>
        /// <param name="duration">Duration of the transform in seconds.</param>
        /// <returns></returns>
        public Transformer FadeTo(float alpha, float duration)
        {
            FadeTransform translate = new FadeTransform(_entity, alpha, duration);
            _transforms.Enqueue(translate);
            return this;
        }

        /// <summary>
        /// Wait for a period of time until executing the next transform.
        /// </summary>
        /// <param name="duration">Waiting time.</param>
        /// <returns></returns>
        public Transformer WaitFor(float duration)
        {
            return this;
        }

        /// <summary>
        /// Loop the sequence for transforms.
        /// </summary>
        public void Loop()
        {
            _repeat = -1;
        }

        /// <summary>
        /// Repeat the sequence of transforms.
        /// </summary>
        /// <param name="times">Number of times to repeat for.</param>
        public void Repeat(int times)
        {
            _repeat = times * _transforms.Count;
        }

        /// <summary>
        /// Execute the callback once all the transformations have finished.
        /// </summary>
        /// <param name="callback">Function to call.</param>
        public void OnFinished(Action callback)
        {
            _onFinish = callback;
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            if (_transforms.Count > 0)
            {
                ITransform currentTransform = _transforms.Peek();
                if (_elapsed > currentTransform.Duration)
                {
                    ITransform oldTransform = _transforms.Dequeue();
                    if (_repeat < 0)
                    {
                        _transforms.Enqueue(oldTransform);
                    }
                    else if (_repeat > 0)
                    {
                        _transforms.Enqueue(oldTransform);
                        _repeat--;
                    }
                    if (_transforms.Count == 0 && _onFinish != null)
                    {
                        _onFinish();
                    }
                    _elapsed = 0;
                }
                else
                {
                    currentTransform.Update(_elapsed);
                    _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            base.LightUpdate(gameTime);
        }

    }
}
