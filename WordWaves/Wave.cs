using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordWaves
{
    public class Wave
    {
        public bool active = false;
        public const float maxWaveProgress = 1.6f;
        public const float waveSpeed = 0.5f;
        public float waveProgress = 0;
        public float waveTimeElapsed = 0;
        public Texture2D waveSmallTx;

        public Wave()
        {
        }

        public void LoadContent(ContentManager Content)
        {
            waveSmallTx = Content.Load<Texture2D>("wave");
        }

        public void Start()
        {
            active = true;
        }

        public void Update(GameTime gameTime)
        {
            float et = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (active)
            {
                waveTimeElapsed += et;
                waveProgress = waveTimeElapsed * waveSpeed;
                if (waveProgress > maxWaveProgress)
                {
                    waveProgress = maxWaveProgress;
                    active = false;
                }
            }
        }

        public void Reset()
        {
            waveTimeElapsed = 0;
        }

        public void Destroy()
        {
            active = false;
        }

        public void Draw(SpriteBatch spriteBatch, int startY, int endY)
        {
            if (active)
            {
                int screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
                int screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
                Vector2 screenSize = new Vector2(screenWidth, screenHeight);
                int waveWidth = (int)(screenSize.X * 0.1f);
                int waveHeight = waveWidth;
                float top0 = startY;
                float bot0 = startY + waveHeight;
                float top1 = endY;
                float bot1 = screenHeight;
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
            }
        }
    }
}
