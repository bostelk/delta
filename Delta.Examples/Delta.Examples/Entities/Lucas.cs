using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Delta;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Delta.Input;
using Microsoft.Xna.Framework.Content;
using Delta.Audio;

namespace Delta.Examples.Entities
{
    public class Lucas : Entity
    {
        const float BOOST_ZOOM = 3.3f;
        const float NORMAL_ZOOM = 4f;
        const float BOOST_SPEED = 160f;
        const float NORMAL_SPEED = 80f;

        float _speed = NORMAL_SPEED;
        float _decel = 0.1f;
        float _accel =  0.2f;
        float _rotation = 0f;
        float _boostElapsed = 0;
        float _boostDuration = 2f;
        bool _isBoosting = false;

        bool _isRightCooldown;
        bool _isLeftCooldown;

        GameHud HUD;
        Texture2D _sheet;

        public Vector2 Velocity;

        public Lucas() : base("Lucas")
        {
        }

        protected override void LoadContent()
        {
            _sheet = G.Content.Load<Texture2D>(@"Graphics\Player");
            base.LoadContent();
        }

        protected override void LateInitialize()
        {
            HUD = Entity.Get("GameHud") as GameHud;
            HUD.Alpha = 0f;

            G.World.Camera.ZoomImmediate(BOOST_ZOOM);
            G.World.Camera.Offset = G.ScreenCenter;
            G.World.Camera.Follow(this);
            G.World.Camera.ZoomOverDuration(NORMAL_ZOOM, 3f);
            G.Audio.SetSoundVolume("SFX_Ambiance_1", 0.8f);
            base.LateInitialize();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            Velocity.X = MathHelper.SmoothStep(Velocity.X, 0, _decel);
            Velocity.Y = MathHelper.SmoothStep(Velocity.Y, 0, _decel);
            if (G.Input.Keyboard.Held(Keys.Up))
            {
                float speedx = (float)Math.Cos(_rotation) * _speed;
                float speedy = (float)Math.Sin(_rotation) * _speed;
                Velocity.Y = MathHelper.SmoothStep(Velocity.Y, speedy, _accel);
                Velocity.X = MathHelper.SmoothStep(Velocity.X, speedx, _accel);
            }
            if (G.Input.Keyboard.Held(Keys.Down))
            {
                float speedx = (float)Math.Cos(_rotation) * -_speed;
                float speedy = (float)Math.Sin(_rotation) * -_speed;
                Velocity.Y = MathHelper.SmoothStep(Velocity.Y, speedy, _accel);
                Velocity.X = MathHelper.SmoothStep(Velocity.X, speedx, _accel);
            }
            if (G.Input.Keyboard.Held(Keys.Left))
            {
                _rotation = MathHelper.WrapAngle(_rotation - 0.08f);
            }
            if (G.Input.Keyboard.Held(Keys.Right))
            {
                _rotation = MathHelper.WrapAngle(_rotation + 0.08f);
            }
            Position += Velocity * G.World.Time.ElapsedSeconds;

            if (G.Input.Keyboard.JustPressed(Keys.Space) && !_isBoosting && G.World.Camera.Scale == NORMAL_ZOOM)
            {
                G.World.Camera.Flash(Color.White);
                G.World.Camera.Shake(10f, 0.5f, ShakeAxis.X | ShakeAxis.Y);
                G.Audio.PlaySound("SFX_LargeExplosion");
                G.Audio.FadeChannelByAmount(AudioChannel.Music, 0.5f, 0.4f);
                G.Audio.SetSoundVolume("SFX_Ambiance_1", 1f);
                G.World.Camera.ZoomByAmount(BOOST_ZOOM - NORMAL_ZOOM);
                _isBoosting = true;
            }

            if (_isBoosting)
            {
                HUD.Alpha = MathHelper.SmoothStep(1f, 0f, (NORMAL_ZOOM - G.World.Camera.Scale) / Math.Abs(BOOST_ZOOM - NORMAL_ZOOM));
                _boostElapsed += G.World.Time.ElapsedSeconds;
                _speed = BOOST_SPEED;
                if (_boostElapsed > _boostDuration && !G.Input.Keyboard.Held(Keys.Space))
                {
                    _speed = NORMAL_SPEED;
                    _boostElapsed = 0;
                    _isBoosting = false;
                    G.World.Camera.ZoomByAmount(NORMAL_ZOOM - BOOST_ZOOM);
                    G.Audio.FadeChannelIn(AudioChannel.Music, 0.5f);
                    G.Audio.SetSoundVolume("SFX_Ambiance_1", 0.8f);
                }
            }
            else if (!_isBoosting && HUD.Alpha < 1)
            {
                HUD.Alpha = MathHelper.SmoothStep(0f, 1f, 1 - (NORMAL_ZOOM - G.World.Camera.Scale) / Math.Abs(BOOST_ZOOM - NORMAL_ZOOM));
            }
            else
            {
                if (G.Input.Keyboard.Held(Keys.LeftControl) && !_isLeftCooldown) // charging
                {
                    G.Audio.PlaySound("SFX_Charge", false);
                    G.Audio.SetSoundPitch("SFX_Charge", HUD.LeftFillPercent);
                    HUD.LeftFillPercent = MathHelper.Lerp(HUD.LeftFillPercent, 1, 0.01f);
                }
                else if (G.Input.Keyboard.JustReleased(Keys.LeftControl)) // fired
                {
                    G.Audio.StopSound("SFX_Charge");
                    G.Audio.PlaySound("SFX_Laser", true);
                    G.Audio.SetSoundVolume("SFX_Laser", HUD.LeftFillPercent);
                    _isLeftCooldown = true;
                }
                else if (!G.Input.Keyboard.Held(Keys.LeftControl)) // cooldown
                {
                    HUD.LeftFillPercent = MathHelper.SmoothStep(HUD.LeftFillPercent, 0, 0.1f);
                    if (HUD.LeftFillPercent < 0.3)
                        _isLeftCooldown = false;
                }

                if (G.Input.Keyboard.Held(Keys.RightControl) && !_isRightCooldown) // charging
                {
                    G.Audio.PlaySound("SFX_Charge", false);
                    G.Audio.SetSoundPitch("SFX_Charge", HUD.RightFillPercent);
                    HUD.RightFillPercent = MathHelper.Lerp(HUD.RightFillPercent, 1, 0.01f);
                }
                else if (G.Input.Keyboard.JustReleased(Keys.RightControl)) // fired
                {
                    G.Audio.StopSound("SFX_Charge");
                    G.Audio.PlaySound("SFX_Laser", true);
                    G.Audio.SetSoundVolume("SFX_Laser", HUD.RightFillPercent);
                    _isRightCooldown = true;
                }
                else if (!G.Input.Keyboard.Held(Keys.RightControl)) // cooldown
                {
                    HUD.RightFillPercent = MathHelper.SmoothStep(HUD.RightFillPercent, 0, 0.1f);
                    if (HUD.RightFillPercent < 0.3)
                        _isRightCooldown = false;
                }
            }
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(_sheet.Width / 2, _sheet.Height / 2);
            spriteBatch.Draw(_sheet, Position, null, Color.White, _rotation + MathHelper.Pi * 0.5f, offset, 1, SpriteEffects.None, 0);

            Vector2 direction = Velocity;
            direction.Normalize();
            if (_isLeftCooldown)
                spriteBatch.DrawLine(Position, _rotation, 500, Color.DarkGreen.SetAlpha(HUD.LeftFillPercent), HUD.LeftFillPercent * 20);
            else if (_isRightCooldown)
                spriteBatch.DrawLine(Position, _rotation, 500, Color.DarkGreen.SetAlpha(HUD.RightFillPercent), HUD.RightFillPercent * 20);
        }

    }
}
