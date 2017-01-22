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
        List<String> phrasePieces;
        Keys previousCharEnum;

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
            startPosition = new Vector2(100, 50);

            colors = new List<Color>();
            colors.Add(Color.White);
            colors.Add(Color.Red);
            colors.Add(Color.Black);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            char currentChar = phrasePieces[1][0];
            Keys currentCharEnum = KeyFromChar(currentChar);
            bool currentCharLower = LowerStatus(currentChar);
            //Console.WriteLine(currentCharEnum);

            bool keyPressed = false;

            // Debug statements for pressing keys
            /*if(state.GetPressedKeys().Length > 0)
            {
                Console.WriteLine(state.GetPressedKeys()[0]);
                if (state.GetPressedKeys()[0] == currentCharEnum && !keyHold)
                {
                    Console.Write("Pressed: ");
                    Console.WriteLine(currentCharEnum);
                }
            }*/

            // Checks if the key was pressed with proper modifiers
            if(state.IsKeyDown(currentCharEnum))
            {
                if (currentCharEnum == Keys.Space)
                {
                    Console.WriteLine("Pressed Quot");
                    Console.WriteLine(currentCharLower);
                    
                }
                if (currentCharEnum == Keys.Space)
                    keyPressed = true;

                if(state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
                {
                    if (!currentCharLower)
                        keyPressed = true;
                }
                else
                {
                    if (currentCharLower)
                        keyPressed = true;
                }
            }

            if(keyPressed && !keyHold)
            {
                if (phrasePieces[2].Length == 0)
                {
                    typed = true;
                }
                else
                {
                    phrasePieces[0] = phrasePieces[0] + phrasePieces[1];
                    phrasePieces[1] = phrasePieces[2][0].ToString();
                    phrasePieces[2] = phrasePieces[2].Substring(1);
                    previousCharEnum = currentCharEnum;

                    if (phrasePieces[1] == currentChar.ToString())
                        keyHold = true;
                }
            }

            if (state.IsKeyUp(previousCharEnum))
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

        public bool Typed
        {
            get { return typed; }
        }

        public bool LowerStatus(char character)
        {
            if (char.IsDigit(character))
                return true;
            if (char.IsLetter(character))
                return char.IsLower(character);

            switch(character.ToString())
            {
                case "'":
                case ";":
                case "[":
                case "]":
                case "=":
                case ",":
                case ".":
                case "/":
                case "`":
                case "\\": return true;
            }

            return false;
        }

        public Keys KeyFromChar(char character)
        {
            if(char.IsDigit(character) || char.IsLetter(character))
                return (Keys)((int)(char.ToUpper(character)));

            Keys key;
            switch (character.ToString())
            {
                case "'": case "\"": key = Keys.OemQuotes; break;
                case ";": case ":": key = Keys.OemSemicolon; break;
                case "[": case "{": key = Keys.OemOpenBrackets; break;
                case "]": case "}": key = Keys.OemCloseBrackets; break;
                case "-": case "_": key = Keys.OemMinus; break;
                case "=": case "+": key = Keys.OemPlus; break;
                case ",": case "<": key = Keys.OemComma; break;
                case ".": case ">": key = Keys.OemPeriod; break;
                case "/": case "?": key = Keys.OemQuestion; break;
                case "|": case "\\": key = Keys.OemPipe; break;
                case "`": case "~": key = Keys.OemTilde; break;

                case "!": key = Keys.D1; break;
                case "@": key = Keys.D2; break;
                case "#": key = Keys.D3; break;
                case "$": key = Keys.D4; break;
                case "%": key = Keys.D5; break;
                case "^": key = Keys.D6; break;
                case "&": key = Keys.D7; break;
                case "*": key = Keys.D8; break;
                case "(": key = Keys.D9; break;
                case ")": key = Keys.D0; break;

                case " ": key = Keys.Space; break;

                default: key = Keys.Oem8; break;
            }

            return key;
        }
    }
}
