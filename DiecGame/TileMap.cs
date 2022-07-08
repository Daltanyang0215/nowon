using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiecGame
{
    internal class TileMap
    {
        public Dictionary<int, TileInfo> tiles = new Dictionary<int, TileInfo>();

        public void setTlieMap(int tliemapcount)
        {
            for (int i = 0; i < tliemapcount; i++)
            {
                if (i % 5 == 0)
                {
                    TileInfo addtile = new TileInfo_Star();
                    addtile.index = i;
                    addtile.name = "STAR";
                    addtile.discription = "별 칸 이다";
                    tiles.Add(i, addtile);
                }
                else
                {
                    TileInfo addtile = new TileInfo();
                    addtile.index = i;
                    addtile.name = "Demmy";
                    addtile.discription = "일반 칸 이다 ";
                    tiles.Add(i, addtile);

                }
            }
        }

    }
}
