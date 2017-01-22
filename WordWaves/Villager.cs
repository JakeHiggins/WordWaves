using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordWaves
{
    public class Villager
    {
        public int keyboardColumn = 0;
        public int keyboardRow = 0;
        public Keys key = Keys.None;
        public bool readyToWhip = false;

        public Villager(Keys associatedKey, int column, int row)
        {
            key = associatedKey;
            keyboardColumn = column;
            keyboardRow = row;
        }
    }
}
