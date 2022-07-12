using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example04_Dicegame
{
    internal class TilemMap
    {
        
        public Dictionary<int, TileInfo> TileMap = new Dictionary<int,TileInfo>();

        
        public TilemMap(int tileCount)
        {
            for (int i = 1; i < tileCount+1; i++)
            {
                if (i % 5 == 0)
                {
                    TileInfo add = new TileInfo_Star();
                    add.index = i;
                    add.name = "샛별";
                    add.discription = "샛별칸입니다";
                    TileMap.Add(add.index, add);
                }
                else
                {
                    TileInfo add = new TileInfo();
                    add.index = i;
                    add.name = "일반";
                    add.discription = "일반칸입니다";
                    TileMap.Add(add.index, add);
                }
            }
        }


    }
}
