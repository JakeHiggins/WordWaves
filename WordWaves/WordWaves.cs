using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordWaves;

namespace WordWaves
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class WordWaves : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PhraseManager phraseManager;
        Texture2D pixel; //a brush for drawing rectangles
        Texture2D villagerTx;

        public WordWaves()
        {
            graphics = new GraphicsDeviceManager(this);
            phraseManager = new PhraseManager();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            phraseManager.Initialize();
            graphics.PreferredBackBufferWidth = 338;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            villagerTx = Content.Load<Texture2D>("red");

            // TODO: use this.Content to load your game content here
            phraseManager.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            phraseManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //helper variables
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;
            Vector2 screenSize = new Vector2(screenWidth, screenHeight);
            float tt = (float)gameTime.TotalGameTime.TotalSeconds;

            //
            //draw scenery
            //
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            int y = 0;

            //draw the clouds (atleast 2)
            int cloudCount = 4;
            float cloudRange = screenSize.X / (cloudCount - 1);
            int cloudWidth = (int)(cloudRange - screenSize.X * 0.1f);
            int cloudHeight = (int)(screenSize.Y * 0.05f);
            for (int i = 0; i < cloudCount; ++i)
            {
                //scroll clouds
                float cloudSpeed = 5;
                int cloudX = (int)(cloudRange * i - (tt * cloudSpeed) % cloudRange);
                spriteBatch.Draw(pixel, new Rectangle(cloudX, y, cloudWidth, cloudHeight), Color.Gray);
            }
            y = (int)(screenSize.Y * 0.15f);

            //draw the sun
            int sun_width = 16;
            int sun_height = 8;
            int sun_x = (int)(screenSize.X / 2 - sun_width / 2);
            spriteBatch.Draw(pixel, new Rectangle(sun_x, y, sun_width, sun_height), Color.Yellow);
            y += sun_height;

            //draw the ocean
            int sea_height = (int)(screenSize.Y * 0.5f);
            spriteBatch.Draw(pixel, new Rectangle(0, y, screenWidth, sea_height), Color.Blue);
            y += sea_height;

            //draw the beach
            int beach_height = screenHeight - y;
            spriteBatch.Draw(pixel, new Rectangle(0, y, screenWidth, beach_height), Color.Tan);

            //draw the villagers
            y += (int)(screenSize.Y * 0.05f);
            for (int i = 0; i < 26; ++i )
            {
                int villagerX = 0;
                int villagerY = 0;
                if(i < 10)
                {
                    villagerX = i;
                    villagerY = 0;
                }else if(i < 19)
                {
                    villagerX = (i - 10);
                    villagerY = 1;
                }else if(i < 26)
                {
                    villagerX = (i - 19);
                    villagerY = 2;
                }
                float villagerRange = screenSize.X / 10;
                int villagerSpacing = 3;
                int villagerWidth = (int)villagerRange - (villagerSpacing*2);
                int villagerHeight = (int)villagerRange - (villagerSpacing*2);
                float vx = (float)villagerX * villagerRange + villagerSpacing;
                float vy = y + (float)villagerY * villagerRange;
                vx += (float)villagerY * villagerRange * 0.25f;
                spriteBatch.Draw(villagerTx, new Rectangle((int)vx, (int)vy, villagerWidth, villagerHeight), Color.White);

            }

                spriteBatch.End();

            // TODO: Add your drawing code here
            phraseManager.Draw(spriteBatch);


            base.Draw(gameTime);
        }
    }
}