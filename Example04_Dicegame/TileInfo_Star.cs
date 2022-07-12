using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example04_Dicegame
{
    internal class TileInfo_Star :TileInfo
    {

        public int starValue = 3;

        public override void OnTileEvent()
        {
            base.OnTileEvent();
            starValue++;
        }

    }
}
