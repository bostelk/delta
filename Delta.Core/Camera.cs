using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    /// <summary>
    /// TODO: Add rotation support, add queue zoom support, fix transitions, add interpolation method support.
    /// </summary>
    public class Camera : Entity
    {
        //float _rotationRate = 0;
        //float _goalRotation;
        float _desiredScale;
        Vector2 _desiredPosition;
        float _translationRate;
        float _scaleRate;
        float _scaleDuration;
        float _scaleElapsed;
        float _moveDuration;
        float _moveElapsed;
        float _shakeDuration;
        float _shakeElapsed;
        float _shakeMagnitude;
        float _shakeSpeed;
        float _shakeLastDirectionChange;
        float _shakeAngle;
        float _shakeDistance;
        Vector2 _shakeOffset;
        Vector2 _shakeAnchorPosition;
        ShakeAxis _shakeOptions;
        ShakeMode _shakeMode;
        float _flashElapsed;
        float _flashDuration;
        Color _flashColor = Color.White;

        float _scale;
        public float Scale
        {
            get { return _scale;}
            private set
            {
                if (_scale != value)
                {
                    _scale = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        float _rotation;
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = (PixelFix) ? DeltaMath.SimpleRound(value) : value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Vector2 _offset;
        public Vector2 Offset
        {
            get
            { return _offset; }
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        public CameraMode Mode { get; set; }

        /// <summary>
        /// The area in the world the camera is locked inside.
        /// </summary>
        public Rectangle BoundedArea { get; set; }

        public bool StayInsideBounds { get; set; }

        private Point Size
        {
            get
            {
                float invZoom = 1 / Scale; // careful, if  zoom is zero...
                Point result = Point.Zero;
                result.X = (int)((float)G.ScreenArea.Width * invZoom);
                result.Y = (int)((float)G.ScreenArea.Height * invZoom);
                return result;
            }
        }

        /// <summary>
        /// The area of the world the camera is viewing.
        /// </summary>
        public Rectangle ViewingArea
        {
            get
            {
                return new Rectangle((int)Position.X - Size.X/2, (int)Position.Y - Size.Y/2, Size.X, Size.Y);
            }
        }

        ///// <summary>
        ///// May be needed for 3D applications; like Farseer.
        ///// </summary>
        //public Matrix FlippedVerticalView
        //{
        //    get
        //    {
        //        return View * Matrix.CreateScale(1, -1, 1);
        //    }
        //}

        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }
        public Matrix World { get; private set; }
        public TransformableEntity Tracking { get; set; }

        public bool IsTracking
        {
            get { return Tracking != null; }
        }

        /// <summary>
        /// Drawing a fraction of a pixel introduces visual glitches. Fix: Round to the nearest pixel.
        /// </summary>
        public bool PixelFix { get; set; }
        public bool IsMoving { get; private set; }
        public bool IsZooming { get; set; }
        public bool IsShaking { get; set; }
        public bool Filter { get; set; }
        public Color Tint { get; set; }
        
        public Camera() : base()
        {
            _scaleRate = 0.5f;
            //_rotationRate = 0.5f;
            _translationRate = 0.5f;
            Position = _desiredPosition = Vector2.Zero;
            Scale = _desiredScale = 1;
            Rotation = 0;
            Tint = Color.White;
            PixelFix = false; // broken.
        }

        protected internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Filter)
            {
                // depends on wheter the camera is drawn within the transformed batch
                //spriteBatch.Draw(DeltaGame.Instance.PixelTexture, DeltaGame.Instance.ScreenArea, Tint);
                spriteBatch.Draw(G.PixelTexture, ViewingArea, Tint);
            }
            if (_flashDuration > _flashElapsed)
            {
                // TODO: Link with ColorExtensions.
                Color temp = _flashColor.SetAlpha(_flashElapsed / _flashDuration);
                //temp.A = (byte) ((_flashElapsed / _flashDuration) * 255.0f);

                // depends on wheter the camera is drawn within the transformed batch
                //spriteBatch.Draw(DeltaGame.Instance.PixelTexture, DeltaGame.Instance.ScreenArea, _flashColor);
                spriteBatch.Draw(G.PixelTexture, ViewingArea, _flashColor);
            }
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            // are we following the tracking entity, the player, or the goal position?
            if (IsTracking && Mode != CameraMode.FollowPlayer)
            {
                _position.X = MathHelper.SmoothStep(_position.X, Tracking.Position.X, _translationRate);
                _position.Y = MathHelper.SmoothStep(_position.Y, Tracking.Position.Y, _translationRate);
                NeedsHeavyUpdate = true;
            }
            else if (!IsTracking && _moveDuration > 0.0f)
            {
                if (_moveElapsed > _moveDuration)
                {
                    _moveElapsed = 0;
                    _moveDuration = 0;
                    _desiredPosition = Vector2.Zero;
                }
                else
                {
                    _moveElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    float amount = MathHelper.Clamp(_moveElapsed / _moveDuration, 0.0f, 1.0f);
                    _position.X = MathHelper.SmoothStep(_position.X, _desiredPosition.X, amount);
                    _position.Y = MathHelper.SmoothStep(_position.Y, _desiredPosition.Y, amount);
                    NeedsHeavyUpdate = true;
                }
            }
            //else if (false && Mode == CameraMode.FollowPlayer)
            //{
            //    Entity player = null; // getPlayerEntity();

            //    _position.X = MathHelper.SmoothStep(_position.X, player.Position.X, _translationRate);
            //    _position.Y = MathHelper.SmoothStep(_position.Y, player.Position.Y, _translationRate);
            //}

            UpdateShake((float)gameTime.ElapsedGameTime.TotalSeconds);
            UpdateScale((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (StayInsideBounds)
            {
                _position.X = MathHelper.Clamp(_position.X, BoundedArea.Left, BoundedArea.Right);
                _position.Y = MathHelper.Clamp(_position.X, BoundedArea.Top, BoundedArea.Bottom);
                NeedsHeavyUpdate = true;
            }

            if (_flashDuration > _flashElapsed)
            {
                _flashElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;    
            }
            else
            {
                _flashDuration = 0;
                _flashElapsed = 0;
            }
        }

        private void UpdateScale(float seconds)
        {
            if (_scaleDuration > _scaleElapsed)
            {
                _scaleElapsed += seconds;
                float amount = MathHelper.Clamp(_scaleElapsed / _scaleDuration, 0.0f, 1.0f);
                //float amount = MathHelper.Clamp((float) Math.Pow(0.02, (double) seconds / _zoomElapsed), 0.0f, 1.0f);
                Scale = MathHelper.SmoothStep(Scale, _desiredScale, amount);
                NeedsHeavyUpdate = true;
            }
            else
            {
                _scaleElapsed = 0.0f;
                _scaleDuration = 0.0f;

                float traveledDistance = _scaleRate * seconds;
                float distanceToGoal = _desiredScale - Scale;
                if (Math.Abs(distanceToGoal) > traveledDistance)
                {
                    if (distanceToGoal < 0)
                    {
                        Scale -= traveledDistance;
                    }
                    else
                    {
                        Scale += traveledDistance;
                    }
                    NeedsHeavyUpdate = true;
                }
                else
                {
                    Scale = _desiredScale; // always finish on an integer.
                    IsZooming = false;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        private void UpdateShake(float seconds)
        {
            if (_shakeDuration > _shakeElapsed)
            {
                _shakeElapsed += seconds;

                if (_shakeMode == ShakeMode.Random)
                {
                    float progress = _shakeElapsed / _shakeDuration;
                    float magnitude = _shakeMagnitude * (1f - (progress * progress)); //NON-LINEAR (x^2)
                    //float magnitude = _shakeMagnitude * (1f - (progress)); //LINEAR
                    _shakeOffset = new Vector2( G.Random.NextFloat() , G.Random.NextFloat()) * magnitude;

                    //if ((_shakeOptions & ShakeOptions.Horizontal) == ShakeOptions.Horizontal)
                    //    _shakeOffset.X = _shakeMagnitude * ((float)_random.NextDouble() * 1280.0f * 2.0f - 1280.0f);
                    //if ((_shakeOptions & ShakeOptions.Vertical) == ShakeOptions.Vertical)
                    //    _shakeOffset.Y = _shakeMagnitude * ((float)_random.NextDouble() * 720.0f * 2.0f - 720.0f);
                }
                else if (_shakeMode == ShakeMode.Directional)
                {
                    float distance = _shakeSpeed * seconds;
                    Vector2 newOffset = Camera.Shift(_shakeOffset, _shakeAngle, distance);

                    if ((_shakeOptions & ShakeAxis.X) == ShakeAxis.X)
                        _shakeOffset.X = newOffset.X;
                    if ((_shakeOptions & ShakeAxis.Y) == ShakeAxis.Y)
                        _shakeOffset.Y = newOffset.Y;

                    float num = _shakeOffset.LengthSquared();
                    if (_shakeElapsed > (_shakeLastDirectionChange + -1f) && num > (_shakeDistance * _shakeDistance))
                    {
                        double angle = Camera.AngleBetween(_position + _shakeOffset, _position);
                        double num2 = 0.0;
                        _shakeAngle = (float)Camera.ClampAngle(angle + num2);
                        _shakeLastDirectionChange = _shakeElapsed;
                    }
                    else
                    {
                        /*
                        if (this.mCameraShakeTTL > 0f && iDeltaTime > 1.401298E-45f)
                        {
                            float num11;
                            MathApproximation.FastSin(MathHelper.WrapAngle(this.mCameraShakeTTL * 60f), out num11);
                            this.mCurrentPosition.Y = this.mCurrentPosition.Y + num11 * this.mCameraShakeMagnitude * (this.mCameraShakeTTL / this.mStartCameraShakeTTL) * 0.0333f;
                        }
                        */
                    }
                }
                NeedsHeavyUpdate = true;
            }
            else
            {
                _shakeOffset = Vector2.Zero;
                _shakeElapsed = 0;
                _shakeDuration = 0;
                _shakeAnchorPosition = Vector2.Zero;
                IsShaking = false;
            }
        }

        protected internal override void HeavyUpdate(GameTime gameTime)
        {
            base.HeavyUpdate(gameTime);
            UpdateView();
            UpdateProjection();
            UpdateWorld();
        }

        protected virtual void UpdateView()
        {
            if (PixelFix)
            {
                View = Matrix.CreateTranslation(DeltaMath.SimpleRound(-Position.X + _shakeOffset.X), DeltaMath.SimpleRound(-Position.Y + _shakeOffset.Y), 0) *
                    Matrix.CreateRotationZ(DeltaMath.SimpleRound(Rotation)) *
                    Matrix.CreateScale(Scale) *
                    Matrix.CreateTranslation(DeltaMath.SimpleRound(Offset.X), DeltaMath.SimpleRound(Offset.Y), 0);
            }
            else
            {
                View = Matrix.CreateTranslation(-Position.X + _shakeOffset.X, -Position.Y + _shakeOffset.Y, 0) *
                  Matrix.CreateRotationZ(Rotation) *
                  Matrix.CreateScale(Scale) *
                  Matrix.CreateTranslation(Offset.X, Offset.Y, 0);
            }
        }

        protected virtual void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographicOffCenter(0, G.ScreenArea.Width, G.ScreenArea.Height, 0, 0, 1);
        }

        protected virtual void UpdateWorld()
        {
            World = Matrix.Identity;
        }

        public void Follow(TransformableEntity target)
        {
            if (target == null)
                return;
            Tracking = target;
            Mode = CameraMode.TrackTarget;
        }

        public void StopFollow()
        {
            Mode = CameraMode.Stationary;
            Tracking = null;
        }

        public void MoveToOverDuration(Vector2 position, float duration)
        {
            if (_position == position)
                return;

            _desiredPosition = position;
            _moveDuration = duration;
            _moveElapsed = 0;
            Tracking = null;
        }

        public void MoveToImmediate(Vector2 position)
        {
            if (_desiredPosition == position)
                return;

            Mode = CameraMode.Stationary;

            // imeditately move to position
            _position = position;
        }

        public void ZoomImmediate(float amount)
        {
            Scale = _desiredScale = amount;
        }

        /// <summary>
        /// Zoom in or out by this amount. The speed is determined by the ZoomRate.
        /// </summary>
        /// <param name="amount">Amount to Zoom in or out by.</param>
        public void ZoomByAmount(float amount)
        {
            IsZooming = true;
            _desiredScale = amount + Scale;
        }

        /// <summary>
        /// Zoom to this level within the given duration.
        /// </summary>
        /// <param name="zoom">Zoom Level</param>
        /// <param name="duration">Duration of zoom in seconds.</param>
        public void ZoomOverDuration(float zoom, float duration)
        {
            if (_desiredScale == zoom)
                return;

            IsZooming = true;
            _desiredScale = zoom;
            _scaleDuration = duration;
        }

        public void Shake(float magnitude, float duration, ShakeAxis options)
        {
            Shake(magnitude, duration, ShakeMode.Random, options);
        }

        public void Shake(float magnitude, float duration, ShakeMode mode, ShakeAxis options)
        {
            IsShaking = true;
            _shakeMagnitude = magnitude;
            _shakeMode = mode;
            _shakeOptions = options;
            _shakeDuration = duration;
            _shakeAnchorPosition = _position;
            _shakeSpeed = 600f;
            _shakeDistance = 30f;
            _shakeAngle = 1.5707963267948966f;
        }

        public void Shake(float distance, float angle, float speed, float duration, ShakeMode mode, ShakeAxis options)
        {
        }

        public void StopShake()
        {
            IsShaking = false;
        }

        public void Flash(Color color)
        {
            _flashDuration = 0.100f; // 100ms
        }

        public void Flash(Color color, float duration)
        {
            _flashColor = color;
            _flashElapsed = 0;
            _flashDuration = duration;
        }

        /// <summary>
        /// Test to see if the position is inside the viewing area.
        /// </summary>
        /// <param name="position">Position to test.</param>
        /// <returns></returns>
        public bool IsViewable(Vector2 position)
        {
            return ViewingArea.Contains((int)position.X, (int)position.Y);
        }

        /// <summary>
        /// Test to see if the rectangle is inside the viewing area.
        /// </summary>
        /// <param name="rectangle">Rectangle to test.</param>
        /// <returns></returns>
        public bool IsViewable(Rectangle rectangle)
        {
            return ViewingArea.Intersects(rectangle) || ViewingArea.Contains(rectangle.Center.X, rectangle.Center.Y);
        }

        public bool IsViewable(TransformableEntity e)
        {
            return IsViewable(e.Position); // TODO: Add hitbox testing too.
        }

        /// <summary>
        /// Transform a position relative to the screen to a position in the world.
        /// </summary>
        /// <param name="screenPosition">Position on the screen</param>
        /// <returns></returns>
        public Vector2 ToWorldPosition(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition - G.ScreenCenter, View) - Offset;
        }

        /// <summary>
        /// Transform a position relative to the worl to a position on the screen.
        /// </summary>
        /// <param name="worldPosition">Position in the world</param>
        /// <returns></returns>
        public Vector2 ToScreenPosition(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, Matrix.Invert(View));
        }

        #region HACK: THESE SHOULDN'T BE HERE
        public static Vector2 Shift(Vector2 location, double angle, float distance)
        {
            location.X += (float)(Math.Cos(angle) * (double)distance);
            location.Y -= (float)(Math.Sin(angle) * (double)distance);
            return location;
        }

        public static double ClampAngle(double angle)
        {
            return angle - 6.2831853071795862 * Math.Floor(angle / 6.2831853071795862);
        }

        public static double AngleBetween(Vector2 location1, Vector2 location2)
        {
            float xDistance = location2.X - location1.X;
            float num = location2.Y - location1.Y;
            num *= -1f;
            return Camera.AngleBetween(num, xDistance);
        }

        public static double AngleBetween(float yDistance, float xDistance)
        {
            double angle = Math.Atan2((double)yDistance, (double)xDistance);
            return Camera.ClampAngle(angle);
        }
        #endregion

    }
}

/// <summary>
/// Flags to control which axis to shake.
/// </summary>
[Flags]
public enum ShakeAxis
{
    /// <summary>
    /// Specifies to shake the X-Axis.
    /// </summary>
    X = 2 << 0,

    /// <summary>
    /// Specifies to shake the Y-Axis.
    /// </summary>
    Y = 2 << 1,
}

public enum ShakeMode
{
    Random,
    Directional,
    Modes
}

public enum CameraMode
{
    FollowPlayer,
    TrackTarget,
    Stationary,
    Modes
}
