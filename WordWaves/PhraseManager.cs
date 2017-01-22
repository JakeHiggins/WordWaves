using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordWaves;

namespace WordWaves
{
    class PhraseManager
    {
        private SpriteFont font;
        private List<Phrase> unusedPhrases;
        private List<Phrase> usedPhrases;

        private Phrase currentPhrase;

        public PhraseManager()
        {
            unusedPhrases = new List<Phrase>();
            usedPhrases = new List<Phrase>();
        }

        public void Initialize()
        {
            unusedPhrases.Add(new Phrase("grump", 0));
            unusedPhrases.Add(new Phrase("boss", 0));
            unusedPhrases.Add(new Phrase("finger", 0));
            unusedPhrases.Add(new Phrase("doggo", 0));
            unusedPhrases.Add(new Phrase("It's happening!", 0));

            currentPhrase = unusedPhrases[0];
            unusedPhrases.RemoveAt(0);
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("TypeTest");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            currentPhrase.Draw(batch, font);
            batch.End();
        }
    }
}
