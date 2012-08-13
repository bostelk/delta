using System;

namespace Delta
{

    public class Poolable : IPoolable
    {

        public Poolable()
            : base()
        {
            Recycle(false);
        }

        public void Recycle()
        {
            Recycle(true);
        }

        protected virtual void Recycle(bool isReleasing)
        {
            if (isReleasing)
                Pool.Release(this);
        }

    }

}
