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

        Random rand = new Random();

        bool win = false;
        bool isGameOver = false;

        public PhraseManager()
        {
            unusedPhrases = new List<Phrase>();
            usedPhrases = new List<Phrase>();

            unusedPhrases.Add(new Phrase("GGggrump", 0));
            unusedPhrases.Add(new Phrase("boss", 0));
            unusedPhrases.Add(new Phrase("finger", 0));
            unusedPhrases.Add(new Phrase("doggo", 0));
            unusedPhrases.Add(new Phrase("It's happening!", 0));
        }

        public void Initialize()
        {
            foreach (Phrase p in unusedPhrases)
            {
                p.Initialize();
            }

            chooseNewPhrase();
        }

        private void chooseNewPhrase()
        {
            if (unusedPhrases.Count == 0)
            {
                EndGame(true);
                return;
            }

            int index = rand.Next(unusedPhrases.Count - 1);
            currentPhrase = unusedPhrases[index];
            unusedPhrases.RemoveAt(index);
        }

        public Phrase GetCurrentPhrase()
        {
            return currentPhrase;
        }

        public void EndGame(bool winner)
        {
            isGameOver = true;
            win = winner;
        }

        public bool IsGameOver()
        {
            return isGameOver;
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("TypeTest");
        }

        public void Update(GameTime gameTime)
        {
            currentPhrase.Update(gameTime);

            if (currentPhrase.Typed)
            {
                usedPhrases.Add(currentPhrase);
                chooseNewPhrase();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (!isGameOver)
            {
                currentPhrase.Draw(batch, font);
            }
            else
            {
                batch.Begin();
                if (win)
                {
                    batch.DrawString(font, "You're Winner!", new Vector2(100, 50), Color.Green);
                }
                else
                {
                    batch.DrawString(font, "You're Loser!", new Vector2(100, 50), Color.Red);
                }
                batch.End();
            }
        }
    }
}