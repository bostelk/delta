using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Delta.Audio
{
    public class Sound3D : Poolable, ISound
    {
        private Cue _cue;
        private TransformableEntity _source;
        private TransformableEntity _dest;
        private AudioEmitter _audioEmitter;
        private AudioListener _audioListener;

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

        internal Sound3D()
        {
            _audioEmitter = new AudioEmitter();
            _audioListener = new AudioListener();
        }

        public static Sound3D Create(string name, Cue cue, TransformableEntity source, TransformableEntity dest, Action<string> onFinished)
        {
            Sound3D freshSound = Pool.Fetch<Sound3D>();
            freshSound.Name = name;
            freshSound._cue = cue;
            freshSound._source = source;
            freshSound._dest = dest;
            freshSound.OnFinished = onFinished;
            return freshSound;
        }

        protected override void Recycle(bool isReleasing)
        {
            Name = String.Empty;
            OnFinished = null;
            _source = null;
            _dest = null;
            _audioEmitter = null;
            _audioListener = null;
            if (isReleasing)
                _cue.Dispose();
            base.Recycle(isReleasing);
        }

        private Vector2 CalculateForward(Vector2 source, Vector2 dest)
        {
            Vector2 value = source - dest;
            if (value == Vector2.Zero)
            {
                return Vector2.Zero;
            }
            float amount = MathHelper.Min(value.LengthSquared() /  MathExtensions.Square((float) G.ScreenArea.Width / 2.0f), 1.0f);
            value.Normalize();
            return Vector2.Lerp(value, Vector2.UnitY, amount);
        }

        private void Apply3D()
        {
            Vector2 newSource = _source.Position * 5; // needs to fall off faster, this is easiest for now.
            Vector2 value = CalculateForward(newSource, _dest.Position);
            _audioEmitter.Position = new Vector3(newSource, 0f);
            _audioListener.Position = new Vector3(_dest.Position, 0f);
            _audioListener.Forward = new Vector3(value, 0f);
            _audioListener.Up = -Vector3.Forward;
            _cue.Apply3D(_audioListener, _audioEmitter);
        }

        public void Play()
        {
            Apply3D();
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

        public bool IsPlaying()
        {
            return !_cue.IsStopped && !_cue.IsDisposed && !_cue.IsPaused;
        }

        public void SetVolume(float amount) 
        {
            _cue.SetVariable("Volume", amount);
        }

        public void SetPitch(float amount)
        {
            _cue.SetVariable("Pitch", amount);
        }


        public bool Update(DeltaGameTime time)
        {
            if (_cue.IsStopped || _cue.IsDisposed)
            {
                return false;
            }
            Apply3D();
            return true;
        }
    }

}
