using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calssinheriatane
{
    internal class Dog : Creature, IFourLeggedWalker
    {

        public override void Breath()
        {
            Console.WriteLine($"개가 숨을 쉰다");
        }
        public void FourLeggedWalk()
        {
            throw new NotImplementedException();
        }
    }
}
