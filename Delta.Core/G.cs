using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Input;
using Delta.Audio;
using Microsoft.Xna.Framework.Content;
using Delta.Graphics.Effects;

using Delta.Content;
using Delta.Graphics;
using Delta.Physics;

namespace Delta
{
    public abstract class G : Microsoft.Xna.Framework.Game
    {
        bool _isInitialized = false;
        internal static Dictionary<string, object> _contentReferences = new Dictionary<string, object>();

        public static G Instance { get; private set; }
        public new static GraphicsDevice GraphicsDevice { get; set; }
        public new static DeltaContentManager Content { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static PrimitiveBatch PrimitiveBatch { get; private set; }
        public static InputManager Input { get; private set; }
        public static AudioManager Audio { get; private set; }
        public static DeltaPhysics Physics { get; private set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public static Texture2D PixelTexture { get; private set; }
        public static SpriteFont Font { get; private set; }
        public static Random Random { get; private set; }
        public static World World { get; private set; }
        public static UI UI { get; private set; }
        public static Rectangle ScreenArea { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }
        public static BlendEffect BlendEffect { get; private set; }
        public static SimpleEffect SimpleEffect { get; private set; }

        public static T GetContent<T>(string assetName)
        {
            if (_contentReferences.ContainsKey(assetName))
                return (T)_contentReferences[assetName];
            return default(T);
        }

        ResourceContentManager _embedded;

        public G() : base()
        {
#if DEBUG
            IsMouseVisible = true;
#endif
            Instance = this;
            ScreenArea = new Rectangle(0, 0, 1280, 720); // need this information from the start
            ScreenCenter = ScreenArea.Center.ToVector2(); // need this information from the start
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferredBackBufferWidth = ScreenArea.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = ScreenArea.Height;
            base.Content.RootDirectory = "Content";
            _embedded = new ResourceContentManager(Services, EmbeddedContent.ResourceManager);
            Random = new Random();
            World = new World();
            UI = new UI();
            Input = new InputManager();
            Audio = new AudioManager(@"Content\Audio\audio.xgs", @"Content\Audio\Sound Bank.xsb", @"Content\Audio\Wave Bank.xwb", @"Content\Audio\StreamingBank.xwb");
            Physics = new DeltaPhysics();
            G.Content = new Delta.Content.DeltaContentManager(base.Content.ServiceProvider, base.Content.RootDirectory);
        }

        protected override void LoadContent()
        {
            GraphicsDevice = base.GraphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            PrimitiveBatch = new PrimitiveBatch(GraphicsDevice);
            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData<Color>(new Color[] { Color.White });
            Font = _embedded.Load<SpriteFont>("TinyFont");
            BlendEffect = new BlendEffect(_embedded.Load<Effect>("BlendEffect"));
            SimpleEffect = new SimpleEffect(_embedded.Load<Effect>("SimpleEffect"));
            ScreenArea = GraphicsDevice.Viewport.Bounds;
            ScreenCenter = ScreenArea.Center.ToVector2();
            base.LoadContent();
            GC.Collect(); // force a collection after content is loaded.
            ResetElapsedTime(); // avoid the update loop trying to play catch-up
        }

        void InternalInitialize()
        {
            _isInitialized = true;
            LateInitialize();
        }

        protected virtual void LateInitialize()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // HANLDE WITH CARE. LaterInitializes need a preprare audio engine!!
            SimpleEffect.Time = (float)gameTime.TotalGameTime.TotalMilliseconds;
            Input.Update(gameTime);
            Audio.Update(gameTime);
            Physics.Simulate((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (!_isInitialized) 
                InternalInitialize();
            World.Update(gameTime);
            UI.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            World.Draw(gameTime, SpriteBatch);
            UI.Draw(gameTime, SpriteBatch);
        }
    }
}
