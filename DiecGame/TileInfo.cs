using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiecGame
{
    internal class TileInfo
    {
        public int index;
        public string name;
        public string discription;
        virtual public void TileEvent()
        {
            index =index == 0 ? 20 : index;
            Console.WriteLine($"Tile number : {index}, The  Player is On {name} , {discription}");
        }

    }
}
