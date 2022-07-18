using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calssinheriatane
{
    internal class Human : Creature , ITwoLeggedWalker
    {
        public string name;
        public float height;

        public override void Breath()
        {
            Console.WriteLine($"{name} (이)가 숨을 쉰다");
        }

        public virtual void Grow()
        {
            mass += 10f;
            height += 5f;

            Console.WriteLine($"{name} 이 자랐다 ! . |{mass},{height}|");

        }

        public void TwoLeggedWalk()
        {
            Console.WriteLine($"{name} 이가 이족보행 한다!");
        }
    }
}
