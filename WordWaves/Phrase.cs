using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordWaves
{
    class Phrase
    {
        String phrase;
        int difficulty;

        public Phrase(String phrase, int difficulty)
        {
            this.phrase = phrase;
            this.difficulty = difficulty;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            batch.DrawString(font, phrase, new Vector2(100, 100), Color.White);
        }
    }
}
