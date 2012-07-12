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
using Delta.Collision;

namespace Delta
{
    public sealed class G
    {
        internal static Dictionary<string, object> _contentReferences = new Dictionary<string, object>();

        public static DeltaGame Instance { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static DeltaContentManager Content { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static PrimitiveBatch PrimitiveBatch { get; private set; }
        public static InputManager Input { get; private set; }
        public static AudioManager Audio { get; private set; }
        public static CollisionEngine Collision { get; private set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public static Texture2D PixelTexture { get; private set; }
        public static SpriteFont Font { get; private set; }
        public static Random Random { get; private set; }
        public static World World { get; private set; }
        public static UI UI { get; private set; }
        public static Rectangle ScreenArea { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }
        public static DeltaEffect DeltaEffect { get; private set; }
        public static SimpleEffect SimpleEffect { get; private set; }

        public static T GetContent<T>(string assetName)
        {
            if (_contentReferences.ContainsKey(assetName))
                return (T)_contentReferences[assetName];
            return default(T);
        }

        internal static void Setup(DeltaGame game, Rectangle screenArea)
        {
            Instance = game;
            ScreenArea = screenArea; // need this information from the start
            ScreenCenter = screenArea.Center.ToVector2(); // need this information from the start
            GraphicsDeviceManager = new GraphicsDeviceManager(game);
            GraphicsDeviceManager.PreferredBackBufferWidth = G.ScreenArea.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = G.ScreenArea.Height;
            Random = new Random();
            World = new World();
            UI = new UI();
            Input = new InputManager();
            Audio = new AudioManager(@"Content\Audio\audio.xgs", @"Content\Audio\Sound Bank.xsb", @"Content\Audio\Wave Bank.xwb", @"Content\Audio\StreamingBank.xwb");
            Collision = new CollisionEngine();
            Content = new Delta.Content.DeltaContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);
        }

        internal static void LoadContent(DeltaGame game, ResourceContentManager resources)
        {
            GraphicsDevice = game.GraphicsDevice;
            GraphicsDevice.DeviceReset += OnDeviceReset;
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            PrimitiveBatch = new PrimitiveBatch(GraphicsDevice);
            PixelTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            PixelTexture.SetData<Color>(new Color[] { Color.White });
            Font = resources.Load<SpriteFont>("TinyFont");
            DeltaEffect = new DeltaEffect(resources.Load<Effect>("DeltaEffect"));
            SimpleEffect = new SimpleEffect(resources.Load<Effect>("SimpleEffect"));
            ScreenArea = GraphicsDevice.Viewport.Bounds;
            ScreenCenter = ScreenArea.Center.ToVector2();
        }

        /// <summary>
        /// Called when the presentation parameters have changed. Examples: fullscreen, window is resized, or manual reset.
        /// </summary>
        internal static void OnDeviceReset(object sender, EventArgs e)
        {
            // xna will try to maintain the backbuffer resolution, however the monitor may not support it.
            // xna will then pick the next best resolution. eg. 1920x1080 fullscreened becomes 1600x900.
            // therefore the original resolution is not maintained and ScreenArea needs to update accordingly.
            ScreenArea = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            ScreenCenter = ScreenArea.Center.ToVector2();
            World.Camera.Offset = ScreenCenter;
        }
       
    }
}
