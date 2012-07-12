using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Movement
{
    /// <summary>
    /// Transforms manipulate a property of over a period of time. Use a single Transformer to create a sequence of Transforms.
    /// Use additional Transformers to create parallel sequences.
    /// </summary>
    public class Transformer : Entity, IRecyclable
    {
        static Pool<Transformer> _pool;
        float _elapsed;
        int _repeat;
        TransformableEntity _entity;
        Action _onTransformFinished;
        Action _onSequenceFinished;
        Queue<ITransform> _transforms;

        public bool IsPaused { get; private set; }

        static Transformer()
        {
            _pool = new Pool<Transformer>(100);
        }

        public Transformer() 
        { 
            _transforms = new Queue<ITransform>();
        }

        static Transformer Create(TransformableEntity entity)
        {
            Transformer transformer = _pool.Fetch();
            transformer._entity = entity;
            return transformer;
        }

        protected override bool ImportCustomValues(string name, string value)
        {
            return base.ImportCustomValues(name, value);
        }

        /// <summary>
        /// Start a sequence of Transforms using this Entity.
        /// </summary>
        /// <param name="entity">Entity to transform.</param>
        /// <returns></returns>
        public static Transformer ThisEntity(TransformableEntity entity)
        {
            Transformer transform = Create(entity);
            G.World.Add(transform);
            return transform;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Scale the entity over a period of time.
        /// </summary>
        /// <param name="scale">The scale to transition to.</param>
        /// <param name="duration">Time taken to reach the goal scale. (seconds)</param>
        /// <returns></returns>
        public Transformer ScaleTo(Vector2 size, float duration)
        {
            ScaleTransform transfrom = ScaleTransform.Create(_entity, size, duration);
            _transforms.Enqueue(transfrom);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Scale the entity over a period of time.
        /// </summary>
        /// <param name="scale">The scale to transition to.</param>
        /// <param name="duration">Time taken to reach the goal scale. (seconds)</param>
        /// <returns></returns>
        public Transformer ScaleTo(Vector2 size, float duration, Interpolation.InterpolationMethod interpolation)
        {
            ScaleTransform transform = ScaleTransform.Create(_entity, size, duration);
            transform.InterpolationMethod = interpolation;
            _transforms.Enqueue(transform);
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
            TranslateTransform transform = TranslateTransform.Create(_entity, position, duration);
            _transforms.Enqueue(transform);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Translate the Entity over a period of time.
        /// </summary>
        /// <param name="position">The position to transition to.</param>
        /// <param name="duration">Time taken to reach the goal position. (seconds)</param>
        /// <returns></returns>
        public Transformer TranslateTo(Vector2 position, float duration, Interpolation.InterpolationMethod interpolation)
        {
            TranslateTransform transform = TranslateTransform.Create(_entity, position, duration);
            transform.InterpolationMethod = interpolation;
            _transforms.Enqueue(transform);
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
            RotateTransform transform = RotateTransform.Create(_entity, MathHelper.ToRadians(angle), duration);
            _transforms.Enqueue(transform);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Rotate the Entity over a period of time.
        /// </summary>
        /// <param name="angle">The angle to transition to. (degrees)</param>
        /// <param name="duration">Time taken to reach the goal angle. (seconds)</param>
        /// <returns></returns>
        public Transformer RotateTo(float angle, float duration, Interpolation.InterpolationMethod interpolation)
        {
            RotateTransform transform = RotateTransform.Create(_entity, MathHelper.ToRadians(angle), duration);
            transform.InterpolationMethod = interpolation;
            _transforms.Enqueue(transform);
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
            FadeTransform transform = FadeTransform.Create(_entity, alpha, duration);
            _transforms.Enqueue(transform);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Fade the Entity over a period of time.
        /// </summary>
        /// <param name="angle">The alpha to transition to. (percent)</param>
        /// <param name="duration">Time taken to reach the goal alpha. (seconds)</param>
        /// <returns></returns>
        public Transformer FadeTo(float alpha, float duration, Interpolation.InterpolationMethod interpolation)
        {
            FadeTransform transform = FadeTransform.Create(_entity, alpha, duration);
            transform.InterpolationMethod = interpolation;
            _transforms.Enqueue(transform);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Flicker the alpha values between a range over a period of time.
        /// </summary>
        /// <param name="min">The alpha is equal to or above this value. (percent)</param>
        /// <param name="max">The alpha is equal to or below this value. (percent)</param>
        /// <param name="duration">Time to flicker for. (seconds)</param>
        /// <returns></returns>
        public Transformer FlickerFor(float min, float max, float duration)
        {
            FlickerTransform transform = FlickerTransform.Create(_entity, min, max, duration);
            _transforms.Enqueue(transform);
            return this;
        }

        /// <summary>
        /// Add to the sequence of Transforms; Blink the entity between being visible and non-visible over a period of time.
        /// </summary>
        /// <param name="min">Time to stay visisble or non-visible for. (seconds)</param>
        /// <param name="duration">Time to blink for. (seconds)</param>
        /// <returns></returns>
        public Transformer BlinkFor(float rate, float duration)
        {
            BlinkTransform transform = BlinkTransform.Create(_entity, rate, duration);
            _transforms.Enqueue(transform);
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
        /// Resume the Transforms.
        /// </summary>
        public void UnPause()
        {
            IsPaused = false;
        }

        /// <summary>
        /// Pause the Transforms.
        /// </summary>
        public void Pause()
        {
            IsPaused = true;
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
        /// Terminates the sequence; Repeat the sequence of transforms indefinitely.
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
            if (_transforms.Count > 0 && !IsPaused)
            {
                ITransform currentTransform = _transforms.Peek();

                // the transform is either just starting, updating or ending.
                if (_elapsed == 0)
                    currentTransform.Begin();
                if (_elapsed > currentTransform.Duration)
                {
                    ITransform oldTransform = _transforms.Dequeue();
                    oldTransform.End();

                    // either loop if -1 or repeat the desired about of times. or recycle it.
                    if (_repeat < 0)
                    {
                        _transforms.Enqueue(oldTransform);
                    }
                    else if (_repeat > 0)
                    {
                        _transforms.Enqueue(oldTransform);
                        _repeat--;
                    }
                    else if (oldTransform is IRecyclable)
                    {
                        (oldTransform as IRecyclable).Recycle();
                    }

                    // the transform has finished.
                    if (_onTransformFinished != null)
                        _onTransformFinished();
                    // the transform sequence has finished; no remaining transforms.
                    if (_transforms.Count == 0 && _onSequenceFinished != null)
                    {
                        _onSequenceFinished();
                        Recycle();
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

        public void Recycle()
        {
            _elapsed = 0;
            _repeat = 0;
            _entity = null;
            _onTransformFinished = null;
            _onSequenceFinished = null;
            _transforms.Clear();

            RemoveNextUpdate = true;
            _pool.Release(this);
        }
    }
}
