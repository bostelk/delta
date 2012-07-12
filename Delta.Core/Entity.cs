using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Delta.Movement;
using Delta.Structures;
using Delta.Collision;
using Delta.Collision.Geometry;
using Delta.Physics;

namespace Delta
{
    [Flags]
    internal enum EntityState
    {
        None = 0x0,
        Enabled = 0x1,
        Visible = 0x2,
        LateInitialized = 0x4,
        LoadedContent = 0x8,
        Updating = 0x10,
        Drawing = 0x20,
        NeedsHeavyUpdate = 0x40,
        RemoveOnNextUpdate = 0x80
    }

    public class Entity : EntityBase
    {
        public static Entity Get(string id)
        {
            id = id.ToLower();
            if (EntityCollection._idReferences.ContainsKey(id))
                return EntityCollection._idReferences[id];
            return null;
        }

        #region TEMP: Transformer
        Transformer _transformer;

        [ContentSerializer]
        public bool _fadeRandom { get; set; } // won't serialize fields? wtf. totally temp.
        OverRange _fadeRange;
        [ContentSerializer]
        public OverRange FadeRange
        {
            get { return _fadeRange; }
            set
            {
                if (!value.IsEmpty())
                {
                    _fadeRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    if (_fadeRandom)
                    {
                        // start the transformer off in a random position within the range.
                        float startupAlpha = G.Random.Between(_fadeRange.Lower, _fadeRange.Upper);
                        _transformer = Transformer.ThisEntity(this).FadeTo(startupAlpha, (startupAlpha / _fadeRange.Upper) * _fadeRange.Duration);
                        _transformer.OnTransformFinished(() =>
                        {
                            // remove the start-up transform logic.
                            _transformer.ClearSequence();
                            _transformer.OnTransformFinished(null);
                            // 50/50 chance to fade from lower to upper or from upper to lower. provides more fade varieties.
                            if (G.Random.FiftyFifty())
                                _transformer.FadeTo(_fadeRange.Upper, _fadeRange.Duration).FadeTo(_fadeRange.Lower, _fadeRange.Duration);
                            else
                                _transformer.FadeTo(_fadeRange.Lower, _fadeRange.Duration).FadeTo(_fadeRange.Upper, _fadeRange.Duration);
                            _transformer.Loop();
                        });
                    }
                    else
                    {
                        _transformer = Transformer.ThisEntity(this).FadeTo(_fadeRange.Lower, _fadeRange.Duration).FadeTo(_fadeRange.Upper, _fadeRange.Duration);
                        _transformer.Loop();
                    }
                }
            }
        }

        OverRange _flickerRange;
        [ContentSerializer]
        public OverRange FlickerRange
        {
            get { return _flickerRange; }
            set
            {
                if (!value.IsEmpty())
                {
                    _flickerRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    _transformer = Transformer.ThisEntity(this).FlickerFor(_flickerRange.Lower, _flickerRange.Upper, _flickerRange.Duration);
                    _transformer.Loop();
                }
            }
        }

        OverRange _blinkRange;
        [ContentSerializer]
        public OverRange BlinkRange
        {
            get { return _blinkRange; }
            set
            {
                if (!value.IsEmpty())
                {
                    _blinkRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    _transformer = Transformer.ThisEntity(this).BlinkFor(_blinkRange.Lower, _blinkRange.Duration);
                    _transformer.Loop();
                }
            }
        }
        #endregion

        internal EntityState _state = EntityState.Enabled | EntityState.Visible;

        [ContentSerializer]
        public string ID { get; internal set; }
        [ContentSerializerIgnore]
        protected Vector2 RenderOrigin { get; private set; }
        [ContentSerializerIgnore]
        protected float RenderRotation { get; private set; }

        [ContentSerializerIgnore]
        public bool LateInitialized
        {
            get { return _state.HasFlag(EntityState.LateInitialized); }
            set
            {
                if (value)
                    _state |= EntityState.LateInitialized;
                else
                    _state &= ~EntityState.LateInitialized;
            }
        }

        [ContentSerializerIgnore]
        public bool LoadedContent
        {
            get { return _state.HasFlag(EntityState.LoadedContent); }
            set
            {
                if (value)
                    _state |= EntityState.LoadedContent;
                else
                    _state &= ~EntityState.LoadedContent;
            }
        }

        [ContentSerializerIgnore]
        public bool IsUpdating
        {
            get { return _state.HasFlag(EntityState.Updating); }
            set
            {
                if (value)
                    _state |= EntityState.Updating;
                else
                    _state &= ~EntityState.Updating;
            }
        }

        [ContentSerializerIgnore]
        public bool IsDrawing
        {
            get { return _state.HasFlag(EntityState.Drawing); }
            set
            {
                if (value)
                    _state |= EntityState.Drawing;
                else
                    _state &= ~EntityState.Drawing;
            }
        }

        [ContentSerializerIgnore]
        public bool NeedsHeavyUpdate
        {
            get { return _state.HasFlag(EntityState.NeedsHeavyUpdate); }
            set
            {
                if (value)
                    _state |= EntityState.NeedsHeavyUpdate;
                else
                    _state &= ~EntityState.NeedsHeavyUpdate;
            }
        }

        [ContentSerializer]
        public bool IsEnabled
        {
            get { return _state.HasFlag(EntityState.Enabled); }
            set
            {
                if (value)
                    _state |= EntityState.Enabled;
                else
                    _state &= ~EntityState.Enabled;
            }
        }

        [ContentSerializer]
        public bool IsVisible
        {
            get { return _state.HasFlag(EntityState.Visible); }
            set
            {
                if (value)
                    _state |= EntityState.Visible;
                else
                    _state &= ~EntityState.Visible;
            }
        }

        [ContentSerializerIgnore]
        public bool RemoveOnNextUpdate
        {
            get { return _state.HasFlag(EntityState.RemoveOnNextUpdate); }
            set
            {
                if (value)
                    _state |= EntityState.RemoveOnNextUpdate;
                else
                    _state &= ~EntityState.RemoveOnNextUpdate;
            }
        }

        Vector2 _position = Vector2.Zero;
        [ContentSerializer]
        public virtual Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPositionChanged();
                }
            }
        }

        Vector2 _offset = Vector2.Zero;
        [ContentSerializer]
        public virtual Vector2 Offset
        {
            get { return _offset; }
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    OnPositionChanged();
                }
            }
        }

        Vector2 _size = Vector2.Zero;
        [ContentSerializer]
        public virtual Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnSizeChanged();
                }
            }
        }

        Vector2 _scale = Vector2.One;
        [ContentSerializer]
        public virtual Vector2 Scale
        {
            get { return _scale; }
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnScaleChanged();
                }
            }
        }

        float _rotation = 0.0f;
        [ContentSerializer]
        public virtual float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    OnRotationChanged();
                }
            }
        }

        Vector2 _origin = Vector2.Zero;
        [ContentSerializer]
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                if (_origin != value)
                {
                    _origin = value.Clamp(Vector2.Zero, Vector2.One);
                    OnOriginChanged();
                }
            }
        }

        Vector2 _pivot = Vector2.One * 0.5f;
        [ContentSerializer]
        public Vector2 Pivot
        {
            get { return _pivot; }
            set
            {
                if (_pivot != value)
                {
                    _pivot = value.Clamp(Vector2.Zero, Vector2.One);
                    OnPivotChanged();
                }
            }
        }

        [ContentSerializer(ElementName = "Tint")]
        Color _tint = Color.White;
        [ContentSerializerIgnore]
        public virtual Color Tint
        {
            get { return _tint * _alpha; }
            set
            {
                if (_tint != value)
                {
                    _tint = value;
                    OnTintChanged();
                }
            }
        }

        float _alpha = 1.0f;
        [ContentSerializer]
        public virtual float Alpha
        {
            get { return _alpha; }
            set
            {
                value = value.Clamp(0.0f, 1.0f);
                if (_alpha != value)
                {
                    _alpha = value;
                    OnAlphaChanged();
                }
            }
        }

        IWrappedBody _wrappedBody;
        [ContentSerializerIgnore]
        public IWrappedBody WrappedBody
        {
            get { return _wrappedBody; }
            set
            {
                if (value != null)
                {
                    _wrappedBody = value;
                    OnWrappedBodyChanged();
                }
            }
        }

        public Entity()
            : base()
        {
            ID = string.Empty;
            NeedsHeavyUpdate = true;
            RenderOrigin = Vector2.Zero;
            RenderRotation = 0.0f;
        }

        public Entity(string id)
            : this()
        {
            ID = id;
        }

        internal void InternalInitialize()
        {
            if (!LateInitialized)
            {
                if (G.GraphicsDevice == null)
                    return;
                LateInitialized = true;
                LateInitialize();
            }
        }

        protected virtual void LateInitialize()
        {
        }

        internal void InternalLoadContent()
        {
            if (!LoadedContent)
            {
                LoadedContent = true;
                LoadContent();
            }
        }

#if WINDOWS
        protected internal virtual bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "visible":
                case "isvisible":
                    IsVisible = bool.Parse(value);
                    return true;
                case "enabled":
                case "isenabled":
                    IsEnabled = bool.Parse(value);
                    return true;
                case "layer":
                case "order":
                case "draworder":
                case "updateorder":
                    Layer = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "pos":
                case "position":
                    Position = Vector2Extensions.Parse(value);
                    return true;
                case "offset":
                    Offset = Vector2Extensions.Parse(value);
                    return true;
                case "scale":
                    Scale = Vector2Extensions.Parse(value);
                    return true;
                case "rot":
                case "rotation":
                    Rotation = float.Parse(value, CultureInfo.InvariantCulture).ToRadians();
                    return true;
                case "tint":
                case "color":
                    Tint = value.ToColor();
                    return true;
                case "opacity":
                case "alpha":
                    Alpha = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "fade":
                    _fadeRange = OverRange.Parse(value);
                    return true;
                case "faderandom":
                    _fadeRange = OverRange.Parse(value);
                    _fadeRandom = true;
                    return true;
                case "flicker":
                    _flickerRange = OverRange.Parse(value);
                    return true;
                case "blink":
                    _blinkRange = OverRange.Parse(value);
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }
#endif

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void InternalUpdate(DeltaTime time)
        {
            if (RemoveOnNextUpdate)
            {
                RemoveOnNextUpdate = false;
                Remove();
            }
            if (!LateInitialized)
                InternalInitialize();
            if (!LoadedContent)
                InternalLoadContent();
            if (CanUpdate())
            {
                BeginUpdate(time);
                LightUpdate(time);
                if (NeedsHeavyUpdate)
                {
                    BeginHeavyUpdate(time);
                    HeavyUpdate(time);
                    EndHeavyUpdate(time);
                }
                EndUpdate(time);
            }
        }

        protected virtual bool CanUpdate()
        {
            if (!IsEnabled || IsUpdating) return false;
            return true;
        }

        protected virtual void LightUpdate(DeltaTime time)
        {
        }

        protected virtual void UpdateRenderPosition()
        {
            _renderPosition = Position + Offset + RenderOrigin - (Origin * Size * Scale);
        }

        protected virtual void UpdateRenderOrigin()
        {
            RenderOrigin = Pivot * Size * Scale;
        }

        protected virtual void UpdateRenderRotation()
        {
            RenderRotation = Rotation.ToRadians();
        }

        protected virtual void UpdateToWrappedBody()
        {
            if (WrappedBody != null)
            {
                WrappedBody.SimulationPosition = Position + Offset;
                WrappedBody.SimulationRotation = RenderRotation;
            }
        }

        protected virtual void UpdateFromWrappedBody()
        {
            if (WrappedBody != null)
            {
                //don't fire OnChanged functions! We don't want to get into a fight between reading and writing off the WrappedBody!
                _position = WrappedBody.SimulationPosition - Offset;
                _rotation = WrappedBody.SimulationRotation.ToDegrees();
                UpdateRenderPosition();
                UpdateRenderRotation();
            }
        }

        protected internal virtual void BeginUpdate(DeltaTime time)
        {
            IsUpdating = true;
        }

        protected internal virtual void EndUpdate(DeltaTime time)
        {
            IsUpdating = false;
        }

        protected internal virtual void BeginHeavyUpdate(DeltaTime time)
        {
            NeedsHeavyUpdate = false;
        }

        protected internal virtual void HeavyUpdate(DeltaTime time)
        {
        }

        protected internal virtual void EndHeavyUpdate(DeltaTime time)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void InternalDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            if (CanDraw())
            {
                BeginDraw(time, spriteBatch);
                Draw(time, spriteBatch);
                EndDraw(time, spriteBatch);
            }
        }

        protected virtual bool CanDraw()
        {
            if (!IsVisible || !LateInitialized || IsDrawing) return false;
            return true;
        }

        protected virtual void BeginDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            IsDrawing = true;
        }

        protected internal virtual void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
        }

        protected internal virtual void EndDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            IsDrawing = false;
        }

        protected internal virtual void OnPositionChanged()
        {
            UpdateRenderPosition();
            UpdateToWrappedBody();
        }

        protected internal virtual void OnSizeChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected internal virtual void OnScaleChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected internal virtual void OnRotationChanged()
        {
            UpdateRenderRotation();
            UpdateToWrappedBody();
        }

        protected virtual void OnOriginChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected virtual void OnPivotChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected virtual void OnTintChanged()
        {
        }

        protected virtual void OnAlphaChanged()
        {
        }

        protected virtual void OnWrappedBodyChanged()
        {
        }

        public override void Recycle()
        {
            base.Recycle();
            ID = string.Empty;
            RenderOrigin = Vector2.Zero;
            RenderRotation = 0.0f;
            _alpha = 1.0f;
            _offset = Vector2.Zero;
            _origin = Vector2.Zero;
            _pivot = Vector2.One * 0.5f;
            _position = Vector2.Zero;
            _rotation = 0.0f;
            _scale = Vector2.One;
            _size = Vector2.Zero;
            _state = EntityState.Enabled | EntityState.Visible;
            _tint = Color.White;
            _wrappedBody = null;
        }

    }
}
