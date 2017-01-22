using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        List<string> phrasePieces;

        Vector2 startPosition;
        Vector2 offset;
        List<Color> colors;

        bool keyHold = false;
        bool typed = false;

        public Phrase(String phrase, int difficulty)
        {
            this.phrase = phrase;
            this.difficulty = difficulty;
        }

        public void Initialize()
        {
            phrasePieces = new List<String>();
            phrasePieces.Add("");
            phrasePieces.Add(phrase[0].ToString());
            phrasePieces.Add(phrase.Substring(1));

            offset = Vector2.Zero;
            startPosition = new Vector2(100);

            colors = new List<Color>();
            colors.Add(Color.White);
            colors.Add(Color.Red);
            colors.Add(Color.Black);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            char currentKey = phrasePieces[1][0];
            Keys currentKeyEnum = (Keys)((int)(char.ToUpper(currentKey)));

            bool currentKeyLower = false;

            // Determine if it's a lower or upper use of the Key
            if (char.IsLower(currentKey))
                currentKeyLower = true;

            bool keyPressed = false;

            // Checks if the key was pressed with proper modifiers
            if(state.IsKeyDown(currentKeyEnum))
            {
                if(state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
                {
                    if (!currentKeyLower)
                        keyPressed = true;
                }
                else
                {
                    if (currentKeyLower)
                        keyPressed = true;
                }
            }

            if(keyPressed && !keyHold)
            {
                keyHold = true;
                if (phrasePieces[2].Length == 0)
                {
                    typed = true;
                }
                else
                {
                    phrasePieces[0] = phrasePieces[0] + phrasePieces[1];
                    phrasePieces[1] = phrasePieces[2][0].ToString();
                    phrasePieces[2] = phrasePieces[2].Substring(1);
                }
            }

            if (state.GetPressedKeys().Length == 0)
                keyHold = false;
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            batch.Begin();
            if (!typed)
            {
                for (int i = 0; i < phrasePieces.Count; i++)
                {
                    batch.DrawString(font, phrasePieces[i], startPosition + offset, colors[i]);
                    offset.X += font.MeasureString(phrasePieces[i]).X;
                }
                offset = Vector2.Zero;
            }
            else
            {
                batch.DrawString(font, phrase, startPosition, Color.Green);
            }
            batch.End();
        }
    }
}
