using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        Texture2D foamTx;
        Texture2D seaTx;
        Texture2D oceanGradientTx;
        Texture2D houseTx;
        Texture2D waveSmallTx;
        string qwertyLayout = "qwertyuiopasdfghjklzxcvbnm";
        Villager[] villagers;
        KeyboardState keystateCurrent, keystateOld;
        SamplerState samplerState = SamplerState.LinearWrap;

        public WordWaves()
        {
            graphics = new GraphicsDeviceManager(this);
            phraseManager = new PhraseManager();
            Content.RootDirectory = "Content";
            //create villagers based on a qwerty keyboard
            villagers = new Villager[26];
            for(int i = 0; i < villagers.Length; ++i)
            {
                Keys villagerKey = (Keys)(int)char.ToUpper(qwertyLayout[i]);
                int villagerX = 0;
                int villagerY = 0;
                if (i < 10)
                {
                    villagerX = i;
                    villagerY = 0;
                }
                else if (i < 19)
                {
                    villagerX = (i - 10);
                    villagerY = 1;
                }
                else if (i < 26)
                {
                    villagerX = (i - 19);
                    villagerY = 2;
                }
                villagers[i] = new Villager(villagerKey, villagerX, villagerY);
            }
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

            // Load textures
            villagerTx = Content.Load<Texture2D>("red");
            foamTx = Content.Load<Texture2D>("foam");
            seaTx = Content.Load<Texture2D>("sea");
            oceanGradientTx = Content.Load<Texture2D>("ocean-gradient");
            houseTx = Content.Load<Texture2D>("house");
            waveSmallTx = Content.Load<Texture2D>("wave");
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

            keystateOld = keystateCurrent;
            keystateCurrent = Keyboard.GetState();

            // TODO: Add your update logic here
            Phrase currentPhrase = phraseManager.GetCurrentPhrase();
            phraseManager.Update(gameTime);

            //
            //update villagers
            //
            foreach(Villager v in villagers)
            {
                if (currentPhrase.Typed)
                {
                    v.readyToWhip = false;
                }
                else
                {
                    if (!v.readyToWhip && keystateCurrent.IsKeyDown(v.key) && keystateOld.IsKeyUp(v.key))
                    {
                        v.readyToWhip = true;
                    }
                }
            }

            //
            // Toggle draw modes
            //
            if(keystateCurrent.IsKeyDown(Keys.D1) && keystateOld.IsKeyUp(Keys.D1))
            {
                samplerState = SamplerState.PointClamp;
            }
            if(keystateCurrent.IsKeyDown(Keys.D2) && keystateOld.IsKeyUp(Keys.D2))
            {
                samplerState = SamplerState.LinearClamp;
            }
            if (keystateCurrent.IsKeyDown(Keys.D3) && keystateOld.IsKeyUp(Keys.D3))
            {
                samplerState = SamplerState.AnisotropicClamp;
            }

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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, samplerState);
            int y = 0;

            //draw the clouds (atleast 3)
            int cloudCount = 16;
            float cloudRange = screenSize.X / (cloudCount - 2);
            int cloudWidth = (int)(cloudRange) + 5;// - screenSize.X * 0.1f);
            int cloudHeight = (int)(screenSize.Y * 0.05f);
            for (int i = 0; i < cloudCount; ++i)
            {
                //scroll clouds
                float cloudSpeed = 5;
                int cloudX = (int)(cloudRange * i - (tt * cloudSpeed) % (cloudRange*2));
                int cloudY = y - (i % 2) * (cloudHeight / 4);
                spriteBatch.Draw(pixel, new Rectangle(cloudX, cloudY, cloudWidth, cloudHeight), Color.Gray);
            }
            y = (int)(screenSize.Y * 0.15f);

            //draw the sun
            int sun_width = 16;
            int sun_height = 8;
            int sun_x = (int)(screenSize.X / 2 - sun_width / 2);
            spriteBatch.Draw(pixel, new Rectangle(sun_x, y, sun_width, sun_height), Color.Yellow);
            y += sun_height;

            //draw the sea in the distance
            int seaY = y;
            int seaTileSize = (int)Math.Ceiling(screenSize.X * 0.1f);
            int SeaTileColumns = (int)Math.Ceiling(screenSize.X / (float)seaTileSize);
            for (int s = 0; s < SeaTileColumns; ++s)
            {
                spriteBatch.Draw(seaTx, new Rectangle(s * seaTileSize, y, seaTileSize, seaTileSize), Color.White);
            }
            y += seaTileSize;

            //draw the ocean
            int ocean_height = (int)(screenSize.Y * 0.4f);
            spriteBatch.Draw(oceanGradientTx, new Rectangle(0, y, screenWidth, ocean_height), Color.White);
            y += ocean_height;

            //draw the foam
            int foamSize = (int)Math.Ceiling(screenSize.X * 0.1f);
            int foamColumns = (int)Math.Ceiling(screenSize.X / (float)foamSize);
            for (int f = 0; f < foamColumns; ++f)
            {
                spriteBatch.Draw(foamTx, new Rectangle(f * foamSize, y, foamSize, foamSize), Color.White);
            }
            y += foamSize;

            //draw the beach
            int beachY = y;
            int beach_height = screenHeight - y;
            Color beachColor = new Color(255, 241, 146, 255);
            spriteBatch.Draw(pixel, new Rectangle(0, y, screenWidth, beach_height), beachColor);

            //draw the villagers
            y += (int)(screenSize.Y * 0.025f);
            float villagerRange = screenSize.X / 10;
            int villagerSpacing = 3;
            int villagerWidth = (int)villagerRange - (villagerSpacing * 2);
            int villagerHeight = (int)villagerRange - (villagerSpacing * 2);
            for (int i = 0; i < 26; ++i )
            {
                //position in QWERTY formation
                Villager villager = villagers[i];
                float vx = (float)villager.keyboardColumn * villagerRange + villagerSpacing;
                float vy = y + (float)villager.keyboardRow * villagerRange;
                vx += (float)villager.keyboardRow * villagerRange * 0.25f;
                Color villager_color = Color.White;
                if (villager.readyToWhip)
                {
                    villager_color = Color.Goldenrod;
                    vy -= villagerRange * 0.1f;
                }
                spriteBatch.Draw(villagerTx, new Rectangle((int)vx, (int)vy, villagerWidth, villagerHeight), villager_color);

            }

            //draw the village
            y += (int)(3 * villagerRange + screenSize.Y * 0.025f);
            int villageHouseCount = 7;
            int villageHouseRange = (int)(screenSize.X / (float)villageHouseCount);
            int villageHouseSpacing = 5;
            int villageHouseSize = villageHouseRange - villageHouseSpacing * 2;
            for (int h = 0; h < villageHouseCount; ++h )
            {
                int villageHouseX = h * villageHouseRange + villageHouseSpacing;
                spriteBatch.Draw(houseTx, new Rectangle(villageHouseX, y, villageHouseSize, villageHouseSize), Color.White);
            }

            //draw the waves
            int waveWidth = (int)(screenSize.X * 0.1f);
            int waveHeight = waveWidth;
            float top0 = seaY;
            float bot0 = seaY + waveHeight;
            float top1 = beachY;
            float bot1 = screenHeight;
            float waveSpeed = 0.5f;
            //slow->fast->slow
            float maxWaveProgress = 1.6f;
            float waveProgress = (tt * waveSpeed) % maxWaveProgress;
            float exponential4 = (float)Math.Pow(waveProgress, 4);
            float translationProgress = exponential4;
            if (waveProgress > 1)
                translationProgress = waveProgress;
            float top = MathHelper.Lerp(top0, top1, translationProgress);
            float bot = MathHelper.Lerp(bot0, bot1, translationProgress);
            Rectangle waveRect = new Rectangle();
            waveRect.Y = (int)top;
            waveRect.Height = (int)(bot - top);
            waveRect.Width = (int)MathHelper.Lerp(waveWidth, screenWidth, waveProgress);
            waveRect.X = (screenWidth / 2) - waveRect.Width / 2;
            Rectangle waveTilingRect = new Rectangle(waveRect.X, 0, waveRect.Width, waveSmallTx.Height);
            spriteBatch.Draw(waveSmallTx, waveRect, waveTilingRect, Color.PowderBlue);

                spriteBatch.End();

            // TODO: Add your drawing code here
            phraseManager.Draw(spriteBatch);


            base.Draw(gameTime);
        }
    }
}