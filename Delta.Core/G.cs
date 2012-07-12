using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Delta.Input;
using Delta.Audio;
using Delta.Graphics;
using Delta.Graphics.Effects;
using Delta.Collision;

namespace Delta
{
    public struct DeltaTime
    {
        public float TotalSeconds { get; internal set; }
        public float ElapsedSeconds { get; internal set; }
        public bool IsRunningSlowly { get; internal set; }
    }

    public class G : Game
    {
        internal static ResourceContentManager _embedded = null;
        internal static GraphicsDeviceManager _graphicsDeviceManager = null;
        internal static bool _lateInitialized = false;

        public new static ContentManager Content { get; private set; }
        public static InputManager Input { get; private set; }
        public static AudioManager Audio { get; private set; }
        public static World World { get; private set; }
        public static HUD HUD { get; private set; }

        public new static GraphicsDevice GraphicsDevice { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static PrimitiveBatch PrimitiveBatch { get; private set; }
        public static CollisionEngine Collision { get; private set; }
        public static Texture2D PixelTexture { get; private set; }
        public static SpriteFont Font { get; private set; }
        public static Random Random { get; private set; }
        public static Rectangle ScreenArea { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }

        public static DeltaEffect DeltaEffect { get; private set; }
        public static SimpleEffect SimpleEffect { get; private set; }

        public G(int screenWidth, int screenHeight)
            : base()
        {
            Content = base.Content;
            Content.RootDirectory = "Content";
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.PreferredBackBufferWidth = screenWidth;
            _graphicsDeviceManager.PreferredBackBufferHeight = screenHeight;
            GraphicsDevice = _graphicsDeviceManager.GraphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            PrimitiveBatch = new PrimitiveBatch(GraphicsDevice);
            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData<Color>(new Color[] { Color.White });
            ScreenArea = GraphicsDevice.Viewport.Bounds;
            ScreenCenter = ScreenArea.Center.ToVector2();
#if DEBUG
            IsMouseVisible = true;
#endif
            _embedded = new ResourceContentManager(Services, EmbeddedContent.ResourceManager);
            Font = _embedded.Load<SpriteFont>("TinyFont");
            DeltaEffect = new DeltaEffect(_embedded.Load<Effect>("DeltaEffect"));
            SimpleEffect = new SimpleEffect(_embedded.Load<Effect>("SimpleEffect"));
            Random = new Random();
            Input = new InputManager();
            Audio = new AudioManager(@"Content\Audio\audio.xgs", @"Content\Audio\Sound Bank.xsb", @"Content\Audio\Wave Bank.xwb", @"Content\Audio\StreamingBank.xwb");
            Collision = new CollisionEngine();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GC.Collect();
            ResetElapsedTime();
        }

        protected virtual void LateInitialize()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            G.Input.Update(gameTime);
            G.Audio.Update(gameTime);
            if (_lateInitialized)
            {
                _lateInitialized = true;
                LateInitialize();
            }
            G.World.Update(gameTime);
            G.HUD.Update(gameTime);
            G.Collision.Simulate((float)gameTime.ElapsedGameTime.TotalSeconds); // simulate after the world update! otherwise simulating a previous frame's worldstate.
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            G.World.Draw();
            G.HUD.Draw();
        }
       
    }
}
