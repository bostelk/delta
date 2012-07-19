using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Delta.Structures;

namespace Delta.Audio
{
    public class Sound1D : ISound, IRecyclable
    {
        private static Pool<Sound1D> _pool;

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

        static Sound1D()
        {
            _pool = new Pool<Sound1D>();
        }

        public Sound1D() { }

        public static Sound1D Create(string name, Cue cue, Action<string> onFinished)
        {
            Sound1D freshSound = _pool.Fetch();
            freshSound.Name = name;
            freshSound._cue = cue;
            freshSound.OnFinished = onFinished;
            return freshSound;
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

        public bool Update(DeltaTime time)
        {
            if (_cue.IsStopped || _cue.IsDisposed)
            {
                return false;
            }
            return true;
        }

        public void Recycle()
        {
            Name = String.Empty;
            OnFinished = null;
            _cue.Dispose();

            _pool.Release(this);
        }

    }

}
