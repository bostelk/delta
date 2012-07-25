using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Delta.Audio
{

    public enum AudioChannel
    {
        Music,
        Sfx,
        Count
    }

    /// <summary>
    /// TODO: do we want to queue sounds while the audio engine is starting up?
    /// </summary>
    public class AudioManager
    {
        const string MUSIC_CATEGORY = "Music";
        const string SFX_CATEGORY = "Sfx";

        WaveBank _waveBank;
        AudioEngine _audioEngine;
        WaveBank _streamingBank;
        SoundBank _soundBank;

         AudioManager _instance;

        TransformableEntity _dest3D;

        List<ISound> _sounds;
        List<ISound> _soundsToRemove;

        float[] _channelVolume;
        bool[] _channelMuted;
        float[] _channelGoalVolume;
        float[] _channelFadeDuration;
        float[] _channelFadeElapsed;
        bool[] _channelFading;

        // when is it safe to grab this? for now we'll grab and cache at runtime.
        AudioCategory[] __channelCategory;
        AudioCategory[] _channelCategory
        {
            get
            {
                if (__channelCategory == null)
                {
                    __channelCategory = new AudioCategory[] { 
                        _audioEngine.GetCategory(MUSIC_CATEGORY),
                        _audioEngine.GetCategory(SFX_CATEGORY),
                    };
                }
                return __channelCategory;
            }
        }

        public TransformableEntity Listener
        {
            get
            {
                return _dest3D;
            }
            set
            {
                _dest3D = value;
            }
        }

        // shortcut
        public float MusicVolume
        {
            get { return _channelVolume[(int)AudioChannel.Music]; }
        }

        // shortcut
        public float SfxVolume
        {
            get { return _channelVolume[(int)AudioChannel.Sfx]; }
        }

        public bool IsMusicMuted
        {
            get { return _channelMuted[(int)AudioChannel.Music]; }
        }

        public bool IsSfxMuted
        {
            get { return _channelMuted[(int)AudioChannel.Sfx]; }
        }

        public bool IsMuted
        {
            get
            {
                for (int i = 0; i < (int)AudioChannel.Count; i++)
                {
                    if (!_channelMuted[i])
                        return false;
                }
                return true;
            }
        }

        public bool ReadyToRock
        {
            get
            {
                return _audioEngine != null && _waveBank.IsPrepared && _streamingBank.IsPrepared;
            }
        }
        public AudioManager(string settingsFile, string soundbankFile, string waveBankFile, string streamingBankFile)
        {
            if (_instance != null)
            {
                throw new Exception("Audio Manager already created");
            }

            try
            {
                _audioEngine = new AudioEngine(settingsFile);
                _soundBank = new SoundBank(_audioEngine, soundbankFile);
                _waveBank = new WaveBank(_audioEngine, waveBankFile);
                _streamingBank = new WaveBank(_audioEngine, streamingBankFile, 0, 16);
            }
            catch
            {
            }

            _sounds = new List<ISound>(100);
            _soundsToRemove = new List<ISound>(10);

            _channelVolume = new float[(int)AudioChannel.Count];
            _channelMuted = new bool[(int)AudioChannel.Count];
            _channelGoalVolume = new float[(int)AudioChannel.Count];
            _channelFadeElapsed = new float[(int)AudioChannel.Count];
            _channelFadeDuration = new float[(int)AudioChannel.Count];
            _channelFading = new bool[(int)AudioChannel.Count];

            _instance = this;
        }

        public  void PlaySound(string soundName)
        {
            PlaySound(soundName, false, null);
        }

        public  void PlaySound(string soundName, bool force)
        {
            PlaySound(soundName, force, null);
        }

        public  void PlaySound(string soundName, bool force, Action<string> onFinished)
        {
            if (!_waveBank.IsPrepared || !_streamingBank.IsPrepared)
            {
                throw new InvalidOperationException("AudioManager is not prepared; update it some more.");
            }
            if (!IsPlaying(soundName) || force)
            {
                Sound1D freshSound = Sound1D.Create(soundName, _soundBank.GetCue(soundName), onFinished);
                freshSound.Play();
                _sounds.Add(freshSound);
            }
        }

        public  void PlayPositionalSound(string soundName, TransformableEntity source)
        {
            PlayPositionalSound(soundName, false, source, null);
        }

        public  void PlayPositionalSound(string soundName, bool force, TransformableEntity source, Action<string> onFinished)
        {
            if (!_waveBank.IsPrepared || !_streamingBank.IsPrepared)
            {
                throw new InvalidOperationException("AudioManager is not prepared; update it some more.");
            }
            // play the sound if not already playing, otherwise force play another instance of the sound
            if (!IsPlaying(soundName) || force)
            {
                TransformableEntity dest = Listener;
                Sound3D freshSound = Sound3D.Create(soundName, _soundBank.GetCue(soundName), source, dest, onFinished);
                freshSound.Play();

                _sounds.Add(freshSound);
            }
        }

        private  ISound FindSound(string soundName)
        {
            ISound result = null;
            foreach (ISound sound in _sounds)
            {
                if (sound.Name == soundName)
                {
                    result = sound;
                    break;
                }
            }
            return result;
        }

        public void PauseSound(string soundName)
        {
            ISound sound = FindSound(soundName);
            if (sound != null)
                sound.Pause();
        }

        public void ResumeSound(string soundName)
        {
            ISound sound = FindSound(soundName);
            if (sound != null)
                sound.Resume();
        }

        public void StopSound(string soundName)
        {
            ISound sound = FindSound(soundName);
            if (sound != null)
                sound.Stop();
        }

        public bool IsPlaying(string soundName)
        {
            ISound sound = FindSound(soundName);
            if (sound == null)
            {
                return false;
            }
            else
            {
                return sound.IsPlaying();
            }
        }

        public void StopAllSounds()
        {
            foreach (ISound sound in _sounds)
            {
                sound.Stop();
            }
        }

        public void PauseAllSounds()
        {
            foreach (ISound sound in _sounds)
            {
                sound.Pause();
            }
        }

        public void ResumeAllSounds()
        {
            foreach (ISound sound in _sounds)
            {
                sound.Resume();
            }
        }

        /// <summary>
        /// Fades the music channel to full volume over a period of time.
        /// </summary>
        /// <param name="duration"></param>
        public void FadeChannelIn(AudioChannel channel, float duration)
        {
            FadeChannelByAmount(channel, duration, 1f);
        }

        /// <summary>
        /// Fades the music channel to no volume over a period of time.
        /// </summary>
        /// <param name="duration"></param>
        public void FadeChannelOut(AudioChannel channel, float duration)
        {
            FadeChannelByAmount(channel, duration, 0f);
        }

        public void FadeChannelByAmount(AudioChannel channel, float duration, float amount)
        {
            _channelGoalVolume[(int)channel] = MathHelper.Clamp(amount, 0f, 1f);
            _channelFadeDuration[(int)channel] = duration;
            _channelFading[(int)channel] = true;
        }

        public void PauseChannel(AudioChannel channel) 
        {
            _channelCategory[(int)channel].Pause();
        }

        public void ResumeChannel(AudioChannel channel)
        {
            _channelCategory[(int)channel].Resume();
        }

        public void Mute()
        {
            MuteChannel(AudioChannel.Music);
            MuteChannel(AudioChannel.Sfx);
        }

        public void MuteChannel(AudioChannel channel)
        {
            _channelMuted[(int)channel] = true;
            _channelCategory[(int)channel].SetVolume(0f);
        }

        public void UnMute()
        {
            UnMuteChannel(AudioChannel.Music);
            UnMuteChannel(AudioChannel.Sfx);
        }

        public void UnMuteChannel(AudioChannel channel)
        {
            _channelMuted[(int)channel] = false;
            _channelCategory[(int)channel].SetVolume(_channelVolume[(int)channel]);
        }

        public void SetChannelVolume(AudioChannel channel, float amount)
        {
            _channelVolume[(int)channel] = amount;
            _channelCategory[(int)channel].SetVolume(amount);
        }

        /// <summary>
        /// On a scale of 0 being mute to 1 being max.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="amount"></param>
        public void SetSoundVolume(string soundName, float amount)
        {
            ISound sound = FindSound(soundName);
            if (sound != null)
                sound.SetVolume(amount);
        }

        /// <summary>
        /// On a scale of 0 being slow, 0.5 normal, and 1 fast.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="amount"></param>
        public void SetSoundPitch(string soundName, float amount)
        {
            ISound sound = FindSound(soundName);
            if (sound != null)
                sound.SetPitch(amount);
        }

        public void ChangeMusic(string soundName, float outDuration, float inDuration)
        {
        }

        public void Update(DeltaTime time)
        {
            // don't complain if the audio engine isn't ready yet
            if (_audioEngine == null)
                return;

            _audioEngine.Update();

            foreach (ISound sound in _sounds)
            {
                if (!sound.Update(time))
                {
                    _soundsToRemove.Add(sound);
                }
            }

            foreach (ISound sound in _soundsToRemove)
            {
                sound.Recycle();
                _sounds.Remove(sound);
            }
            _soundsToRemove.Clear();

            for (int i = 0; i < (int)AudioChannel.Count; i++)
            {
                if (_channelFading[i])
                {
                    if (_channelFadeDuration[i] < _channelFadeElapsed[i])
                    {
                        _channelVolume[i] = MathHelper.SmoothStep(_channelVolume[i], _channelGoalVolume[i], _channelFadeElapsed[i] / _channelFadeDuration[i]);
                        _channelCategory[i].SetVolume(_channelVolume[i]);
                        _channelFadeElapsed[i] = time.ElapsedSeconds;
                    }
                    else
                    {
                        _channelVolume[i] = _channelGoalVolume[i];
                        _channelCategory[i].SetVolume(_channelVolume[i]);
                        _channelGoalVolume[i] = 0f;
                        _channelFadeElapsed[i] = 0f;
                        _channelFadeDuration[i] = 0f;
                        _channelFading[i] = false;
                    }
                }
            }
        }
    }
}
