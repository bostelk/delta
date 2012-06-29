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
using Delta.Input;
using Delta.Audio;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AudioExample : ExampleBase
    {
        const string _controlText = "[space] play.[enter] stop all.[left] previous sound.[right] next sound.[up] music volume inc.[down] music volume dec.[f1] mute/unmute.\n[f2] fade out.[f3] fade in.[num0] force play.[num8] sfx volume inc.[num2] sfx volume dec.[num7] pitch up.[num8] pitch dec.";
        Vector2 _controlTextPosition = new Vector2(640, 0);
        float _pitch = 0.5f;
        float _volume = 1f;

        public List<String> _sounds = new List<string>() {
            "BGM_Ice",
            "BGM_Evil",
            "SFX_Ambiance_1",
            "SFX_LargeExplosion",
            "SFX_Laser",
        };
        int _soundIndex = 0;

        public AudioExample() : base("AudioExample")
        {

        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void LateInitialize()
        {
            G.Audio.PlaySound(_sounds[_soundIndex]);
            base.LateInitialize();
        }

        protected override void Update(GameTime gameTime)
        {

            if (G.Input.Keyboard.JustPressed(Keys.Left))
            {
                G.Audio.StopAllSounds();
                _soundIndex = DeltaMath.Wrap(_soundIndex - 1, 0, _sounds.Count - 1);
                G.Audio.PlaySound(_sounds[_soundIndex]);
                _pitch = 0.5f;
                _volume = 1f;
            }
            if (G.Input.Keyboard.JustPressed(Keys.Right))
            {
                G.Audio.StopAllSounds();
                _soundIndex = DeltaMath.Wrap(_soundIndex + 1, 0, _sounds.Count - 1);
                G.Audio.PlaySound(_sounds[_soundIndex]);
                _pitch = 0.5f;
                _volume = 1f;
            }
            if (G.Input.Keyboard.Held(Keys.Up))
            {
                G.Audio.SetChannelVolume(MathHelper.SmoothStep(G.Audio.MusicVolume, 1.0f, 0.1f), AudioChannel.Music);
            }
            if (G.Input.Keyboard.Held(Keys.Down))
            {
                G.Audio.SetChannelVolume(MathHelper.SmoothStep(G.Audio.MusicVolume, 0.0f, 0.1f), AudioChannel.Music);
            }
            if (G.Input.Keyboard.Held(Keys.NumPad8))
            {
                G.Audio.SetChannelVolume(MathHelper.SmoothStep(G.Audio.SfxVolume, 1.0f, 0.1f), AudioChannel.Sfx); ;
            }
            if (G.Input.Keyboard.Held(Keys.NumPad2))
            {
                G.Audio.SetChannelVolume(MathHelper.SmoothStep(G.Audio.SfxVolume, 0.0f, 0.1f), AudioChannel.Sfx);
            }
            if (G.Input.Keyboard.Held(Keys.NumPad7))
            {
                G.Audio.SetSoundPitch(_sounds[_soundIndex], _pitch = MathHelper.Clamp(_pitch+0.01f, 0,1f));
            }
            if (G.Input.Keyboard.Held(Keys.NumPad1))
            {
                G.Audio.SetSoundPitch(_sounds[_soundIndex], _pitch = MathHelper.Clamp(_pitch-0.01f, 0,1f));
            }
            if (G.Input.Keyboard.Held(Keys.NumPad9))
            {
                G.Audio.SetSoundVolume(_sounds[_soundIndex], _volume = MathHelper.Clamp(_volume+0.01f, 0,1f));
            }
            if (G.Input.Keyboard.Held(Keys.NumPad3))
            {
                G.Audio.SetSoundVolume(_sounds[_soundIndex], _volume = MathHelper.Clamp(_volume-0.01f, 0,1f));
            }
            if (G.Input.Keyboard.JustPressed(Keys.Space))
            {
                if (G.Audio.IsPlaying(_sounds[_soundIndex]))
                    G.Audio.PauseSound(_sounds[_soundIndex]);
                else
                    G.Audio.ResumeSound(_sounds[_soundIndex]);
            }
            if (G.Input.Keyboard.JustPressed(Keys.NumPad0))
            {
                G.Audio.PlaySound(_sounds[_soundIndex], true);
            }
            if (G.Input.Keyboard.JustPressed(Keys.Enter))
            {
                G.Audio.StopSound(_sounds[_soundIndex]);
                G.Audio.PlaySound(_sounds[_soundIndex]);
                G.Audio.SetSoundPitch(_sounds[_soundIndex], _pitch);
                G.Audio.SetSoundVolume(_sounds[_soundIndex], _volume);
            }
            if (G.Input.Keyboard.JustPressed(Keys.RightShift))
            {
                G.Audio.PlaySound(_sounds[_soundIndex], true);
                G.Audio.SetSoundPitch(_sounds[_soundIndex], _pitch);
                G.Audio.SetSoundVolume(_sounds[_soundIndex], _volume);
            }
            if (G.Input.Keyboard.JustPressed(Keys.F1))
            {
                if (G.Audio.IsMuted)
                    G.Audio.Mute();
                else
                    G.Audio.UnMute();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDeviceManager.GraphicsDevice.Clear(ClearColor);
            SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
            SpriteBatch.DrawString(G.Font, _controlText, _controlTextPosition, Color.White, TextAlignment.Center);
            SpriteBatch.DrawString(G.Font, InfoText, new Vector2(0, 50), Color.White);
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        public string InfoText
        {
            get
            {
                return string.Format("Currently playing: {0}\nTime: {1}/{2}\nMusic Volume: {3}\nSfx Volume: {4}\nIsMuted: {5}\nIsPlaying: {6}\nSound Pitch: {7}\nSound Volume: {8}", _sounds[_soundIndex], "", "", G.Audio.MusicVolume, G.Audio.SfxVolume, G.Audio.IsMuted, "", _pitch, _volume);
            }
        }
    }
}
