using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Movement
{
    public abstract class BaseTransform : ITransform, IRecyclable
    {
        protected TransformableEntity _entity;

        /// <summary>
        /// Length of the transform in seconds.
        /// </summary>
        public float Duration
        {
            get;
            protected set;
        }

        public float PercentFinished
        {
            get;
            protected set;
        }

        public Func<float, float, float, float> InterpolationMethod = MathHelper.Lerp;

        public virtual void Update(float seconds) { }

        public virtual void Recycle()
        {
            _entity = null;
            Duration = 0;
            PercentFinished = 0;
            InterpolationMethod = MathHelper.Lerp;
        }
    }
}
