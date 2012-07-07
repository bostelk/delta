using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    /// <summary>
    /// Transforms manipulate a property of over a period of time. Use a single Transformer to create a sequence of Transforms.
    /// Use additional Transformers to create parallel sequences.
    /// </summary>
    public class Transformer : Entity
    {
        float _elapsed;
        int _repeat;
        TransformableEntity _entity;
        Action _onTransformFinished;
        Action _onSequenceFinished;
        Queue<ITransform> _transforms;

        Transformer(TransformableEntity entity)
        {
            _entity = entity;
            _transforms = new Queue<ITransform>();
        }

        /// <summary>
        /// Start a sequence of Transforms using this Entity.
        /// </summary>
        /// <param name="entity">Entity to transform.</param>
        /// <returns></returns>
        public static Transformer ThisEntity(TransformableEntity entity)
        {
            Transformer transform = new Transformer(entity);
            G.World.Add(transform);
            return transform;
        }

        /// <summary>
        /// Clear the remaining Transforms in the sequence.
        /// </summary>
        public void ClearSequence()
        {
            _transforms.Clear();
        }

        /// <summary>
        /// Skip the current Transform.
        /// </summary>
        public void SkipTransform() 
        {
            _transforms.Dequeue();
        }

        /// <summary>
        /// Add to the sequence of Transforms; Scale the entity over a period of time.
        /// </summary>
        /// <param name="scale">The scale to transition to.</param>
        /// <param name="duration">Time taken to reach the goal scale. (seconds)</param>
        /// <returns></returns>
        public Transformer ScaleTo(Vector2 size, float duration)
        {
            ScaleTransform scale = new ScaleTransform(_entity, size, duration);
            _transforms.Enqueue(scale);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Translate the Entity over a period of time.
        /// </summary>
        /// <param name="position">The position to transition to.</param>
        /// <param name="duration">Time taken to reach the goal position. (seconds)</param>
        /// <returns></returns>
        public Transformer TranslateTo(Vector2 position, float duration)
        {
            TranslateTransform translate = new TranslateTransform(_entity, position, duration);
            _transforms.Enqueue(translate);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Rotate the Entity over a period of time.
        /// </summary>
        /// <param name="angle">The angle to transition to. (degrees)</param>
        /// <param name="duration">Time taken to reach the goal angle. (seconds)</param>
        /// <returns></returns>
        public Transformer RotateTo(float angle, float duration)
        {
            RotateTransform translate = new RotateTransform(_entity, MathHelper.ToRadians(angle), duration);
            _transforms.Enqueue(translate);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Fade the Entity over a period of time.
        /// </summary>
        /// <param name="angle">The alpha to transition to. (percent)</param>
        /// <param name="duration">Time taken to reach the goal alpha. (seconds)</param>
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
        /// Repeat the sequence of transforms indefinitely.
        /// </summary>
        public void Loop()
        {
            _repeat = -1;
        }

        /// <summary>
        /// Repeat the sequence of transforms for a limited number of times.
        /// </summary>
        /// <param name="times">Number of times to repeat for.</param>
        public void Repeat(int times)
        {
            _repeat = times * _transforms.Count;
        }

        /// <summary>
        /// Execute the callback each time a Transform finishes.
        /// </summary>
        /// <param name="callback">Function to call.</param>
        public void OnTransformFinished(Action callback)
        {
            _onTransformFinished = callback;
        }

        /// <summary>
        /// Execute the callback once all the Transforms have finished.
        /// </summary>
        /// <param name="callback">Function to call.</param>
        public void OnSequenceFinished(Action callback)
        {
            _onSequenceFinished = callback;
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            if (_transforms.Count > 0)
            {
                ITransform currentTransform = _transforms.Peek();
                if (_elapsed > currentTransform.Duration)
                {
                    ITransform oldTransform = _transforms.Dequeue();
                    
                    // either loop if -1 or repeat the desired about of times.
                    if (_repeat < 0)
                    {
                        _transforms.Enqueue(oldTransform);
                    }
                    else if (_repeat > 0)
                    {
                        _transforms.Enqueue(oldTransform);
                        _repeat--;
                    }

                    // the transform has finished.
                    if (_onTransformFinished != null)
                        _onTransformFinished();
                    // the transform sequence has finished; no remaining transforms.
                    if (_transforms.Count == 0 && _onSequenceFinished != null)
                        _onSequenceFinished();

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
