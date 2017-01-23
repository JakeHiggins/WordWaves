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
    public class WaveManager
    {
        public bool active = true;
        public float elapsedLevelTime = 0;
        public float timeSinceLastWave = 0;
        public float levelStartTime = 3;
        public List<Wave> waves = new List<Wave>();
        public Texture2D waveSmallTx;

        public void AddWave()
        {
            waves.Add(new Wave());
        }

        public void LoadContent(ContentManager Content)
        {
            waveSmallTx = Content.Load<Texture2D>("wave");
        }

        public void Update(GameTime gameTime)
        {
            if (!active)
                return;
            float et = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedLevelTime += et;
            timeSinceLastWave += et;
            for (int i = 0; i < waves.Count; ++i)
            {
                waves[i].Update(gameTime);
                if (!waves[i].active)
                {
                    waves.RemoveAt(i);
                    i--;
                }
            }
            if (ShouldSpawnWave())
            {
                SpawnWave(0);
            }
        }
        public bool ShouldSpawnWave()
        {
            if (elapsedLevelTime < levelStartTime)
                return false;
            if (timeSinceLastWave < 3)
                return false;
            return true;
        }
        public void SpawnWave(float xPercentage)
        {
            Wave newWave = new Wave();
            waves.Add(newWave);
            newWave.Start();
            timeSinceLastWave = 0;
        }
        public void Deactivate()
        {
            active = false;
        }

        public void Draw(SpriteBatch spriteBatch, int seaY, int beachY)
        {
            foreach(Wave w in waves)
            {
                w.Draw(spriteBatch, seaY, beachY, waveSmallTx);
            }
        }
    }
}
