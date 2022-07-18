using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiecGame
{
    internal class TileInfo_Star : TileInfo
    {
        public int getStars=1;
        public override void TileEvent()
        {
            base.TileEvent();
            getStars++;
        }
    }
}
