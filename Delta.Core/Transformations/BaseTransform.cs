using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Transformations
{
    public abstract class BaseTransform : Poolable, ITransform
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

        protected override void Recycle(bool isReleasing)
        {
            _entity = null;
            Duration = 0;
            PercentFinished = 0;
            InterpolationMethod = MathHelper.Lerp;
            base.Recycle(isReleasing);
        }

        public Interpolation.InterpolationMethod InterpolationMethod = Interpolation.Linear;

        public virtual void Begin() { }
        public virtual void Update(float seconds) { }
        public virtual void End() { }
    }
}
