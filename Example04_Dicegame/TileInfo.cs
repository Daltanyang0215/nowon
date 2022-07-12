using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example04_Dicegame
{
    internal class TileInfo
    {
        public int index;
        public string name;
        public string discription;

        public virtual void OnTileEvent()
        {
            Console.WriteLine($"{index} 번째 칸에 도착 : {name}, {discription}");
        }

    }
}
