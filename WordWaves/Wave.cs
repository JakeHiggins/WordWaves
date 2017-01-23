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
        protected float waveProgressOld = 0;
        public float waveProgress = 0;
        public float waveTimeElapsed = 0;

        public Wave()
        {
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
                waveProgressOld = waveProgress;
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

        /// <summary>
        /// Determine whether the wave reached a certain point in its progression
        /// </summary>
        /// <param name="progressPoint">The progress we are awaiting.</param>
        /// <returns>Whether the wave hit this point in the last frame</returns>
        public bool CheckOverlap(float progressPoint)
        {
            //warn(tim): this doesnt cuurrently handle if progressOld > progress 
            return progressPoint > waveProgressOld && progressPoint <= waveProgress;
        }

        public void Draw(SpriteBatch spriteBatch, int startY, int endY, Texture2D waveTexture)
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
                waveRect.Width = (int)MathHelper.Lerp(waveWidth, screenWidth/4, waveProgress);
                waveRect.X = (screenWidth / 2) - waveRect.Width / 2;
                Rectangle waveTilingRect = new Rectangle(waveRect.X, 0, waveRect.Width, waveTexture.Height);
                spriteBatch.Draw(waveTexture, waveRect, waveTilingRect, Color.PowderBlue);
            }
        }
    }
}
