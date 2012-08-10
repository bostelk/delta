using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Delta.Audio
{
    public class Sound1D : Poolable, ISound
    {
        private Cue _cue;

        public string Name
        {
            get;
            private set;
        }

        public Action<string> OnFinished
        {
            get;
            set;
        }

        internal Sound1D() 
            : base()
        { 
        }

        public static Sound1D Create(string name, Cue cue, Action<string> onFinished)
        {
            Sound1D freshSound = Pool.Acquire<Sound1D>();
            freshSound.Name = name;
            freshSound._cue = cue;
            freshSound.OnFinished = onFinished;
            return freshSound;
        }

        protected override void Recycle(bool isReleasing)
        {
            Name = String.Empty;
            OnFinished = null;
            if (isReleasing)
                _cue.Dispose();
            base.Recycle(isReleasing);
        }

        public void Play()
        {
            _cue.Play();
        }

        public void Pause()
        {
            _cue.Pause();
        }

        public void Resume()
        {
            _cue.Resume();
        }
        
        public void Stop()
        {
            _cue.Stop(AudioStopOptions.Immediate);
        }

        public void Stop(AudioStopOptions stopOptions)
        {
            _cue.Stop(stopOptions);
            if (OnFinished != null)
            {
                OnFinished(Name);
            }
        }

        public void SetVolume(float amount) 
        {
            amount = MathHelper.Clamp(amount, 0f, 1f);
            _cue.SetVariable("Volume", amount);
        }

        /// <summary>
        /// On a scale of 0 being slow, 0.5 normal, and 1 fast.
        /// </summary>
        /// <param name="amount"></param>
        public void SetPitch(float amount)
        {
            amount = MathHelper.Clamp(amount, 0f, 1f);
            _cue.SetVariable("Pitch", amount);
        }

        public bool IsPlaying()
        {
            return !_cue.IsStopped && !_cue.IsDisposed && !_cue.IsPaused;
        }

        public bool Update(DeltaGameTime time)
        {
            if (_cue.IsStopped || _cue.IsDisposed)
            {
                return false;
            }
            return true;
        }

    }

}
